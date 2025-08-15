using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyConnections : MonoBehaviourPunCallbacks
{
    private bool connected = false;
    private RoomOptions roomOptions;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        connected = true;

        roomOptions = new RoomOptions
        {
            IsVisible = false,
            MaxPlayers = 4
        };

        PhotonNetwork.JoinOrCreateRoom("sapeLoquita", roomOptions, TypedLobby.Default);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Desconectado de Photon. Motivo: " + cause);

        Invoke("Reconnect", 3f);
    }
    private void Reconnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Reintentando conexión a Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void CreateRoom()
    {
        if (!connected || !PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Photon no está listo aún.");
            return;
        }

        roomOptions = new RoomOptions
        {
            IsVisible = false,
            MaxPlayers = 4
        };

        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        else
            JoinOrCreateRoom();
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Te fuiste de la room.");
    }

    private void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("sapeLoquita", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
    }
}
