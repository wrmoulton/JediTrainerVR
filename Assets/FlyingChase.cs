using UnityEngine;

public class FlyingChase : MonoBehaviour
{
    public Transform target;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float stopDistance = 2.0f;

    [Header("Height Lock")]
    public float lockedY = 3.2f;           // world-space height (meters)
    public bool lockToInitialY = true;     // if true, uses spawn height automatically

    [Header("Rotation (Yaw Only)")]
    public float turnSpeed = 8f;

    void Start()
    {
        if (lockToInitialY)
            lockedY = transform.position.y; // lock at spawn height
    }

    void Update()
    {
        if (!target) return;

        // --- Position: lock Y ---
        Vector3 myPos = transform.position;
        myPos.y = lockedY;
        transform.position = myPos;

        Vector3 targetPos = target.position;
        targetPos.y = lockedY; // chase on a flat plane

        // --- Move toward target (flat plane) ---
        Vector3 toTarget = targetPos - transform.position;
        float dist = toTarget.magnitude;

        if (dist > stopDistance)
        {
            Vector3 step = toTarget.normalized * moveSpeed * Time.deltaTime;
            transform.position += step;
        }

        // --- Rotate: yaw only (no pitch/roll) ---
        Vector3 lookDir = toTarget;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.0001f)
        {
            Quaternion yaw = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, yaw, turnSpeed * Time.deltaTime);

            // hard-lock x/z rotation just in case something else modifies it
            Vector3 e = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0f, e.y, 0f);
        }
    }
}
