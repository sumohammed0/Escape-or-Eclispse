using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // Import the Unity Scene Management namespace

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
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) {
        //if (Input.GetAxis("Vertical") > 0) {
            selectedIndex--;
            if (selectedIndex < 0) {
                selectedIndex = introButtons.Length - 1;
            }
            EventSystem.current.SetSelectedGameObject(introButtons[selectedIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) {
        //else if (Input.GetAxis("Vertical") < 0) {
            selectedIndex++;
            if (selectedIndex > introButtons.Length - 1) {
                selectedIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(introButtons[selectedIndex]);
        }

        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner")) {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void StartGame() {
        Debug.Log("Start Game button pressed");
        SceneManager.LoadScene("LoadingScene"); // Load the Loading Scene
        //charMoveScript.enabled = true; // Enable the character controller script
        // startGameCanvas.SetActive(true);
        // player.GetComponent<RaycastHandler>().enabled = true; // Enable the raycast handler script
        // player.GetComponent<CharacterMovement>().enabled = true; // Disable the intro screen manager script
        this.gameObject.SetActive(false);
    }
    
    public void SettingsMenu() {
        Debug.Log("Settings button pressed");
        settingsCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}