using UnityEngine;
using Photon.Pun; // Import the Photon Unity Networking namespace

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab; // Reference to the player prefab
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;

    private void Start()
    {
        // Spawn the player at a random position within the specified bounds
        Vector3 randomPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ)
        );

        // Instantiate the player prefab over the network
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

        // Search for the camera in the prefab hierarchy explicitly
        Transform cameraTransform = player.transform.Find("XRCardboardRig/HeightOffset/Main Camera");

        if (cameraTransform != null)
        {
            Camera playerCamera = cameraTransform.GetComponent<Camera>();

            if (playerCamera != null)
            {
                // Check if the PhotonView is owned by the local player
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    playerCamera.enabled = true;
                    if (Camera.main != null)
                    {
                        Camera.main.gameObject.SetActive(false);  // Disable the current main camera
                    }
                    playerCamera.gameObject.SetActive(true);  // Ensure the new camera is active
                }
                else
                {
                    playerCamera.enabled = false;
                }
            }
        }
    }
}
