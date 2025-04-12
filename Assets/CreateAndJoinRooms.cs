using UnityEngine;
using UnityEngine.UI; // Import the Unity UI namespace
using Photon.Pun;
using TMPro; // Import the Photon Unity Networking namespace

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text); // Create a new room with the specified name and options
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text); // Join an existing room with the specified name
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Puzzle2Scene"); // Load the game scene after successfully joining a room
    }
}
