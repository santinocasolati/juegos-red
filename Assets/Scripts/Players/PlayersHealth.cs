using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHealth : MonoBehaviourPun
{
    private int maxHealth = 100;
    private int currentHealth = 100;

    public Action<Player> OnDeath;

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Damage(int amount)
    {
        if (!GameManager.Instance.gameStarted) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        if (!photonView.IsMine) return;

        if (currentHealth <= 0)
            photonView.RPC("RPC_PlayerDeath", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_PlayerDeath()
    {
        OnDeath?.Invoke(photonView.Owner);

        if (gameObject == null) return;

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
