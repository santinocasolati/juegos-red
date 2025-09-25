using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStandingGameManager : GameManager
{
    [SerializeField] private int initialHealth = 1;

    private List<Player> alivePlayers = new();

    protected override void StartGame()
    {
        base.StartGame();
        alivePlayers.AddRange(PhotonNetwork.PlayerList);

        foreach (GameObject player in playerInstances)
        {
            PlayersHealth playersHealth = player.GetComponent<PlayersHealth>();
            playersHealth.SetMaxHealth(initialHealth);
            playersHealth.OnDeath += PlayerDied;
        }
    }

    public void PlayerDied(Player player)
    {
        if (!gameStarted) return;

        alivePlayers.Remove(player);

        if (!PhotonNetwork.IsMasterClient) return;

        if (alivePlayers.Count <= 1)
        {
            int winnerActorNumber = alivePlayers[0].ActorNumber;
            photonView.RPC("RPC_AnnounceWinner", RpcTarget.All, winnerActorNumber);
        }
    }
}
