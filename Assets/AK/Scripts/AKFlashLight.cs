using UnityEngine;

public class AKFlashLight : MonoBehaviour
{ 
    [SerializeField] private Light flashlightSpot; // Assign the spotlight here
    public bool isOn = true;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    [Header("Light Settings")]
    [SerializeField] private float intensity = 2f;
    [SerializeField] private float range = 20f;
    [SerializeField] private Sprite FlashLigthImage ;
    void Start()
    {
        if (flashlightSpot == null)
        {
            flashlightSpot = GetComponentInChildren<Light>(); // Auto-find if not set
        }

        ApplySettings();
    }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
                ToggleFlashlight();
            ApplySettings();
        }

    public void ToggleFlashlight()
    {
        isOn = !isOn;
    }

    public void SetIntensity(float newIntensity)
    {
        intensity = newIntensity;
        flashlightSpot.intensity = intensity;
    }

    public void SetRange(float newRange)
    {
        range = newRange;
        flashlightSpot.range = range;
    }

    private void ApplySettings()
    {
        flashlightSpot.intensity = intensity;
        flashlightSpot.range = range;
        flashlightSpot.enabled = isOn;
    }


}

