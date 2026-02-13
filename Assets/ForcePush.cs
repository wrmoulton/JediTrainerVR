using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class ForcePushXR : MonoBehaviour
{
    [Header("References")]
    public Transform handPoint;          // left controller transform
    public Transform head;              // XR Camera (HMD). Drag Main Camera here.
    public PlayerStats playerStats;     // drag your PlayerStats here (or auto-find)

    [Header("Push Settings")]
    public float radius = 5f;
    public float pushDistance = 5f;
    public LayerMask enemyLayer;

    [Header("Force Cost")]
    public float forceCost = 20f;       // how much Force per push

    [Header("Gesture Tuning")]
    public float minForwardSpeed = 0.3f; // m/s (try 0.35–0.6)
    public float cooldown = 0.3f;

    [Header("Debug")]
    public bool debugLogs = true;

    private InputDevice leftController;
    private float lastFireTime;

    void Start()
    {
        if (!handPoint) handPoint = transform;
        if (!head) head = Camera.main ? Camera.main.transform : null;

        if (!playerStats)
            playerStats = FindFirstObjectByType<PlayerStats>(); // Unity 2023+

        InitializeController();

        if (debugLogs) Debug.Log("[FORCE PUSH] Started. Hold X + thrust forward.");
    }

    void InitializeController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);

        if (devices.Count > 0)
        {
            leftController = devices[0];
            if (debugLogs) Debug.Log("[FORCE PUSH] Left controller: " + leftController.name);
        }
        else
        {
            Debug.LogWarning("[FORCE PUSH] No left controller found.");
        }
    }

    void Update()
    {
        if (!leftController.isValid)
        {
            InitializeController();
            return;
        }

        bool xPressed = false;
        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out xPressed);
        if (!xPressed) return;

        if (Time.time - lastFireTime < cooldown)
        {
            if (debugLogs) Debug.Log("[FORCE PUSH] Cooldown active");
            return;
        }

        Vector3 vel;
        if (!leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out vel))
        {
            if (debugLogs) Debug.LogWarning("[FORCE PUSH] No deviceVelocity available");
            return;
        }

        Vector3 forwardDir = head ? head.forward : handPoint.forward;
        forwardDir.y = 0f;
        forwardDir.Normalize();

        float forwardSpeed = Vector3.Dot(vel, forwardDir);

        if (debugLogs)
            Debug.Log($"[FORCE PUSH] X HELD | forwardSpeed={forwardSpeed:F2} | force={(playerStats ? playerStats.force : -1):F1}");

        if (forwardSpeed >= minForwardSpeed)
        {
            // --- FORCE GATE ---
            if (!playerStats)
            {
                Debug.LogWarning("[FORCE PUSH] No PlayerStats assigned/found. Cannot spend Force.");
                return;
            }

            if (!playerStats.TrySpendForce(forceCost))
            {
                if (debugLogs) Debug.Log($"[FORCE PUSH] Not enough Force! Need {forceCost}, have {playerStats.force:F1}");
                lastFireTime = Time.time; // optional: prevent spam while empty
                return;
            }

            if (debugLogs) Debug.Log($"[FORCE PUSH] THRUST DETECTED → FIRING (spent {forceCost} Force)");

            FirePush();

            lastFireTime = Time.time;
        }
    }

    void FirePush()
    {
        Vector3 origin = handPoint.position;
        Collider[] hits = Physics.OverlapSphere(origin, radius, enemyLayer, QueryTriggerInteraction.Collide);

        int pushed = 0;

        foreach (var h in hits)
        {
            Knockbackable kb = h.GetComponentInParent<Knockbackable>();
            if (kb == null) continue;

            Vector3 dir = (kb.transform.position - origin);
            kb.Knockback(dir, pushDistance);
            pushed++;
        }

        if (debugLogs) Debug.Log($"[FORCE PUSH] Fired. Enemies pushed: {pushed}");
    }

    private void OnDrawGizmosSelected()
    {
        Transform t = handPoint ? handPoint : transform;
        Gizmos.DrawWireSphere(t.position, radius);
    }
}
