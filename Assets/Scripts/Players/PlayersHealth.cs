using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHealth : MonoBehaviourPun
{
    private int maxHealth;
    private int currentHealth;

    public Action<Player> OnDeath;

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Damage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        if (!photonView.IsMine) return;

        if (currentHealth <= 0)
            photonView.RPC("PlayerDeath", RpcTarget.All);
    }

    [PunRPC]
    private void PlayerDeath()
    {
        OnDeath?.Invoke(photonView.Owner);
        Destroy(gameObject);
    }
}
