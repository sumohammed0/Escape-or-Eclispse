using UnityEngine;
using System.Collections;

public class AKDirectionalLightGradualDimmer : MonoBehaviour
{
    [SerializeField] private Light[] directionalLights;
    [SerializeField] private float totalDuration = 600f; // 10 minutes
    [SerializeField] private float stepInterval = 120f; // 2 minutes

    private float[] originalIntensities;
    private int steps;
    private float[] stepAmounts;

    void Start()
    {
        // Save original intensities
        originalIntensities = new float[directionalLights.Length];
        for (int i = 0; i < directionalLights.Length; i++)
        {
            if (directionalLights[i] != null)
                originalIntensities[i] = directionalLights[i].intensity;
        }

        // How many steps do we need
        steps = Mathf.FloorToInt(totalDuration / stepInterval);

        // Calculate reduction amount for each step
        stepAmounts = new float[steps + 1];
        for (int s = 0; s <= steps; s++)
        {
            stepAmounts[s] = 1f - ((float)s / steps); // 1.0 to 0.0
        }

        StartCoroutine(DimInSteps());
    }

    IEnumerator DimInSteps()
    {
        for (int i = 0; i <= steps; i++)
        {
            float multiplier = stepAmounts[i];

            for (int l = 0; l < directionalLights.Length; l++)
            {
                if (directionalLights[l] != null)
                {
                    directionalLights[l].intensity = originalIntensities[l] * multiplier;
                }
            }

            yield return new WaitForSeconds(stepInterval);
        }
    }
}
