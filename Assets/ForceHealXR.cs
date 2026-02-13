using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class ForceHealXR : MonoBehaviour
{
    [Header("References")]
    public Transform handPoint;        // left controller transform
    public Transform head;             // XR Camera (Main Camera)
    public PlayerStats playerStats;    // your PlayerStats component

    [Header("Heal Settings")]
    public float healPerSecond = 10f;  // healing rate while shaking
    public float maxHealPerSecond = 35f;
    public bool requireForce = true;
    public float forceCostPerSecond = 20f;

    [Header("Gesture Tuning")]
    [Tooltip("Sideways speed threshold to count as a shake (m/s). Start ~0.6, lower if hard to trigger.")]
    public float minSideSpeed = 0.6f;

    [Tooltip("How often we can apply a heal tick (seconds).")]
    public float tickInterval = 0.12f;

    [Tooltip("How many shake hits are needed within the window to start healing.")]
    public int shakesToActivate = 2;

    [Tooltip("Time window (seconds) for counting shakes.")]
    public float shakeWindow = 0.35f;

    [Header("Debug")]
    public bool debugLogs = true;

    private InputDevice leftController;

    private float lastTickTime = -999f;
    private float lastShakeTime = -999f;
    private int shakeCount = 0;
    private float lastSideSign = 0f;

    void Start()
    {
        if (!handPoint) handPoint = transform;
        if (!head) head = Camera.main ? Camera.main.transform : null;
        if (!playerStats) playerStats = FindFirstObjectByType<PlayerStats>();

        InitializeController();

        if (debugLogs) Debug.Log("[FORCE HEAL] Started. Hold Y + shake side-to-side.");
    }

    void InitializeController()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);

        if (devices.Count > 0)
        {
            leftController = devices[0];
            if (debugLogs) Debug.Log("[FORCE HEAL] Left controller: " + leftController.name);
        }
        else
        {
            Debug.LogWarning("[FORCE HEAL] No left controller found.");
        }
    }

    void Update()
    {
        if (!leftController.isValid)
        {
            InitializeController();
            return;
        }

        if (!playerStats)
        {
            Debug.LogWarning("[FORCE HEAL] No PlayerStats assigned/found.");
            return;
        }

        // Y on Quest Left controller = secondaryButton
        bool yHeld = false;
        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out yHeld);

        if (!yHeld)
        {
            // reset shake tracking when not holding Y
            shakeCount = 0;
            lastSideSign = 0f;
            return;
        }

        // Get tracked controller velocity
        Vector3 vel;
        if (!leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out vel))
        {
            if (debugLogs) Debug.LogWarning("[FORCE HEAL] No deviceVelocity available");
            return;
        }

        // Define "sideways" relative to head/camera (most intuitive)
        Vector3 sideDir = head ? head.right : handPoint.right;
        sideDir.y = 0f;
        sideDir.Normalize();

        float sideSpeed = Vector3.Dot(vel, sideDir); // signed speed (+right, -left)
        float absSideSpeed = Mathf.Abs(sideSpeed);

        if (debugLogs)
            Debug.Log($"[FORCE HEAL] Y HELD | vel={vel} | sideSpeed={sideSpeed:F2}");

        // Detect a "shake" as a sign flip across the threshold
        float sign = Mathf.Sign(sideSpeed);

        bool aboveThreshold = absSideSpeed >= minSideSpeed;
        bool signChanged = (lastSideSign != 0f && sign != 0f && sign != lastSideSign);

        if (aboveThreshold)
        {
            // first strong movement sets initial sign
            if (lastSideSign == 0f) lastSideSign = sign;

            if (signChanged)
            {
                float now = Time.time;

                // if too much time passed, restart the shake counter
                if (now - lastShakeTime > shakeWindow)
                    shakeCount = 0;

                shakeCount++;
                lastShakeTime = now;
                lastSideSign = sign;

                if (debugLogs)
                    Debug.Log($"[FORCE HEAL] SHAKE detected! count={shakeCount}/{shakesToActivate}");
            }
        }

        // Activate healing if we have enough shakes recently
        bool healingActive = (Time.time - lastShakeTime) <= shakeWindow && shakeCount >= shakesToActivate;

        if (!healingActive) return;

        // Tick healing at an interval (prevents insane spam)
        if (Time.time - lastTickTime < tickInterval) return;

        float dt = tickInterval;

        // Optional: scale heal based on how hard you're shaking
        float intensity01 = Mathf.Clamp01((absSideSpeed - minSideSpeed) / Mathf.Max(0.01f, minSideSpeed));
        float healRate = Mathf.Lerp(healPerSecond, maxHealPerSecond, intensity01);

        // Spend Force (per second) to heal
        if (requireForce)
        {
            float cost = forceCostPerSecond * dt;
            if (!playerStats.TrySpendForce(cost))
            {
                if (debugLogs) Debug.Log("[FORCE HEAL] Not enough Force to heal.");
                lastTickTime = Time.time;
                return;
            }
        }

        // Apply heal
        playerStats.health = Mathf.Min(playerStats.maxHealth, playerStats.health + healRate * dt);

        if (debugLogs)
            Debug.Log($"[FORCE HEAL] HEAL tick +{healRate * dt:F1} | health={playerStats.health:F1}/{playerStats.maxHealth}");

        lastTickTime = Time.time;
    }
}
