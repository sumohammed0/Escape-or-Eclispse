using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventoryItems = new List<GameObject>();
    public GameObject inventoryFirstSelected;
    GameObject[] inventoryButtons;
    public GameObject settingsCanvas;
    int selectedIndex; 
    public RaycastHandler raycastHandlerScript;
    public GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        //player.GetComponent<RaycastHandler>().enabled = false; // Enable the raycast handler script
        player.GetComponent<CharacterMovement>().enabled = false; // Disable the intro screen manager script
        EventSystem.current.SetSelectedGameObject(inventoryFirstSelected); // highlight first selected button
        inventoryButtons = GameObject.FindGameObjectsWithTag("inventoryButton"); // get all buttons for inventory
        
        if (raycastHandlerScript.puzzle1Solved) {
            AddItemToInventory(raycastHandlerScript.raygunObj.gameObject); // add the raygun to the inventory index = 0
        }


        selectedIndex = 0;
        // for every item in the inventory put its image inside of the inventory
        foreach (GameObject item in inventoryItems) {
            inventoryButtons[selectedIndex].transform.GetChild(0).GetComponent<Image>().sprite = item.transform.parent.GetComponent<Image>().sprite;
            selectedIndex++;
        }

        selectedIndex = 0; // reset selected index to 0

        if (raycastHandlerScript.isGrabbing && raycastHandlerScript.isHoldingRaygun) {
            Debug.Log("Raygun is currently being held so highlighting this button");
            selectedIndex = 0;
            EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
        }
    }
    
    void OnDisable() {
        inventoryItems.Clear(); // clear the inventory items when the inventory is closed
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        //if (Input.GetAxis("Vertical") > 0) {
            selectedIndex--;
            if (selectedIndex < 0) {
                selectedIndex = inventoryButtons.Length - 1;
            }
            EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
        //else if (Input.GetAxis("Vertical") < 0) {
            selectedIndex++;
            if (selectedIndex > inventoryButtons.Length - 1) {
                selectedIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
        }


        if (Input.GetKeyDown(KeyCode.B)) {
            Debug.Log("button pressed B");
        //if (Input.GetButtonDown("js1")) {
            //Debug.Log($"Button pressed {EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text}");
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            Debug.Log("close inventory: m clicked");
            CloseInventory();
        }
    }

    public void AddItemToInventory(GameObject item) {
        if (inventoryItems.Count >=5) {
            Debug.Log("Inventory is full");
            return;
        }

        item.SetActive(false);
        raycastHandlerScript.ReleaseObject();
        inventoryItems.Add(item);
        Debug.Log("Item added to inventory: " + item.name);
    }

    public void SelectObject() {
        Debug.Log("Object selected from inventory");
        GameObject selectedObj = inventoryItems[selectedIndex];
        inventoryItems.RemoveAt(selectedIndex);

        if (selectedObj.CompareTag("Raygun")) {
            Debug.Log("Raygun selected from inventory");
            selectedObj.SetActive(true);
            raycastHandlerScript.PickUpRaygun(selectedObj.transform); // pick up the raygun if raygun selected from inventory
        }

        CloseInventory();
    }

    public void CloseInventory() {
        player.GetComponent<RaycastHandler>().enabled = true; // Enable the raycast handler script
        player.GetComponent<CharacterMovement>().enabled = true; // Disable the intro screen manager script
        this.gameObject.SetActive(false);
    }
}
