using UnityEngine;

public class JediTrainingDroid : MonoBehaviour
{
    [Header("Movement Area (Center + Size)")]
    public Vector3 areaCenter;
    public Vector3 areaSize = new Vector3(10f, 5f, 10f);

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float stoppingDistance = 0.5f;
    public float timeBetweenTargets = 2f;

    [Header("Player Target")]
    public Transform player;
    public float rotationSpeed = 5f;

    private Vector3 targetPosition;
    private float waitTimer;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        MoveToTarget();
        LookAtPlayer();
    }

    void MoveToTarget()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > stoppingDistance)
        {
            // Move toward target position
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            // Wait before picking new position
            waitTimer += Time.deltaTime;

            if (waitTimer >= timeBetweenTargets)
            {
                PickNewTarget();
                waitTimer = 0f;
            }
        }
    }

    void PickNewTarget()
    {
        targetPosition = new Vector3(
            Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
            Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2),
            Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2)
        );
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // Shows the flying area in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
