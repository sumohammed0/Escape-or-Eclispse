using UnityEngine;
using UnityEngine.SceneManagement;

public class DesktopCameraLook : MonoBehaviour
{
    // This script is responsible for controlling the camera's look direction using mouse input, for the desktop version of the game.
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;
    bool isActiveScene = false;

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Only lock and hide the cursor in gameplay scene
        if (currentScene == "Puzzle2Scene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isActiveScene = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isActiveScene = false;
        }
    }

    void Update()
    {
        if (!isActiveScene) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
