using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
public class SettingMenuManager : MonoBehaviour
{
    public GameObject settingFirstSelected;
    public GameObject introCanvas;
    GameObject[] settingButtons;
    int selectedIndex;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(settingFirstSelected);
        settingButtons = GameObject.FindGameObjectsWithTag("settingButton");
        selectedIndex = 0;
    }

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(settingFirstSelected);
        settingButtons = GameObject.FindGameObjectsWithTag("settingButton");
        selectedIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
        //if (Input.GetAxis("Vertical") > 0) {
            selectedIndex--;
            if (selectedIndex < 0) {
                selectedIndex = settingButtons.Length - 1;
            }
            EventSystem.current.SetSelectedGameObject(settingButtons[selectedIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
        //else if (Input.GetAxis("Vertical") < 0) {
            selectedIndex++;
            if (selectedIndex > settingButtons.Length - 1) {
                selectedIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(settingButtons[selectedIndex]);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            Debug.Log("button pressed B");
        //if (Input.GetButtonDown("js1")) {
            //Debug.Log($"Button pressed {EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text}");
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void GoBack() {
        Debug.Log("Back button pressed");
        introCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OpenInventory() {
        Debug.Log("Inventory button pressed");
        // Open the inventory UI here
    }
}
