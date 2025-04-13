using UnityEngine;

public class AKLightSwitch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("References")]
    public Light lightToToggle;        // The Light component to turn on/off
    public GameObject switchObject;    // Optional: The switch object (for animation or visuals)

    private bool isOn = true;          // Current state of the light


    // This function can be called to toggle the light
    public void interact()
    {
        if (lightToToggle == null) return;

        isOn = !isOn;
        lightToToggle.enabled = isOn;

        Debug.Log("Light toggled. New state: " + (isOn ? "On" : "Off"));
    }
}

