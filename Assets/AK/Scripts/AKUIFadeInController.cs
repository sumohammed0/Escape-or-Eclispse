using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AKUIFadeInController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image blackOverlay; // assign the black screen image
    [SerializeField] private TextMeshProUGUI messageText; // or use UnityEngine.UI.Text if not TMP
    [SerializeField] Canvas EndGameCanvas; // assign the canvas that contains the black screen and text
    [SerializeField] CharacterMovement characterMovement; // reference to the character movement script

    [Header("Timing")]
    [SerializeField] private float triggerTime = 10f; // wait X seconds before fading
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        // Set initial alpha to 0 (completely transparent)
        SetAlpha(0f);
        EndGameCanvas.enabled = false; // Disable the canvas at the start
        // Start the fade trigger timer
        StartCoroutine(FadeInAfterDelay());
    }

    IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(triggerTime);

        float elapsed = 0f;
        EndGameCanvas.enabled = true; // Disable the canvas at the start
        characterMovement.enabled = false; // Disable character movement
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(1f); // ensure fully visible
    }

    private void SetAlpha(float alpha)
    {
        if (blackOverlay)
        {
            Color c = blackOverlay.color;
            c.a = alpha;
            blackOverlay.color = c;
        }

        if (messageText)
        {
            Color c = messageText.color;
            c.a = alpha;
            messageText.color = c;
        }
    }
}
