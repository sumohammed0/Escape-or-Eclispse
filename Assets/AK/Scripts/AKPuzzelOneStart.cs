using UnityEngine;

public class AKPuzzelOneStart : MonoBehaviour
{
    public GameObject puzzleOnelocker;// Reference to the locker manager object
    [SerializeField] AKPuzzleOneLockerManager puzzle1LockManager;

    void Start()
    {
        puzzleOnelocker.SetActive(false);// Disable the locker manager at the start
    }
    public void startPuzzleOne()
    {
        puzzle1LockManager.characterMovement.enabled = false; // Disable the locker manager at the start
        puzzle1LockManager.isPlayingPuzzle1 = true;
        AnimateLocker1Drawer();
        puzzleOnelocker.SetActive(true);// Enable the locker manager
        gameObject.SetActive(false);// Disable the current game object
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
}