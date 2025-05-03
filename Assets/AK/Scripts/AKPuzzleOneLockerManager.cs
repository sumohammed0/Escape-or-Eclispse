using UnityEngine;
using TMPro;
public class AKPuzzleOneLockerManager : MonoBehaviour
{
    public int maxDigits = 8; // Set the required sequence length
    public string correctSequence = "21678543"; // Set the correct sequence
    public GameObject lockedObject; // Assign the object to be unlocked
    private string currentInput = ""; // Stores the current input string
    [SerializeField] private  TMP_Text displayText; // Base class for both UI and 3D text
    [SerializeField] private AudioSource pressAudioSource;
    [SerializeField] private GameObject DrawerOneLocker;
    [SerializeField] private GameObject DialPad;
    public CharacterMovement characterMovement;
    public bool isPlayingPuzzle1 = false; // Flag to check if the puzzle is being played

    public void Start()
    {
        displayText.text = ""; // Clear the text at the start
        if (correctSequence.Length != maxDigits)
            throw new System.Exception("Correct sequence length does not match the required sequence length.");

        foreach (char c in correctSequence)
        {
            if (!char.IsDigit(c))
                throw new System.Exception("Correct sequence contains invalid characters. Only digits (0-9) are allowed.");
        }
    }

    public void Update()
    {
        if (!isPlayingPuzzle1)
            return;
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("jsmenu_mine") || Input.GetButtonDown("jsmenu_partner")){
            DrawerOneLocker.SetActive(true);
            DialPad.SetActive(false);
            characterMovement.enabled = true; // Enable character movement when the script is disabled
            isPlayingPuzzle1 = false;
        }

        currentInput = displayText.text; // Update the input sequence from the GUItextmeshpro
        CheckSequence(currentInput);
    }

    public void CheckSequence(string enteredSequence)
    {
        if (enteredSequence == correctSequence)
             StartCoroutine(Unlock());
    }



    public void DeleteLastDigit()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
        else
        {
            Debug.Log("No digits to delete.");
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }


    public void AddDigit(int digit)
    {
        if (currentInput.Length < maxDigits)
        {
            currentInput += digit.ToString();
            UpdateDisplay();
            if (pressAudioSource)
                pressAudioSource.Play();
        }
        else
        {
            Debug.Log("Maximum input length reached.");
        }

    }

    System.Collections.IEnumerator Unlock()
    {
        DrawerOneLocker.SetActive(false);
        Debug.Log("Correct sequence entered. Unlocking...");
        if (lockedObject != null) {
            Debug.Log("changing layer");
            lockedObject.layer = LayerMask.NameToLayer("Interactable");; // Example action
        }
        yield return new WaitForSeconds(1f);
        characterMovement.enabled = true; // Enable character movement when the script is disabled
        gameObject.SetActive(false);
    }
}
