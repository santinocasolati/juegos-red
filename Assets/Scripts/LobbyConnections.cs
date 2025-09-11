using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyConnections : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private CharacterSelection characterSelection;
    [SerializeField] private string sceneToLoad;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        usernameInput.text = PlayerPrefs.GetString("Username");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        JoinOrCreateRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Disconnected from Photon: " + cause);
    }

    public void OnConnectButton()
    {
        if (string.IsNullOrEmpty(usernameInput.text)) return;

        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("Username", usernameInput.text);

        int characterIndex = characterSelection.CurrentIndex;
        var props = new Hashtable { { "characterIndex", characterIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        PlayerPrefs.SetInt("Character", characterIndex);

        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnDisconnectButton()
    {
        PhotonNetwork.Disconnect();
    }

    private void JoinOrCreateRoom()
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("DefaultRoom", options, TypedLobby.Default);
        Debug.Log("Joining/Creating room...");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);

        ServiceLocator.Instance.AccessService<UIPagesService>().ChangePage("room_lobby");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Left Room");

        ServiceLocator.Instance.AccessService<UIPagesService>().ChangePage("enter_room");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (!PhotonNetwork.IsMasterClient) return;

        if(changedProps.ContainsKey("Ready"))
        {
            CheckAllReady();
        }
    }

    private void CheckAllReady()
    {
        //if (PhotonNetwork.PlayerList.Length <= 1) return;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!p.CustomProperties.TryGetValue("Ready", out object readyObj) || !(bool)readyObj)
            {
                return;
            }
        }

        PhotonNetwork.LoadLevel(sceneToLoad);
    }
}
