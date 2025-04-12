using UnityEngine;
using UnityEngine.UIElements;

public class AKDialCreater : MonoBehaviour
{
    [Header("World Space Dial Settings")]
    public Transform[] itemsToArrange;
    public Transform centerPoint;
    public bool rotateTowardsCenter = false;
    public float radiousMultiPlier = .17f;
    void Start()
    {
        ArrangeAroundCenter();
    }
    private void Update()
    {
    }
    public void UpdateRadious()
    {
        if (itemsToArrange == null || itemsToArrange.Length == 0)
        {
            Debug.LogWarning("No items assigned to arrange.");
            return;
        }
        // Calculate radius from the first item
        // Evenly distribute angles around the circle
        for (int i = 0; i < itemsToArrange.Length; i++)
        {
            Vector3 direction = centerPoint.position - itemsToArrange[i].position;
            direction = direction.normalized;
            itemsToArrange[i].position = centerPoint.position -  direction * radiousMultiPlier;
        }
    }
    public void ArrangeAroundCenter()
    {
        if (itemsToArrange == null || itemsToArrange.Length == 0)
        {
            Debug.LogWarning("No items assigned to arrange.");
            return;
        }
        // Calculate radius from the first item
        float radius = Vector3.Distance(centerPoint.localPosition, itemsToArrange[0].localPosition);
        // Evenly distribute angles around the circle
        float angleStep = 360 / itemsToArrange.Length;
        for (int i = 0; i < itemsToArrange.Length; i++)
        {
            int currentIndex = i - (int)(itemsToArrange.Length / 2);
            float angle = currentIndex * angleStep;// * Mathf.Deg2Rad;
            itemsToArrange[i].position = centerPoint.position + new Vector3(0,Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad))*radius* radiousMultiPlier;
            itemsToArrange[i].rotation = Quaternion.Euler(angle, 0, 0); // Set rotation around X-axis
        }
    }
}
