using TMPro;  // Make sure you have this if you're using TextMeshPro
using UnityEngine;

public class AK_CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Drag your TimerText UI object here in the Inspector
    public float totalTime = 12 * 60f; // 12 minutes in seconds

    private float currentTime;

    void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timerText.text = "00:00";
            // Optional: Trigger game over or some event
        }
    }
}
