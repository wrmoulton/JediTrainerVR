using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public int maxReflections = 1; // reflect once, then despawn

    private Rigidbody rb;
    private int reflectionCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime); // auto cleanup
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        // If we hit the lightsaber layer -> reflect
        if (collision.gameObject.layer == LayerMask.NameToLayer("LightSaberHitBox"))
        {
            Debug.Log("Hit the lightsaber hitbox");
            if (reflectionCount < maxReflections)
            {
                Reflect(collision);
                reflectionCount++;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Hit something else :(");
            // Hit anything else -> destroy
            Destroy(gameObject);
        }
    }

    void Reflect(Collision collision)
    {
        // Get surface normal of impact
        Vector3 normal = collision.contacts[0].normal;

        // Reflect velocity direction
        Vector3 reflectedDirection = Vector3.Reflect(rb.velocity.normalized, normal);

        // Apply reflected velocity
        rb.velocity = reflectedDirection * rb.velocity.magnitude;

        // Rotate laser to match new direction
        transform.rotation = Quaternion.LookRotation(reflectedDirection);
    }
}
