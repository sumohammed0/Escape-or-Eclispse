using UnityEngine;
using System.Collections;
using TMPro;


public class ItemHanlder : MonoBehaviour
{
    public float maxRaylength = 20f; // Max distance to pick up objects
    public LayerMask pickupLayer; // Assign to "Interactable" layer for filtering
    public float itemMenueHeightOffest = 1;
    private RaycastHit hit;

//character controller
    public GameObject characterControlllerContainer;
    private CharacterMovement charController;

    // Menu properties
    public enum MenuState
    {
        None,
        MainMenu,
        Inventory,
        ItemMenu
    }

    public GameObject hoveredObject = null;
    public GameObject grabbedObject = null;
    public float translationSpeed = 2f; // Speed for moving Cube1
    public float rotationSpeed = 50f; // Speed for rotating Cube2
    public Transform grabPoint; // Empty object in front of the camera for grabbing objects\
    public bool isGrabbing = false;

    void Start(){
        charController = characterControlllerContainer.GetComponent<CharacterMovement>();
    }

    void Update()
    {
        HandleRaycast();
    }
    void HandleRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, maxRaylength, pickupLayer);
        getHoverableObject(hit);

        if (hoveredObject)
        {
            if (hoveredObject.name == "Cube1")
            {
                //if (Input.GetKey(KeyCode.R) || Input.GetButton("js1"))
                if (Input.GetButton("js2"))
                    MoveObject(hoveredObject);
            }
            else if  (hoveredObject.name == "Cube2")
            {
                //if (Input.GetKey(KeyCode.R) || Input.GetButton("js1"))
                 if (Input.GetButton("js2"))
                    RotateObject(hoveredObject);
            }
            else if (hoveredObject)
                HandleGrab();
        }
    }
    public void getHoverableObject(RaycastHit hit)
    {
        GameObject tempObject = null;
        if (hit.collider )
        {
            tempObject = hit.collider.gameObject;
            bool isHoverable = tempObject.layer == LayerMask.NameToLayer("Interactable") &&
                                tempObject.CompareTag("Pickup");
            if (isHoverable )
            {
                hoveredObject = tempObject;
                hoveredObject.GetComponent<Outline>().enabled = true;
            }
            else
                disableHoverableObject();
            return;
        }
        disableHoverableObject();
    }

    void MoveObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.transform.position += Vector3.right * translationSpeed * Time.deltaTime; // Move upward
        }
    }
    void RotateObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Rotate around Y-axis
        }
    }

    public void dropObject()
    {
        grabbedObject.transform.SetParent(null);
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false; 
        grabbedObject = null;
        hoveredObject = null; 
    }
    public void HandleGrab()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            isGrabbing = !isGrabbing; // Toggle grabbing state
        }

        if (
            (   
                (grabbedObject == null && hoveredObject != null) 
                                                                || grabbedObject) 
                                                                                     && isGrabbing)
        {

            grabObject();
        }



        else if ( grabbedObject != null && !isGrabbing)
        {
            dropObject();
        }

    }

    private void disableHoverableObject()
    {
        if (hoveredObject)
        {
            hoveredObject.GetComponent<Outline>().enabled = false;
            hoveredObject = null;
        }
    }
    public void grabObject()
    {
        // Grab the object
        grabbedObject = hoveredObject;
        grabbedObject.transform.SetParent(grabPoint);
        grabbedObject.transform.localPosition = Vector3.zero;
        grabbedObject.transform.up = Vector3.up;//localRotation = Quaternion.identity; // Reset rotation
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
    }
}