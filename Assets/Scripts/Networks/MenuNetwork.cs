using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNetwork : MonoBehaviour
{
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    //This is the clients version number (separates players from each other)
    string _gameVersion = "1";

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players and so new room will be created")]
    public byte MaxPlayersPerRoom = 3;

    [Tooltip("The Ui Panel to let the user enter name and connect to lobby")]
    public GameObject LoginPanel;

    [Tooltip("The UI Panel to allow people to start/leave a lobby")]
    public GameObject LobbyPanel;

    public GameObject StartButton;

    private bool isConnecting = false;
    

    private void Awake()
    {
        PhotonNetwork.logLevel = Loglevel;
        // we dont join lobby, we can retrieve list of rooms without joining lobby
        PhotonNetwork.autoJoinLobby = false;
        // ths makes sure we use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level
        PhotonNetwork.automaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        LobbyPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void Connect()
    {
        isConnecting = true;
        LobbyPanel.SetActive(true);
        LoginPanel.SetActive(false);
        //Check if we are connected to a server
        if (PhotonNetwork.connected)
        {
            Debug.LogError("We are connected, joining room");
            //if we are connected we join a room
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("We are connecting using game verrsion");
            //connect using gameVersion
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    private void OnConnectedToMaster()
    {
        print("We are connected to master, attempting to join room");
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    private void OnJoinedRoom()
    {
        if(!PhotonNetwork.isMasterClient)
        {
            StartButton.SetActive(false);
        }
    }
    public void StartGame()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
        //PhotonNetwork.Disconnect();
    }
}
