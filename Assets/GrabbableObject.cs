using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Optional Lightsaber Hitbox")]
    public Rigidbody hitboxRigidbody;   // drag lightsaber hitbox here
    public Collider hitboxCollider;     // optional, but recommended

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Object starts as normal physics object
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
        }

        // Hitbox should ALWAYS be kinematic (for laser reflection)
        if (hitboxRigidbody != null)
        {
            hitboxRigidbody.isKinematic = false;
            hitboxRigidbody.useGravity = false;
        }
    }

    // Called when grabbed
    public void GrabObject()
    {
        if (rb != null)
        {
            rb.isKinematic = true;   // follow hand
            rb.useGravity = false;
        }

        // Ensure lightsaber hitbox stays active and kinematic
        if (hitboxRigidbody != null)
        {
            hitboxRigidbody.isKinematic = true;
            hitboxRigidbody.useGravity = false;
        }

        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
        Debug.Log("Object Grabbed!");
    }

    // Called when released
    public void ReleaseObject()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Hitbox stays kinematic even when released
        if (hitboxRigidbody != null)
        {
            hitboxRigidbody.isKinematic = false;
            hitboxRigidbody.useGravity = false;
        }
        Debug.Log("Object Released!");
    }
}
