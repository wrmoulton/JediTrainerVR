using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class FutureSightEnemiesXR : MonoBehaviour
{
    [Header("Input (Right Controller)")]
    public XRNode controllerNode = XRNode.RightHand;   // Right hand
    // Quest Right controller B = secondaryButton

    [Header("Prediction")]
    public float futureTime = 1.25f;                   // seconds ahead
    public float updateRate = 0.10f;                   // refresh interval
    public LayerMask enemyLayer;

    [Header("Visuals")]
    public GameObject ghostPrefab;                     // transparent sphere/quad prefab
    public float ghostScale = 0.25f;
    public float yOffset = 0.10f;

    [Header("Force Drain (Required)")]
    public PlayerStats playerStats;
    public float forceCostPerSecond = 12f;

    [Header("Debug")]
    public bool debugLogs = false;

    private InputDevice rightController;
    private float nextUpdateTime;

    private readonly Dictionary<FlyingChase, GameObject> ghostByChase = new();

    void Start()
    {
        if (!playerStats) playerStats = FindFirstObjectByType<PlayerStats>();
        InitController();

        if (debugLogs) Debug.Log("[FUTURE SIGHT] Started. Hold B to see enemy future positions (drains Force).");
    }

    void InitController()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
        if (devices.Count > 0) rightController = devices[0];
    }

    void Update()
    {
        if (!rightController.isValid) InitController();

        // B on Quest RIGHT controller = secondaryButton
        bool bHeld = false;
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bHeld);

        if (!bHeld)
        {
            DisableAllGhosts();
            return;
        }

        if (!playerStats)
        {
            if (debugLogs) Debug.LogWarning("[FUTURE SIGHT] No PlayerStats found.");
            DisableAllGhosts();
            return;
        }

        // Drain Force continuously while held
        float cost = forceCostPerSecond * Time.deltaTime;
        if (!playerStats.TrySpendForce(cost))
        {
            if (debugLogs) Debug.Log("[FUTURE SIGHT] Not enough Force.");
            DisableAllGhosts();
            return;
        }

        if (Time.time < nextUpdateTime) return;
        nextUpdateTime = Time.time + updateRate;

        UpdateGhosts();
    }

    void UpdateGhosts()
    {
        FlyingChase[] chasers = FindObjectsByType<FlyingChase>(FindObjectsSortMode.None);

        var seen = new HashSet<FlyingChase>();

        foreach (var chase in chasers)
        {
            if (!chase || !chase.isActiveAndEnabled) continue;
            if (!chase.target) continue;

            // Layer filter (enemy root should be on Enemy layer)
            if (((1 << chase.gameObject.layer) & enemyLayer) == 0) continue;

            seen.Add(chase);

            Vector3 predicted = PredictFuturePosition(chase, futureTime);
            predicted.y += yOffset;

            GameObject ghost = GetOrCreateGhost(chase);
            ghost.transform.position = predicted;
            ghost.transform.localScale = Vector3.one * ghostScale;
            if (!ghost.activeSelf) ghost.SetActive(true);
        }

        // Disable ghosts for chasers not seen now
        var keys = new List<FlyingChase>(ghostByChase.Keys);
        foreach (var k in keys)
        {
            if (!seen.Contains(k) && ghostByChase[k] != null)
                ghostByChase[k].SetActive(false);
        }

        if (debugLogs) Debug.Log($"[FUTURE SIGHT] Updated ghosts for {seen.Count} enemies.");
    }

    Vector3 PredictFuturePosition(FlyingChase chase, float t)
    {
        // Replicate FlyingChase behavior: lock Y, move toward target, stop at stopDistance
        Vector3 enemyPos = chase.transform.position;
        enemyPos.y = chase.lockedY;

        Vector3 targetPos = chase.target.position;
        targetPos.y = chase.lockedY;

        Vector3 toTarget = targetPos - enemyPos;
        float dist = toTarget.magnitude;

        if (dist <= chase.stopDistance || dist < 0.0001f)
            return enemyPos;

        Vector3 dir = toTarget / dist;
        float maxApproach = Mathf.Max(0f, dist - chase.stopDistance);

        float moveAmount = Mathf.Min(chase.moveSpeed * t, maxApproach);

        Vector3 predicted = enemyPos + dir * moveAmount;
        predicted.y = chase.lockedY;
        return predicted;
    }

    GameObject GetOrCreateGhost(FlyingChase chase)
    {
        if (ghostByChase.TryGetValue(chase, out var ghost) && ghost != null)
            return ghost;

        ghost = Instantiate(ghostPrefab);
        ghost.name = $"FutureGhost_{chase.gameObject.name}";
        ghostByChase[chase] = ghost;
        return ghost;
    }

    void DisableAllGhosts()
    {
        foreach (var kv in ghostByChase)
            if (kv.Value) kv.Value.SetActive(false);
    }
}
