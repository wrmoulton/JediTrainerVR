using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component when the scene starts
        rb = GetComponent<Rigidbody>();
        // Ensure it starts kinematic (static)
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    // This method is called by your main interaction/grab script when the player grabs the object
    public void GrabObject()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Disable kinematic mode, allowing physics to take over
            // Optionally re-enable gravity if you want it to fall when released
            rb.useGravity = true; 
        }
    }

    // This method is called by your main interaction/grab script when the player releases the object
    public void ReleaseObject()
    {
        // You might want to leave it as a non-kinematic object so it reacts to physics after being released.
        // If you need it to snap back to being static/kinematic immediately upon release,
        // you would add:
        // rb.isKinematic = true;
    }
}
