using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string playerPrefabName;
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        Transform spawn = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber % spawnPoints.Count];

        PhotonNetwork.Instantiate(playerPrefabName, spawn.position, spawn.rotation);
    }
}
