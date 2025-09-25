using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListEntryUI : MonoBehaviour
{
    [SerializeField] private List<DisplayFrames> characterSprites;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private UISpriteAnimator characterImg;

    public void SetPlayer(Player player)
    {
        playerNameText.text = player.NickName;

        if (player.CustomProperties.TryGetValue("characterIndex", out object index))
        {
            characterImg.frames = characterSprites[(int)index].sprites;
        }

        playerNameText.color = Color.red;
    }

    public void SetPlayerReady(bool isReady)
    {
        playerNameText.color = isReady ? Color.green : Color.red;
    }
}
