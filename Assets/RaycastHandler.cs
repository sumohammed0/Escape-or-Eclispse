using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DoorScript;

public class RaycastHandler : MonoBehaviourPunCallbacks
{
    [Header("Raycast Settings")]
    public float rayLength = 5f;
    public Color rayColor = Color.green;
    public LayerMask interactableLayer;
    public Transform cameraTransform;
    private LineRenderer lineRenderer;
    private Transform lastHighlightedObject, grabbedObject;
    private Color originalObjectColor;
    public bool isGrabbing;
    // private bool isGrabbing;
    private Transform raygunObject;
    public Transform raygunObj;
    public bool isHoldingRaygun = false;
    private PhotonView view;
    // public List<GameObject> inventoryItems = new List<GameObject>(); // keep track of the items in the inventory
    public InventoryManager inventoryManagerScript; // Reference to the inventory manager script
    public GameObject inventoryCanvas; // Reference to the inventory canvas
    public bool puzzle1Solved = true;
    bool puzzle2Solved = false;
    bool puzzle4Solved = false;
    private Transform nearbyEngraving; // Tracks the nearby Engraver object
    private Transform raygunParent; // Store the grabbed object GameObject
    void Start()
    {
        view = GetComponent<PhotonView>();
        SetupLineRenderer();
    }

    void SetupLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = rayColor;
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (!cameraTransform) return;

            if (isHoldingRaygun)
            {
                HandleRaygunState();
            }
            else
            {
                Vector3 rayOrigin = cameraTransform.position - cameraTransform.up * 0.3f;
                Ray ray = new Ray(rayOrigin, cameraTransform.forward);
                lineRenderer.SetPosition(0, rayOrigin);

                if (isGrabbing) HandleGrabbedState();
                else HandleDefaultState(ray, rayOrigin);
            }
            // if (puzzle2Solved) {
            //     Debug.Log("Puzzle 2 solved"); // Debug log to confirm the puzzle is solved
            // }
            if (Input.GetKeyDown(KeyCode.M)) {
                Debug.Log("open inventory: m clicked");
                inventoryCanvas.SetActive(true); // open the inventory
            }
        }
    }

    void HandleRaygunState()
    {
        if (!raygunObject) return;

        raygunObject.position = cameraTransform.position + cameraTransform.right * 0.1f + cameraTransform.forward * 0.3f;
        raygunObject.rotation = cameraTransform.rotation;

        Vector3 rayOrigin = cameraTransform.position - cameraTransform.up * 0.3f;
        Ray ray = new Ray(rayOrigin, cameraTransform.forward);
        lineRenderer.SetPosition(0, rayOrigin);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (Input.GetKeyDown(KeyCode.Y) && hit.collider.CompareTag("Ground"))
            {
                // Debug.Log("Teleporting to: " + hit.point); // Debug log to confirm the teleportation
                this.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
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
        if (grabbedObject != null)
        {
            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * 1f;
            grabbedObject.position = newPosition;
            lineRenderer.SetPosition(1, newPosition);

            PhotonView pv = grabbedObject.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                // Continuously sync position
                pv.RPC("SyncGrabbedPosition", RpcTarget.Others, newPosition, grabbedObject.rotation);
            }

            // Detect Engraving under the Moonstone
            Ray detectBelow = new Ray(grabbedObject.position, Vector3.down);
            RaycastHit engravingHit;
            if (Physics.Raycast(detectBelow, out engravingHit, 1f))
            {
                if (engravingHit.collider.CompareTag("Engraving"))
                {
                    nearbyEngraving = engravingHit.transform;
                }
                else
                {
                    nearbyEngraving = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ReleaseObject();
            }
        }
    }


    void HandleDefaultState(Ray ray, Vector3 rayOrigin)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            HandleLocker1Interactions(hit);
            handlePuzzle2(hit);
            HandleInteractableHit(hit);
            handleLightSwitch(hit);
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + cameraTransform.forward * rayLength);
            RemoveHighlight();
        }
    }
    void handleLightSwitch(RaycastHit hit)
    {
        if (hit.collider.CompareTag("LightSwitch"))
        {
            HighlightObject(hit.collider.transform);
            if (Input.GetKeyDown(KeyCode.B))
            {
                AKLightSwitch lightSwitch = hit.collider.GetComponent<AKLightSwitch>();
                lightSwitch?.interact();
            }
        }
        else
        {
            RemoveHighlight();
        }
    }

    void handlePuzzle2(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Puzzle2"))
        {
            HighlightObject(hit.collider.transform);

            if (hit.collider.name == "glass_holder")
                handleSandClock(hit);
            else if (hit.collider.name == "drawerluck2")
                handlepuzzle2Drawerlock(hit);
        }
        else
            RemoveHighlight();
    }

    private void handlepuzzle2Drawerlock(RaycastHit hit) {
    if (isGrabbing) return;
        AKpuzzle2Start puzzle1StartLocker = hit.collider.GetComponent<AKpuzzle2Start>();
        if (Input.GetKeyDown(KeyCode.B))
            puzzle1StartLocker?.startPuzzle();
    }
    
    private void handleSandClock(RaycastHit hit) {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isGrabbing) return;
            {
                AKPuzzle2SandClockManager sandClockManager = hit.collider.GetComponent<AKPuzzle2SandClockManager>();
                if (sandClockManager.IsSolved && !sandClockManager.ClueManager.isFadeIn)
                    sandClockManager.ClueManager.StartFadeSequence(2);
                else if (sandClockManager != null)
                    sandClockManager.FlipSandClock();
            }
        }
    }
    
    private void HandleLocker1Interactions(RaycastHit hit)
     { 
        if (isGrabbing) return;
        if (hit.collider.CompareTag("Locker1"))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (hit.collider.name == "Puzzle1DrawerLocker")
                {
                    hit.collider.GetComponent<AKPuzzelOneStart>().startPuzzle();
                    return;
                }
                Outline outline = hit.collider.GetComponent<Outline>();
                outline.enabled = true;
                AKDigitButton button = hit.collider.GetComponent<AKDigitButton>();
                if (button != null)
                {
                    button.Interact();
                }
                outline.enabled = false;
            }
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
        // else if (hit.collider.CompareTag("Raygun"))
        // {
        //     HighlightObject(hit.collider.transform);

        //     if (Input.GetKeyDown(KeyCode.B))
        //     {
        //         PickUpRaygun(hit.collider.transform);
        //     }
        // }
        else if (hit.collider.CompareTag("door"))
        {
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Door hit"); // Debug log to confirm the hit
                Door door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    door.OpenDoor();
                }
            }
        }
        else if (hit.collider.CompareTag("Moonstone"))
        {
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B) && !isGrabbing)
            {
                GrabObject(hit.collider.transform);
            }
        }
        else
        {
            RemoveHighlight();
            RemoveOutline();
        }
    }


    private void RemoveOutline()
    {
        if (lastHighlightedObject)
        {
            Outline outline = lastHighlightedObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    public void GrabObject(Transform obj)
    {
        PhotonView objectPhotonView = obj.GetComponent<PhotonView>();
        if (objectPhotonView != null)
        {
            // Handle objects with PhotonView
            objectPhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            grabbedObject = obj;
            isGrabbing = true;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Collider col = obj.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            obj.SetParent(cameraTransform);

            objectPhotonView.RPC("SyncInitialGrab", RpcTarget.OthersBuffered, 
                obj.position,
                obj.rotation,
                view.ViewID);
        }
        else
        {
            // Handle objects without PhotonView
            grabbedObject = obj;
            isGrabbing = true;
        }
    }

    public void ReleaseObject()
    {
        if (grabbedObject == null) return;

        PhotonView pv = grabbedObject.GetComponent<PhotonView>();

        if (pv != null)
        {
            Collider col = grabbedObject.GetComponent<Collider>();
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedObject.CompareTag("Moonstone"))
            {
                AKengravingIdentifier StonIdentifier = grabbedObject.GetComponent<AKengravingIdentifier>();
                if (!StonIdentifier) throw new System.Exception("Plesae Assign the engraving identifier to the moonstone object.");
                Transform closestEngraving = FindNearbyEngraving(grabbedObject.position, 0.5f, StonIdentifier.moon_identifier);
                if (closestEngraving != null)
                {
                    grabbedObject.SetParent(closestEngraving);
                    grabbedObject.position = closestEngraving.position;
                    grabbedObject.rotation = closestEngraving.rotation;
                    MoonstoneScript ms = grabbedObject.GetComponent<MoonstoneScript>();
                    if (ms)
                    {
                        ms.photonView.RPC("SyncPlacement", RpcTarget.AllBuffered,
                            closestEngraving.position,
                            closestEngraving.rotation,
                            closestEngraving.GetComponent<PhotonView>().ViewID
                        );
                    }
                    Door door = FindFirstObjectByType<Door>();
                    if (door != null)
                    {
                        door.photonView.RPC("RPC_NotifyMoonstonePlaced", RpcTarget.AllBuffered);
                    }
                }
            }
            else
            {
                if (rb) rb.isKinematic = false;
                if (col) col.enabled = true;
                grabbedObject.SetParent(null);
            }
        }

        grabbedObject = null;
        isGrabbing = false;
    }

    void HighlightObject(Transform obj)
    {
        if (lastHighlightedObject == obj) return;
        RemoveHighlight();
        lastHighlightedObject = obj;

        // Add or enable the Outline component
        var outline = obj.GetComponent<Outline>();
        if (!outline)
        {
            outline = obj.gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 5f;
        }
        outline.enabled = true;
    }

    void RemoveHighlight()
    {
        if (!lastHighlightedObject) return;

        // Disable the Outline component
        var outline = lastHighlightedObject.GetComponent<Outline>();
        if (outline)
        {
            outline.enabled = false;
        }
        lastHighlightedObject = null;
    }

    public void PickUpRaygun(Transform raygun)
    {
        Debug.Log("Raygun picked up"); // Debug log to confirm the raygun is picked up
        raygunObject = raygun;
        isHoldingRaygun = true;
        raygunParent = raygun.transform.parent.transform; // Store the parent GameObject of the raygun
        raygunObject.SetParent(cameraTransform);
        raygunObject.localPosition = new Vector3(0.5f, -0.2f, 0.3f);
        raygunObject.localRotation = Quaternion.identity;
    }

    public void ReleaseRaygun()
    {
        if (raygunObject)
        {
            raygunObject.SetParent(raygunParent);
            //raygunObject.SetParent(null);
            raygunObject.gameObject.SetActive(false); // should be stored in the inventory so set inactive until selected again
            raygunObject = null;
        }
        isHoldingRaygun = false;
    }

    public void HandleOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // Auto-approve all ownership requests
        targetView.TransferOwnership(requestingPlayer);
    }

    Transform FindNearbyEngraving(Vector3 position, float maxDistance, int stoneIdentifier)
    {
        Collider[] colliders = Physics.OverlapSphere(position, maxDistance);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Engraving"))
            {
                AKengravingIdentifier engravingIdentifier = col.GetComponent<AKengravingIdentifier>();
                if (!engravingIdentifier) return null;
                if ((stoneIdentifier +  engravingIdentifier.moon_identifier == 1))
                {
                    return col.transform;
                }
            }
        }
        return null;
    }
}
