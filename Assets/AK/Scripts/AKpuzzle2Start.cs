using UnityEngine;
public class AKpuzzle2Start : MonoBehaviour
{
    public GameObject puzzlelocker;// Reference to the locker manager object
    public AKPuzzle2Manager puzzle2Manager;
    [SerializeField] CharacterMovement characterMovement;
    private float ButtonDescriptionfontSize;

    void Start()
    {
        puzzlelocker.SetActive(false);// Disable the locker manager at the start
    }
    private void Update()
    {
        if ((puzzle2Manager.isplaingPuzzle2 && Input.GetKeyDown(KeyCode.L))  
                                | puzzle2Manager.IsSolved)
        {
            puzzle2Manager.ButtonDescriptionss.text = "";
            stopPuzzle();
        }
    }
    public void startPuzzle()
    {
        ButtonDescriptionfontSize = puzzle2Manager.ButtonDescriptionss.fontSize;
        characterMovement.enabled = false;// Disable character movement
        AnimateLocker1Drawer();
        puzzle2Manager.isplaingPuzzle2 = true;// Set the puzzle manager to playing state
        puzzlelocker.SetActive(true);// show the locker
    }
    public void stopPuzzle()
    {
        puzzle2Manager.ButtonDescriptionss.fontSize = ButtonDescriptionfontSize;
        puzzle2Manager.isplaingPuzzle2 = false;// Set the puzzle manager to not playing state
        puzzlelocker.SetActive(false);// Disable the locker manager
        characterMovement.enabled = true;// Disable character movement
    }

    System.Collections.IEnumerator AnimateLocker1Drawer()
    {
        Vector3 originalPosition = transform.localPosition;
        Vector3 pressedPosition = originalPosition + new Vector3(.5f, 0, 0); // Adjust the offset as needed
        float animationDuration = 0.1f; // Duration of the press animation

        // Move to pressed position
        float elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, pressedPosition, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = pressedPosition;

    }
    private void Ondisable()
    {
       puzzlelocker.SetActive(false);// Disable the locker manager when the script is disabled
       characterMovement.enabled = true;// Disable character movement
    }
}


