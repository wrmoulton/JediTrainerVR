using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public int maxReflections = 1;
    public float reflectCooldown = 0.1f; // prevents instant re-hit

    private Rigidbody rb;
    private int reflectionCount = 0;
    private bool canReflect = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit layer: " + LayerMask.LayerToName(collision.gameObject.layer));

        // Ignore collisions briefly after reflecting
        if (!canReflect) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("LightSaberHitBox"))
        {
            Debug.Log("Hit the lightsaber hitbox");

            if (reflectionCount < maxReflections)
            {
                Reflect(collision);
                reflectionCount++;

                // prevent instant second hit
                StartCoroutine(ReflectionCooldown());
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Hit something else :(");
            Destroy(gameObject);
        }
    }

    void Reflect(Collision collision)
    {
        if (rb == null) return;

        Vector3 normal = collision.contacts[0].normal;

        Vector3 reflectedDirection =
            Vector3.Reflect(rb.velocity.normalized, normal);

        rb.velocity = reflectedDirection * rb.velocity.magnitude;

        transform.rotation = Quaternion.LookRotation(reflectedDirection);
    }

    System.Collections.IEnumerator ReflectionCooldown()
    {
        canReflect = false;
        yield return new WaitForSeconds(reflectCooldown);
        canReflect = true;
    }
}
