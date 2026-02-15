using UnityEngine;

public class RandomFlyer : MonoBehaviour
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

    [Header("Laser Shooting")]
    public GameObject laserPrefab;   // assign in inspector
    public Transform firePoint;      // where laser spawns
    public float laserSpeed = 20f;

    [Header("Audio")]
    public AudioSource audioSource;   // drag droid AudioSource here
    public AudioClip laserSound;      // drag your mp3 here

    private Vector3 targetPosition;
    private float waitTimer;
    private bool hasShot = false;

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
            // Moving
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            hasShot = false; // reset shooting for next stop
        }
        else
        {
            // Stop + shoot once
            if (!hasShot)
            {
                ShootLaser();
                hasShot = true;
            }

            // Wait before moving again
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

    void ShootLaser()
    {
        if (laserPrefab == null || firePoint == null || player == null) return;

        // Play laser sound
        if (audioSource != null && laserSound != null)
        {
            audioSource.PlayOneShot(laserSound);
            Debug.Log("playing laser sound");
        }

        // Get exact direction to player
        Vector3 direction = (player.position - firePoint.position).normalized;

        // Spawn laser
        GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);

        // Force correct size
        laser.transform.localScale = Vector3.one;

        // Make laser face travel direction
        laser.transform.rotation = Quaternion.LookRotation(direction);

        // Move toward player
        Rigidbody rb = laser.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * laserSpeed;
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
