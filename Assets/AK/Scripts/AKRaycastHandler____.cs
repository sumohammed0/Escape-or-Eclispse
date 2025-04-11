using UnityEngine;
using UnityEngine.EventSystems;

public class AKRaycastHandler___ : MonoBehaviour
{
    private LineRenderer lineRenderer; // Reference to the LineRenderer component
    [SerializeField]
    private Transform rayOrigin; // The origin point of the raycast
    [SerializeField]
    private float rayDistance = 10f; // Maximum distance of the raycast
    [SerializeField]
    private LayerMask interactiveLayer; // Layer mask to filter interactive objects
    private Transform lastHitObject; // To keep track of the last hit object
    public CharacterMovement playerMovement;
    public RaycastHandler playerRaycaster;



    public void Start()
    {
        //playerRaycaster.setActive(false); // Disable the raycaster object
        //playerMovement.setActive(false); // Disable the player movement
        SetupLineRenderer();
    }

    void SetupLineRenderer()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = 0.02f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = lineRenderer.endColor = Color.yellow;
        }
     }

    public void Update()
    {
        PerformRaycast();
        if (lastHitObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Call the OnPointerClick method on the last hit object
                ExecuteEvents.Execute(lastHitObject.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
        }
    }

     void PerformRaycast()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayDistance, interactiveLayer))
        {
            // Update the LineRenderer positions
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);

            // Check if the hit object has the tag "Locker1"
            if (hit.collider.CompareTag("Locker1"))
            {
                // Enable the outline component
                EnableOutline(hit.collider.gameObject);

                // Invoke the OnPointerClick method
            }

            lastHitObject = hit.transform;
        }
        else
        {
            // Optionally disable the outline on the last hit object
            if (lastHitObject != null)
            {
                DisableOutline(lastHitObject.gameObject);
                lastHitObject = null;
            }
        }
    }

     void EnableOutline(GameObject obj)
    {
        var outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

     void DisableOutline(GameObject obj)
    {
        var outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }


}
