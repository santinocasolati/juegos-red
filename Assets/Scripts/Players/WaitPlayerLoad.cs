using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitPlayerLoad : MonoBehaviourPunCallbacks
{
    private int playersLoaded;

    public UnityEvent OnPlayersReady;

    private void Start()
    {
        photonView.RPC("RPC_SceneLoaded", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_SceneLoaded()
    {
        playersLoaded++;

        if (playersLoaded == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("RPC_AllReady", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_AllReady()
    {
        OnPlayersReady?.Invoke();
    }
}
