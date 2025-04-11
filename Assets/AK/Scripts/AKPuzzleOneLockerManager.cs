using UnityEngine;
using TMPro;
public class AKPuzzleOneLockerManager : MonoBehaviour
{
    public int maxDigits = 4; // Set the required sequence length
    public string correctSequence = "1234"; // Set the correct sequence
    public GameObject lockedObject; // Assign the object to be unlocked
    private string currentInput = ""; // Stores the current input string
    [SerializeField] private  TMP_Text displayText; // Base class for both UI and 3D text
    [SerializeField] private AudioSource pressAudioSource;

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
        currentInput = displayText.text; // Update the input sequence from the GUItextmeshpro
        CheckSequence(currentInput);
    }

    public void CheckSequence(string enteredSequence)
    {
        if (enteredSequence == correctSequence)
        {
            Unlock();
        }
        else
        {
            Debug.Log("Incorrect sequence. Try again.");
        }
    }

    private void Unlock()
    {
        Debug.Log("Correct sequence entered. Unlocking...");
        if (lockedObject != null)
        {
            lockedObject.SetActive(false); // Example action
        }
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

            // Animate the parent of the parent (button-cube) of the button
            Transform buttonCube = transform.parent?.parent;
            if (buttonCube != null)
            {
                StartCoroutine(AnimateButtonPress(buttonCube));
            }

            // Play press audio if available
            if (pressAudioSource != null)
            {
                pressAudioSource.Play();
            }
        }
        else
        {
            Debug.Log("Maximum input length reached.");
        }


     System.Collections.IEnumerator AnimateButtonPress(Transform buttonCube)
    {
        Vector3 originalPosition = buttonCube.localPosition;
        Vector3 pressedPosition = originalPosition + new Vector3(0, -0.1f, 0); // Adjust the offset as needed
        float animationDuration = 0.1f; // Duration of the press animation

        // Move to pressed position
        float elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            buttonCube.localPosition = Vector3.Lerp(originalPosition, pressedPosition, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        buttonCube.localPosition = pressedPosition;

        // Move back to original position
        elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            buttonCube.localPosition = Vector3.Lerp(pressedPosition, originalPosition, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        buttonCube.localPosition = originalPosition;
    }
}
}
