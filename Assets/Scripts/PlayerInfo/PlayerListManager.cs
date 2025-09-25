using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerEntryPrefab;

    private Dictionary<Player, PlayerListEntryUI> playerList = new();

    public override void OnEnable()
    {
        base.OnEnable();
        RefreshPlayerList();
    }

    public void RefreshPlayerList()
    {
        foreach (Transform child in playerListParent)
            Destroy(child.gameObject);

        playerList.Clear();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entryObj = Instantiate(playerEntryPrefab, playerListParent);
            PlayerListEntryUI entryUI = entryObj.GetComponent<PlayerListEntryUI>();
            entryUI.SetPlayer(p);
            playerList.Add(p, entryUI);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.ContainsKey("Ready"))
        {
            if (playerList.ContainsKey(targetPlayer))
            {
                bool notReady = (targetPlayer.CustomProperties.TryGetValue("Ready", out object readyObj) && (bool)readyObj);
                playerList[targetPlayer].SetPlayerReady(notReady);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"{newPlayer.UserId} connected");
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
