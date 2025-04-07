using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class IntroScreenManager : MonoBehaviour
{
    public CharacterMovement charMoveScript; // Reference to the character controller script
    public GameObject introFirstSelected;
    public GameObject startGameCanvas;
    public GameObject settingsCanvas;

    GameObject[] introButtons;
    int selectedIndex;    
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(introFirstSelected);
        introButtons = GameObject.FindGameObjectsWithTag("introMenuButton");
        player = GameObject.FindGameObjectWithTag("Player");
        selectedIndex = 0;
    }

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(introFirstSelected);
        introButtons = GameObject.FindGameObjectsWithTag("introMenuButton");
        player = GameObject.FindGameObjectWithTag("Player");
        selectedIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
        //if (Input.GetAxis("Vertical") > 0) {
            selectedIndex--;
            if (selectedIndex < 0) {
                selectedIndex = introButtons.Length - 1;
            }
            EventSystem.current.SetSelectedGameObject(introButtons[selectedIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
        //else if (Input.GetAxis("Vertical") < 0) {
            selectedIndex++;
            if (selectedIndex > introButtons.Length - 1) {
                selectedIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(introButtons[selectedIndex]);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            Debug.Log("button pressed B");
        //if (Input.GetButtonDown("js1")) {
            //Debug.Log($"Button pressed {EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text}");
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void StartGame() {
        Debug.Log("Start Game button pressed");
        //charMoveScript.enabled = true; // Enable the character controller script
        startGameCanvas.SetActive(true);
        player.GetComponent<RaycastHandler>().enabled = true; // Enable the raycast handler script
        player.GetComponent<CharacterMovement>().enabled = true; // Disable the intro screen manager script
        this.gameObject.SetActive(false);
    }
    
    public void SettingsMenu() {
        Debug.Log("Settings button pressed");
        settingsCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}