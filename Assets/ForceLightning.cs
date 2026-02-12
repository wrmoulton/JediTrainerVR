using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class ForceLightning : MonoBehaviour
{
    [Header("Lightning Settings")]
    public Transform handPoint;      // right hand or palm
    public float range = 20f;
    public float forcePower = 15f;
    public LayerMask enemyLayer;

    [Header("Visual")]
    public LineRenderer lightningLine;

    private InputDevice rightController;

    void Start()
    {
        InitializeController();
    }

    void InitializeController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
            rightController = devices[0];
    }

    void Update()
    {
        // Reconnect controller if needed
        if (!rightController.isValid)
        {
            InitializeController();
        }

        bool aButtonPressed;

        // PrimaryButton = A button on Quest right controller
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed) && aButtonPressed)
        {
            Debug.Log("A button pressed");
            FireLightning();
        }
        else
        {
            lightningLine.enabled = false;
        }
    }

    void FireLightning()
    {
        RaycastHit hit;

        Vector3 origin = handPoint.position;
        Vector3 direction = handPoint.forward;

        lightningLine.enabled = true;
        lightningLine.SetPosition(0, origin);

        if (Physics.Raycast(origin, direction, out hit, range))
        {
            lightningLine.SetPosition(1, hit.point);

            // Check if hit object is enemy
            if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    Vector3 knockDirection =
                        (hit.collider.transform.position - transform.position).normalized;

                    rb.AddForce(knockDirection * forcePower, ForceMode.Impulse);
                }
            }
        }
        else
        {
            lightningLine.SetPosition(1, origin + direction * range);
        }
    }
}
