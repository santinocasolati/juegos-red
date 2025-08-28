using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PlayerListManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerEntryPrefab;

    public override void OnEnable()
    {
        base.OnEnable();
        RefreshPlayerList();
    }

    public void RefreshPlayerList()
    {
        foreach (Transform child in playerListParent)
            Destroy(child.gameObject);

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entryObj = Instantiate(playerEntryPrefab, playerListParent);
            PlayerListEntryUI entryUI = entryObj.GetComponent<PlayerListEntryUI>();
            bool isLocal = p == PhotonNetwork.LocalPlayer;
            entryUI.SetPlayer(p, isLocal);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerLeftRoom(newPlayer);
        RefreshPlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        RefreshPlayerList();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        RefreshPlayerList();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        foreach (Transform child in playerListParent)
            Destroy(child.gameObject);
    }
}
