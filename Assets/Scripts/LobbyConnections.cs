using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyConnections : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField usernameInput;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Callback: Connected to Master Server.");
    }

    public override void OnConnected()
    {
        Debug.Log("Callback: Connected to NameServer.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon: " + cause);
    }

    public void OnConnectButton()
    {
        PhotonNetwork.NickName = string.IsNullOrEmpty(usernameInput.text)
            ? "Player" + Random.Range(1000, 9999)
            : usernameInput.text;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            JoinOrCreateRoom();
        }
        else
        {
            Debug.Log("Still connecting...");
        }
    }

    void JoinOrCreateRoom()
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("DefaultRoom", options, TypedLobby.Default);
        Debug.Log("Joining/Creating room...");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);

        ServiceLocator.Instance.AccessService<UIPagesService>().ChangePage("room_lobby");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        ServiceLocator.Instance.AccessService<UIPagesService>().ChangePage("enter_room");
    }
}
