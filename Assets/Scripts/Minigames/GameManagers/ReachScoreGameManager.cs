using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReachScoreGameManager : GameManager
{
    [SerializeField] private int pointsToWin = 100;

    private Dictionary<Player, int> playersScore = new();

    public Dictionary<Player, int> Scores {  get { return playersScore; } }

    public UnityEvent OnScoresModified;

    protected override void StartGame()
    {
        base.StartGame();
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            playersScore.Add(p, 0);
        }

        foreach (GameObject player in playerInstances)
        {
            player.GetComponent<PlayerControls>().canAttack = true;
        }
    }

    public void AddScore(Player player, int score)
    {
        photonView.RPC("RPC_AddScore", RpcTarget.All, player, score);
    }

    [PunRPC]
    private void RPC_AddScore(Player player, int score)
    {
        playersScore[player] += score;

        if (playersScore[player] >= pointsToWin)
            photonView.RPC("RPC_AnnounceWinner", RpcTarget.All, player.ActorNumber);
        else
            OnScoresModified?.Invoke();
    }

    protected override void PlayerLeft(Player p)
    {
        base.PlayerLeft(p);

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            photonView.RPC("RPC_AnnounceWinner", RpcTarget.All, PhotonNetwork.PlayerList[0].ActorNumber);
        }
    }
}
