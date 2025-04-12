using UnityEngine;
using System;

public class AKDigitButton : MonoBehaviour
{
    [SerializeField] private AKPuzzleOneLockerManager puzzleManager;
    public int digitValue;
    public void Interact()
    {
        StartCoroutine(AnimateButtonPress());
        if (digitValue>0)
            puzzleManager.AddDigit(digitValue);
        else
            puzzleManager.DeleteLastDigit();
    }
    System.Collections.IEnumerator AnimateButtonPress()
    {
        Vector3 originalPosition = transform.localPosition;
        Vector3 pressedPosition = originalPosition + new Vector3(0, -0.1f, 0); // Adjust the offset as needed
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

        // Move back to original position
        elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            transform.localPosition = Vector3.Lerp(pressedPosition, originalPosition, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
    }
}
