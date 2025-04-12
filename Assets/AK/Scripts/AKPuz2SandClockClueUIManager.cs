using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AKPuz2SandClockClueUIManager : MonoBehaviour
{
    public GameObject canvas;
    public Image[] images;
    public float fadeDuration = 1f;
    //public float waitDuration = 4f;
    public bool isFadeIn = false;

    public void StartFadeSequence(float waitDuration)
    {
        StartCoroutine(FadeSequence(waitDuration));
    }

    private IEnumerator FadeSequence(float waitDuration)
    {
        isFadeIn = true;
        // Enable canvas
        canvas.SetActive(true);

        // Fade in
        yield return StartCoroutine(FadeImages(0f, 1f, fadeDuration));

        // Wait
        yield return new WaitForSeconds(waitDuration);

        // Fade out
        yield return StartCoroutine(FadeImages(1f, 0f, fadeDuration));

        // Optionally disable canvas
        canvas.SetActive(false);
        isFadeIn = false;
    }


    private IEnumerator FadeImages(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            foreach (var image in images)
            {
                var color = image.color;
                color.a = alpha;
                image.color = color;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha is set
        foreach (var image in images)
        {
            var color = image.color;
            color.a = endAlpha;
            image.color = color;
        }
    }
}