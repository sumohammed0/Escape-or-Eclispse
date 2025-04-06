using UnityEngine;
using Photon.Pun; // Import the Photon Unity Networking namespace
using UnityEngine.SceneManagement; // Import the Unity Scene Management namespace

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon server using the settings defined in the PhotonServerSettings file
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); // Join the default lobby after connecting to the master server
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("LobbyScene"); // Load the Lobby scene after joining the lobby
    }
}
