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
        EventSystem.current.SetSelectedGameObject(inventoryFirstSelected);
        inventoryButtons = GameObject.FindGameObjectsWithTag("inventoryButton");
        selectedIndex = 0;
        // foreach (GameObject item in inventoryItems) {
        //     inventoryButtons[selectedIndex].transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<Actions>().itemSprite;
        // }
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

    public void ChooseObject() {
        Debug.Log("Object chosen");
        raycastHandlerScript.GrabObject(inventoryItems[selectedIndex].transform);
        inventoryItems.RemoveAt(selectedIndex);
        CloseInventory();
    }
    public void CloseInventory() {
        player.GetComponent<RaycastHandler>().enabled = true; // Enable the raycast handler script
        player.GetComponent<CharacterMovement>().enabled = true; // Disable the intro screen manager script
        this.gameObject.SetActive(false);
    }
}
