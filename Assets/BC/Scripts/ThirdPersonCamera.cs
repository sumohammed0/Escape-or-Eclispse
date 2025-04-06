using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;      // The player character
    public Vector3 offset = new Vector3(0f, 3f, -6f); // Position behind and above
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}

