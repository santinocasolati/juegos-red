using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerManager : MonoBehaviourPun
{
    [SerializeField] private Image character;
    [SerializeField] private TMP_Text nickname;
    [SerializeField] private string menuScene;
    [SerializeField] private List<Sprite> characterSprites;

    private void Start()
    {
        ShowWinner();
    }

    private void ShowWinner()
    {
        Player winner = GameData.Instance.GetWinner();

        if (winner.CustomProperties.TryGetValue("characterIndex", out object index))
        {
            character.sprite = characterSprites[(int)index];
        }

        nickname.text = winner.NickName;

        if (winner == PhotonNetwork.LocalPlayer)
        {
            WinLeaderboards.Instance.AddOneToScore();
        }

        PhotonNetwork.Disconnect();
    }

    public void Exit()
    {
        SceneManager.LoadSceneAsync(menuScene, LoadSceneMode.Single);
    }
}
