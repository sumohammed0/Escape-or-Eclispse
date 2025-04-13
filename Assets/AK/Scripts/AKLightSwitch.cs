using UnityEngine;
using System.Collections;

public class AKLightSwitch : MonoBehaviour
{
    [Header("References")]
    public Light lightToToggle;        // The Light component to turn on/off
    //public GameObject switchObject;    // The visual switch to animate (should be a child with a Transform)

    private bool isOn = true;          // Current state of the light


    // Call this to toggle light and push the switch
    public void interact()
    {
        ToggleLight();
            StartCoroutine(PushSwitch());
    }

    private void ToggleLight()
    {
        isOn = !isOn;
        lightToToggle.enabled = isOn;
        //Debug.Log("Light toggled. New state: " + (isOn ? "On" : "Off"));
    }

    private IEnumerator PushSwitch()
    {
        Vector3 originalPos = transform.localPosition;
        Vector3 pushedPos = originalPos + new Vector3(0f, 0f, -0.1f);

        // Push
        transform.localPosition = pushedPos;

        // Wait for 0.2 seconds
        yield return new WaitForSeconds(0.2f);

        // Pull back
        transform.localPosition = originalPos;
    }
}
