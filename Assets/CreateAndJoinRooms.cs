using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected to Photon. Can't create room.");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // You can change this as needed

        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        Debug.Log("Creating room: " + createInput.text);
    }

    public void JoinRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected to Photon. Can't join room.");
            return;
        }

        PhotonNetwork.JoinRoom(joinInput.text);
        Debug.Log("Attempting to join room: " + joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Puzzle2Scene");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Room creation failed. Code: {returnCode}, Message: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Joining room failed. Code: {returnCode}, Message: {message}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Disconnected from Photon. Reason: {cause}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connecting to Photon...");
        }
    }
}

//using UnityEngine;
//using UnityEngine.UI; // Import the Unity UI namespace
//using Photon.Pun;
//using TMPro; // Import the Photon Unity Networking namespace

//public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
//{
//    public TMP_InputField createInput;
//    public TMP_InputField joinInput;

//    public void CreateRoom()
//    {
//        PhotonNetwork.CreateRoom(createInput.text); // Create a new room with the specified name and options
//        Debug.Log("Creating room: " + createInput.text); // Log the name of the room being created
//    }

//    public void JoinRoom()
//    {
//        PhotonNetwork.JoinRoom(joinInput.text); // Join an existing room with the specified name
//        Debug.Log("Joining room: " + joinInput.text); // Log the name of the room being joined
//    }

//    public override void OnJoinedRoom()
//    {
//        PhotonNetwork.LoadLevel("Puzzle2Scene"); // Load the game scene after successfully joining a room
//        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name); // Log the name of the joined room
//    }
//}
