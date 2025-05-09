using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using DoorScript;
using System.Data.Common;

public class AKRayCaster : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
    private PhotonView view;

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

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("jsA_mine") || Input.GetButtonDown("jsA_partner"))
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
            HandleInteractableHit(hit);
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + cameraTransform.forward * rayLength);
            RemoveHighlight();
        }
    }
    void HandleLocker1Interactions(RaycastHit hit)
    {
        if(isGrabbing) return;
        if (hit.collider.CompareTag("Locker1"))
        {
            Outline outline = hit.collider.GetComponent<Outline>();
            outline.enabled = true;
            AKDigitButton button = hit.collider.GetComponent<AKDigitButton>();
            if (button != null && Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
            {
                    button.Interact();
            }
            outline.enabled = false;
        }
    }
    void HandleInteractableHit(RaycastHit hit)
    {
        lineRenderer.SetPosition(1, hit.point);
        if (hit.collider.CompareTag("Interactable"))
        {
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner") && !isGrabbing)
                GrabObject(hit.collider.transform);
        }
        else if (hit.collider.CompareTag("Drawer"))
        {
            // Debug.Log("Drawer hit"); // Debug log to confirm the hit
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
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

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
            {
                PickUpRaygun(hit.collider.transform);
            }
        }
        else if (hit.collider.CompareTag("door"))
        {
            HighlightObject(hit.collider.transform);

            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
            {
                Debug.Log("Door hit"); // Debug log to confirm the hit
                hit.collider.GetComponent<Door>().OpenDoor();
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
            // Handle objects with PhotonView
            pv.RPC("SyncRelease", RpcTarget.AllBuffered,
                grabbedObject.position,
                grabbedObject.rotation);

            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            grabbedObject.SetParent(null);
        }

        // Handle objects without PhotonView
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
        raygunObject.SetParent(cameraTransform);
        raygunObject.localPosition = new Vector3(0.5f, -0.2f, 0.3f);
        raygunObject.localRotation = Quaternion.identity;
    }

    void ReleaseRaygun()
    {
        if (raygunObject)
        {
            raygunObject.SetParent(null);
            raygunObject = null;
        }
        isHoldingRaygun = false;
    }

    public void HandleOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // Auto-approve all ownership requests
        targetView.TransferOwnership(requestingPlayer);
    }
}


