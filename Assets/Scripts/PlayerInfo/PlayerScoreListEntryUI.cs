using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreListEntryUI : MonoBehaviour
{
    [SerializeField] private Image character;
    [SerializeField] private TMP_Text nickname;
    [SerializeField] private TMP_Text score;
    [SerializeField] private List<Sprite> characterSprites;

    public void SetData(Player player, int currentScore)
    {
        if (player.CustomProperties.TryGetValue("characterIndex", out object index))
        {
            character.sprite = characterSprites[(int)index];
        }

        nickname.text = player.NickName;
        score.text = currentScore.ToString();
    }
}
