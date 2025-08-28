using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;

    public void SetPlayer(Player player, bool isLocalPlayer)
    {
        playerNameText.text = player.NickName;

        playerNameText.color = isLocalPlayer ? Color.green : Color.red;
    }
}
