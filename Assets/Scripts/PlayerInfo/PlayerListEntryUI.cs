using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListEntryUI : MonoBehaviour
{
    [SerializeField] private List<Sprite> characterSprites;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Image playerCharacterImage;

    public void SetPlayer(Player player, bool isLocalPlayer)
    {
        playerNameText.text = player.NickName;

        playerNameText.color = isLocalPlayer ? Color.green : Color.red;

        if (player.CustomProperties.TryGetValue("characterIndex", out object index))
        {
            playerCharacterImage.sprite = characterSprites[(int)index];
        }
    }
}
