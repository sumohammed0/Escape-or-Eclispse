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
    public SolvedPuzzleManager solvedPuzzleManagerScript;
    public GameObject player;
    public GameObject flashlight;
    public GameObject raygun;
    private float horizontalInputCooldown = 0.2f; // seconds
    private float horizontalTimer = 0f;
    // public SpawnPlayers spawnPlayersScript; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //flashlight = GameObject.FindGameObjectsWithTag("flashlight")[0];

        // solvedPuzzleManagerScript = GameObject.FindGameObjectWithTag("SolveManager").GetComponent<SolvedPuzzleManager>();
        // player.GetComponent<CharacterMovement>().enabled = false; // Disable the intro screen manager script
        // EventSystem.current.SetSelectedGameObject(inventoryFirstSelected); // highlight first selected button
        // inventoryButtons = GameObject.FindGameObjectsWithTag("inventoryButton"); // get all buttons for inventory
        // Debug.Log("Inventory opened");
        
        // if (solvedPuzzleManagerScript.puzzle1Solved) {
        //     AddItemToInventory(raycastHandlerScript.raygunObj.gameObject); // add the raygun to the inventory index = 0
        //     AddItemToInventory(flashlight);
        // }

        // // Debug.Log("Inventory here");


        // selectedIndex = 0;
        // // for every item in the inventory put its image inside of the inventory
        // foreach (GameObject item in inventoryItems) {
        //     if(inventoryButtons[selectedIndex].transform.GetChild(0) == null) {
        //         Debug.Log("No button child found");
        //     }
        //     if (item.transform.parent.GetComponent<Image>() == null) {
        //         Debug.Log("No image found");
        //     }

        //     inventoryButtons[selectedIndex].transform.GetChild(0).GetComponent<Image>().sprite = item.transform.parent.GetComponent<Image>().sprite;

        //     selectedIndex++;
        // }

        // this.selectedIndex = 0; // reset selected index to 0
        //EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
    }

    void OnEnable()
    {
        Debug.Log("Inventory enabled");
        player = this.transform.parent.gameObject;
        player.GetComponent<RaycastHandler>().enabled = false; // Enable the raycast handler script
        solvedPuzzleManagerScript = GameObject.FindGameObjectWithTag("SolveManager").GetComponent<SolvedPuzzleManager>();
        player.GetComponent<CharacterMovement>().enabled = false; // Disable the intro screen manager script
        EventSystem.current.SetSelectedGameObject(inventoryFirstSelected); // highlight first selected button
        inventoryButtons = GameObject.FindGameObjectsWithTag("inventoryButton"); // get all buttons for inventory
        Debug.Log("Inventory opened");
        
        if (solvedPuzzleManagerScript.puzzle1Solved) {
            //AddItemToInventory(raycastHandlerScript.raygunObj.gameObject); // add the raygun to the inventory index = 0
            AddItemToInventory(raygun);
            AddItemToInventory(flashlight);
        }

        Debug.Log("Inventory here");


        selectedIndex = 0;
        // for every item in the inventory put its image inside of the inventory
        foreach (GameObject item in inventoryItems) {
            Debug.Log("Item in inventory: " + item.name);
            if(inventoryButtons[selectedIndex].transform.GetChild(0) == null) {
                Debug.Log("No button child found");
            }
            if (item.transform.parent.GetComponent<Image>() == null) {
                Debug.Log("No image found");
            }
            Debug.Log("Item image: " + item.transform.parent.GetComponent<Image>().sprite);
            inventoryButtons[selectedIndex].transform.GetChild(0).GetComponent<Image>().sprite = item.transform.parent.GetComponent<Image>().sprite;

            selectedIndex++;
        }

        this.selectedIndex = 0; // reset selected index to 0

        // if (raycastHandlerScript.isGrabbing && raycastHandlerScript.isHoldingRaygun) {
        //     Debug.Log("Raygun is currently being held so highlighting this button");
        //     this.selectedIndex = 0;
        //     EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
        // }
        // else if (raycastHandlerScript.isGrabbing && raycastHandlerScript.isHoldingFlashlight) {
        //     Debug.Log("flashlight is currently being held so highlighting this button");
        //     this.selectedIndex = 1;
        //     EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
        // }
    }
    
    void OnDisable() {
        inventoryItems.Clear(); // clear the inventory items when the inventory is closed
        Debug.Log("cleared inventory");
        foreach (GameObject button in inventoryButtons) {
            button.transform.GetChild(0).GetComponent<Image>().sprite = null; // clear the inventory buttons
        }
        Debug.Log("cleared inventory buttons");
        //selectedIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalTimer -= Time.deltaTime;

        float axis = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftArrow) || (axis < -0.5f && horizontalTimer <= 0f)) {
            selectedIndex -= 1;
            if (selectedIndex < 0) {
                selectedIndex = inventoryButtons.Length - 1;
            }
            EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
            horizontalTimer = horizontalInputCooldown;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || (axis > 0.5f && horizontalTimer <= 0f)) {
            selectedIndex += 1;
            if (selectedIndex > inventoryButtons.Length - 1) {
                selectedIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(inventoryButtons[selectedIndex]);
            horizontalTimer = horizontalInputCooldown;
        }

        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner")) {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("jsB_mine") || Input.GetButtonDown("jsB_partner")) {
            CloseInventory();
        }
    }

    public void AddItemToInventory(GameObject item) {
        if (inventoryItems.Count >=5) {
            Debug.Log("Inventory is full");
            return;
        }

        item.SetActive(false);
        if (item.CompareTag("Raygun")) {
            Debug.Log("released raygun to add to inventory");
            raycastHandlerScript.ReleaseRaygun();
        }
        else if (item.CompareTag("flashlight")) {
            Debug.Log("released flashlight to add to inventory");
            raycastHandlerScript.ReleaseFlashlight();
        }
        else {
            raycastHandlerScript.ReleaseObject();
        }
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
        if (selectedObj.CompareTag("flashlight")) {
            Debug.Log("Flashlight selected from inventory");
            selectedObj.SetActive(true);
            raycastHandlerScript.PickUpFlashlight(selectedObj.transform); // pick up the flashlight if flashlight selected from inventory
        }

        CloseInventory();
    }

    public void CloseInventory() {
        player.GetComponent<RaycastHandler>().enabled = true; // Enable the raycast handler script
        player.GetComponent<CharacterMovement>().enabled = true; // Disable the intro screen manager script
        this.gameObject.SetActive(false);
    }
}
