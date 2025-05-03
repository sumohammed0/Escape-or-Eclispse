using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using DoorScript; 
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RaycastHandler : MonoBehaviourPunCallbacks
{
    [Header("Raycast Settings")]
    [SerializeField] TextMeshProUGUI ButtonDescriptionss;
    public float rayLength = 5f;
    public Color rayColor = Color.green;
    public LayerMask interactableLayer;
    public Transform cameraTransform;
    private LineRenderer lineRenderer;
    private Transform lastHighlightedObject, grabbedObject;
    public bool isGrabbing;
    private Transform raygunObject;
    private Transform flashlightObject;

    public Transform raygunObj;
    public bool isHoldingRaygun = false;
    public bool isHoldingFlashlight = false;
    private PhotonView view;
    // public List<GameObject> inventoryItems = new List<GameObject>(); // keep track of the items in the inventory
    public InventoryManager inventoryManagerScript; // Reference to the inventory manager script
    public GameObject inventoryCanvas; // Reference to the inventory canvas
    private Transform nearbyEngraving; // Tracks the nearby Engraver object
    private Transform raygunParent; // Store the grabbed object GameObject
    private Transform flashlightParent; // Store the grabbed flashlight GameObject]
    private string sancdclockButtonDescriptions = "B(Keyboard), X(Joystick): Flip SandClock";
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
                HandleFlashlightState();
                Vector3 rayOrigin = cameraTransform.position - cameraTransform.up * 0.3f;
                Ray ray = new Ray(rayOrigin, cameraTransform.forward);
                lineRenderer.SetPosition(0, rayOrigin);

                if (isGrabbing) HandleGrabbedState();
                else HandleDefaultState(ray, rayOrigin);
            }
            if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("jsB_mine") || Input.GetButtonDown("jsB_partner")) {
                inventoryCanvas.SetActive(true);
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

            if (Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("jsY_mine") || Input.GetButtonDown("jsY_partner") && hit.collider.CompareTag("Ground"))
            {
                // Debug.Log("Teleporting to: " + hit.point); // Debug log to confirm the teleportation
                this.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + cameraTransform.forward * rayLength);
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("jsA_mine") || Input.GetButtonDown("jsA_partner")) ReleaseRaygun();
    }

    void HandleFlashlightState()
    {
        if (!flashlightObject) return;
        if (!isHoldingFlashlight) return;
        flashlightObject.position = cameraTransform.position + cameraTransform.right * 0.1f + cameraTransform.forward * 0.3f;
        flashlightObject.rotation = cameraTransform.rotation;

        Vector3 rayOrigin = cameraTransform.position - cameraTransform.up * 0.3f;
        Ray ray = new Ray(rayOrigin, cameraTransform.forward);
        lineRenderer.SetPosition(0, rayOrigin);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + cameraTransform.forward * rayLength);
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("jsA_mine") || Input.GetButtonDown("jsA_partner")) ReleaseFlashlight();
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

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("jsA_mine") || Input.GetButtonDown("jsA_partner")) 
            {
                ReleaseObject();
            }
        }
    }

    void HandleDefaultState(Ray ray, Vector3 rayOrigin)
    {
        // Always update the raycast line first
        lineRenderer.SetPosition(0, rayOrigin);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            // Update raycast end point to hit point
            lineRenderer.SetPosition(1, hit.point);
            
            // Handle interactions in priority order
            if (hit.collider.CompareTag("LightSwitch"))
            {
                handleLightSiwtch(hit);
            }
            else if (hit.collider.CompareTag("Puzzle2"))
            {
                handlePuzzle2(hit);
            }
            else if (hit.collider.CompareTag("Locker1") || 
                    hit.collider.gameObject.layer == LayerMask.NameToLayer("puzzle1"))
            {
                HandleLocker1Interactions(hit);
            }
            else
            {
                HandleInteractableHit(hit);
            }
        }
        else
        {
            // No hit - extend ray to full length
            lineRenderer.SetPosition(1, rayOrigin + cameraTransform.forward * rayLength);
            ButtonDescriptionss.text = "";
            RemoveHighlight();
        }
    }

    void handleLightSiwtch( RaycastHit hit)
    {
        if (isGrabbing) return;
        if (hit.collider.CompareTag("LightSwitch"))
        {
            HighlightObject(hit.collider.transform, Color.yellow);
            ButtonDescriptionss.text = " \t B(Keyboard), X(Joystick): Push the switch";
            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
            {
                AKLightSwitch lightSwitch = hit.collider.GetComponent<AKLightSwitch>();
                if (lightSwitch != null)
                {
                    lightSwitch.interact();
                }
            }
        }
        else
        {
            RemoveHighlight();
        }
    }

    void handlePuzzle2(RaycastHit hit)
    {
        HighlightObject(hit.collider.transform, Color.yellow);
        if (hit.collider.name == "glass_holder")
        {
            ButtonDescriptionss.text = sancdclockButtonDescriptions;

            AKPuzzle2SandClockManager sandClockManager = hit.collider.GetComponent<AKPuzzle2SandClockManager>();
            if (isHoldingFlashlight)
                sandClockManager.ClueManager.IsHoldingFlashLigth = true;
            else
                sandClockManager.ClueManager.IsHoldingFlashLigth = false;
            if (sandClockManager.IsSolved && !sandClockManager.ClueManager.isFadeIn)
            {
                sancdclockButtonDescriptions = " \t B(Keyboard), X(Joystick): show Combination";
                if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
                    sandClockManager.ClueManager.StartFadeSequence(2);
            }
            else if (sandClockManager != null)
            {
                if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
                    sandClockManager.FlipSandClock();
            }
        }
        else if (hit.collider.name == "drawerluck2")
        {
            ButtonDescriptionss.text = " \t B(Keyboard), X(Joystick): Open lock";
            handlepuzzle2Drawerlock(hit);
        }
        else
            ButtonDescriptionss.text = "";
    }

    private void handlepuzzle2Drawerlock(RaycastHit hit) {
    if (isGrabbing) return;
    AKpuzzle2Start puzzle1StartLocker = hit.collider.GetComponent<AKpuzzle2Start>();
    if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
        puzzle1StartLocker?.startPuzzle();
    }

    private void HandleLocker1Interactions(RaycastHit hit)
    {
        if (isGrabbing) return;

        HighlightObject(hit.collider.transform, Color.blue);
        ButtonDescriptionss.text = " \t B(Keyboard), X(Joystick): Interact";

        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
        {
            if (hit.collider.name == "Puzzle1DrawerLocker")
            {
                hit.collider.GetComponent<AKPuzzelOneStart>()?.startPuzzleOne();
            }
            else
            {
                hit.collider.GetComponent<AKDigitButton>()?.Interact();
            }
        }
    }
    

    void HandleInteractableHit(RaycastHit hit)
    {
        lineRenderer.SetPosition(1, hit.point);
        if (hit.collider.CompareTag("Interactable"))
        {
            HighlightObject(hit.collider.transform, Color.white);

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner") && !isGrabbing)
                GrabObject(hit.collider.transform);
        }
        else if (hit.collider.CompareTag("Drawer"))
        {
            // Debug.Log("Drawer hit"); // Debug log to confirm the hit
            HighlightObject(hit.collider.transform, Color.white);

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
            {
                DrawerController drawerController = hit.collider.GetComponent<DrawerController>();
                if (drawerController != null)
                {
                    drawerController.ToggleDrawer();
                }
            }
        }
        else if (hit.collider.CompareTag("door"))
        {
            HighlightObject(hit.collider.transform, Color.white);

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
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
            HighlightObject(hit.collider.transform, Color.white);

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner") && !isGrabbing)
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
            Debug.Log("grabbed obj");

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
            Debug.Log("grabbed obj");
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
                else 
                {
                    if (rb) rb.isKinematic = false;
                    if (col) col.enabled = true;
                    grabbedObject.SetParent(null);
                }
            }
            else
            {
                if (rb) rb.isKinematic = false;
                if (col) col.enabled = true;
                grabbedObject.SetParent(null);
            }
        }
        Debug.Log("Released object: " + grabbedObject.name); // Debug log to confirm the release
        grabbedObject = null;
        isGrabbing = false;
    }

    void HighlightObject(Transform obj, Color outlineColor)
    {
        // Skip if we're already highlighting this object
        if (lastHighlightedObject == obj) return;
        
        // Clear previous highlight
        RemoveHighlight();
        
        // Store new highlight
        lastHighlightedObject = obj;

        // Get or add outline component
        var outline = obj.GetComponent<Outline>();
        if (!outline)
        {
            outline = obj.gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 5f;
        }

        // Apply outline settings
        outline.OutlineColor = outlineColor;
        outline.enabled = true;
    }

    void RemoveHighlight()
    {
        if (!lastHighlightedObject) return;

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

    public void PickUpFlashlight(Transform flashlight)
    {
        Debug.Log("flashlight picked up"); // Debug log to confirm the raygun is picked up
        flashlightObject = flashlight;
        isHoldingFlashlight = true;
        flashlightParent = flashlight.transform.parent.transform; // Store the parent GameObject of the raygun
        flashlightObject.SetParent(cameraTransform);
        flashlightObject.localPosition = new Vector3(0.5f, -0.2f, 0.3f);
        flashlightObject.localRotation = Quaternion.identity;
    }

    public void ReleaseRaygun()
    {
        if (raygunObject)
        {
            raygunObject.SetParent(raygunParent);
            raygunObject.gameObject.SetActive(false); // should be stored in the inventory so set inactive until selected again
            raygunObject = null;
        }
        isHoldingRaygun = false;
    }

    public void ReleaseFlashlight()
    {
        if (flashlightObject)
        {
            flashlightObject.SetParent(flashlightParent);
            //raygunObject.SetParent(null);
            flashlightObject.gameObject.SetActive(false); // should be stored in the inventory so set inactive until selected again
            flashlightObject = null;
        }
        isHoldingFlashlight = false;
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
                if (stoneIdentifier +  engravingIdentifier.moon_identifier == 1)
                {
                    return col.transform;
                }
            }
        }
        return null;
    }
}
