using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrabbed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ensure the object starts in a fixed position
        if (rb != null)
        {
            rb.isKinematic = true; // Makes object static initially
        }
    }

    // This function should be called by your interaction system's "Grab" event
    public void OnGrab()
    {
        if (!isGrabbed)
        {
            isGrabbed = true;
            if (rb != null)
            {
                rb.isKinematic = false; // Allow physics (gravity, etc.) when grabbed
                // Optional: You might want to disable gravity if the grabbing mechanism handles the position
                // rb.useGravity = false; 
            }
            // Add your specific grabbing logic here (e.g., parenting to a hand transform, using joints)
        }
    }

    // This function should be called by your interaction system's "Release" event
    public void OnRelease()
    {
        if (isGrabbed)
        {
            isGrabbed = false;
            // When released, the object will continue with normal physics (e.g., fall with gravity)
            if (rb != null)
            {
                rb.useGravity = true;
            }
            // Add your specific release logic here
        }
    }
}
