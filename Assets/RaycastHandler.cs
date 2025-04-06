using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float rayLength = 5f;
    public Color rayColor = Color.green;
    public LayerMask interactableLayer;
    public Transform cameraTransform;

    private LineRenderer lineRenderer;
    private Transform lastHighlightedObject, grabbedObject;
    private Color originalObjectColor;
    private bool isGrabbing;
    private Transform raygunObject;
    private bool isHoldingRaygun = false;

    void Start() => SetupLineRenderer();

    void SetupLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = rayColor;
    }

    void Update()
    {
        if (!cameraTransform) return;

        if (isHoldingRaygun)
        {
            HandleRaygunState(); // Handle raygun-specific logic
        }
        else
        {
            Vector3 rayOrigin = cameraTransform.position - cameraTransform.up * 0.3f;
            Ray ray = new Ray(rayOrigin, cameraTransform.forward);
            lineRenderer.SetPosition(0, rayOrigin);

            if (isGrabbing) HandleGrabbedState();
            else HandleDefaultState(ray, rayOrigin);
        }
    }

    void HandleRaygunState()
    {
        if (!raygunObject) return;

        raygunObject.position = cameraTransform.position + cameraTransform.right * 0.1f + cameraTransform.forward * 0.3f; // Position the raygun to the side
        raygunObject.rotation = cameraTransform.rotation; // Align the raygun with the camera's rotation

        Vector3 rayOrigin = cameraTransform.position - cameraTransform.up * 0.3f; // Keep the raycast centered
        Ray ray = new Ray(rayOrigin, cameraTransform.forward);
        lineRenderer.SetPosition(0, rayOrigin);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (Input.GetKeyDown(KeyCode.Y) && hit.collider.CompareTag("Ground"))
            {
                // Debug.Log("Teleporting to: " + hit.point); // Debug log to confirm the teleportation
                this.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z); // Teleport to the hit point
            }
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + cameraTransform.forward * rayLength);
        }

        if (Input.GetKeyDown(KeyCode.Q)) ReleaseRaygun();
    }

    void HandleGrabbedState()
    {
        if (isHoldingRaygun && raygunObject)
        {
            raygunObject.position = cameraTransform.position + cameraTransform.forward * 0.5f; // Position the raygun in front of the camera
            raygunObject.rotation = cameraTransform.rotation; // Align the raygun with the camera's rotation

            Vector3 rayOrigin = raygunObject.position;
            Ray ray = new Ray(rayOrigin, raygunObject.forward);
            lineRenderer.SetPosition(0, rayOrigin);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
            {
                lineRenderer.SetPosition(1, hit.point);

                if (hit.collider.CompareTag("Ground") && Input.GetKeyDown(KeyCode.Y))
                {
                    transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z); // Teleport to the hit point
                }
            }
            else
            {
                lineRenderer.SetPosition(1, rayOrigin + raygunObject.forward * rayLength);
            }

            if (Input.GetKeyDown(KeyCode.Q)) ReleaseRaygun();
        }
        else if (grabbedObject)
        {
            grabbedObject.position = cameraTransform.position + cameraTransform.forward * 1f; // Hold closer to the user
            lineRenderer.SetPosition(1, grabbedObject.position);

            if (Input.GetKeyDown(KeyCode.Q)) ReleaseObject();
        }
    }

    void HandleDefaultState(Ray ray, Vector3 rayOrigin)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            HandleInteractableHit(hit);
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + cameraTransform.forward * rayLength);
            RemoveHighlight();
        }
    }

    void HandleInteractableHit(RaycastHit hit)
    {
        lineRenderer.SetPosition(1, hit.point);
        if (hit.collider.CompareTag("Interactable"))
        {
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B) && !isGrabbing)
                GrabObject(hit.collider.transform);
        }
        else if (hit.collider.CompareTag("Drawer"))
        {
            // Debug.Log("Drawer hit"); // Debug log to confirm the hit
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B))
            {
                DrawerController drawerController = hit.collider.GetComponent<DrawerController>();
                if (drawerController != null)
                {
                    drawerController.ToggleDrawer();
                }
            }
        }
        else if (hit.collider.CompareTag("Raygun"))
        {
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B))
            {
                PickUpRaygun(hit.collider.transform);
            }
        }
        else
        {
            RemoveHighlight();
        }
    }

    void GrabObject(Transform obj)
    {
        grabbedObject = obj;
        isGrabbing = true;
    }

    void ReleaseObject()
    {
        grabbedObject = null;
        isGrabbing = false;
    }

    void HighlightObject(Transform obj)
    {
        if (lastHighlightedObject == obj) return;
        RemoveHighlight();
        lastHighlightedObject = obj;
        var renderer = obj.GetComponent<Renderer>();
        if (!renderer) return;
        originalObjectColor = renderer.material.color;
        renderer.material.color = Color.yellow;
    }

    void RemoveHighlight()
    {
        if (!lastHighlightedObject) return;
        var renderer = lastHighlightedObject.GetComponent<Renderer>();
        if (renderer) renderer.material.color = originalObjectColor;
        lastHighlightedObject = null;
    }

    void PickUpRaygun(Transform raygun)
    {
        raygunObject = raygun;
        isHoldingRaygun = true;
        raygunObject.SetParent(cameraTransform); // Parent the raygun to the camera
        raygunObject.localPosition = new Vector3(0.5f, -0.2f, 0.3f); // Adjust position to the side relative to the camera
        raygunObject.localRotation = Quaternion.identity; // Reset rotation
    }

    void ReleaseRaygun()
    {
        if (raygunObject)
        {
            raygunObject.SetParent(null); // Detach the raygun from the camera
            raygunObject = null;
        }
        isHoldingRaygun = false;
    }
}
