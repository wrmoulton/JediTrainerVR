using UnityEngine;

public class ForceGrab : MonoBehaviour
{
    public float grabDistance = 15f;
    public float grabAngleThreshold = 0.8f; // how directly palm faces object
    public float pullSpeed = 20f;
    public Transform holdPoint; // where object floats near hand

    private Rigidbody grabbedObject;

    void Update()
    {
        if (grabbedObject == null)
            TryGrab();
        else
            HoldObject();

        // Debug.DrawRay(transform.position, transform.forward * 5f, Color.green, 0f, false);
    }

    void TryGrab()
    {
        RaycastHit hit;

        // Ray from palm
        if (Physics.Raycast(transform.position, transform.forward, out hit, grabDistance))
        {
            if (hit.collider.CompareTag("ForceGrabbable"))
            {
                Vector3 directionToObject =
                    (hit.collider.transform.position - transform.position).normalized;

                float dot = Vector3.Dot(transform.forward, directionToObject);

                // palm facing object enough
                if (dot > grabAngleThreshold)
                {
                    grabbedObject = hit.collider.GetComponent<Rigidbody>();

                    if (grabbedObject != null)
                    {
                        grabbedObject.useGravity = false;
                        grabbedObject.drag = 5f;
                    }
                }
            }
        }
    }

    void HoldObject()
    {
        if (grabbedObject == null) return;

        // move object toward hand
        Vector3 targetPos = holdPoint.position;

        grabbedObject.velocity =
            (targetPos - grabbedObject.position) * pullSpeed;

        // release if palm no longer facing object
        Vector3 directionToObject =
            (grabbedObject.position - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, directionToObject);

        if (dot < 0.3f)
        {
            ReleaseObject();
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject == null) return;

        grabbedObject.useGravity = true;
        grabbedObject.drag = 0;
        grabbedObject = null;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.up * 2f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.right * 2f);
    }
}

