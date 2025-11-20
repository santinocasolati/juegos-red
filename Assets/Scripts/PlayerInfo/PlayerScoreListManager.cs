using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScoreListManager : MonoBehaviourPun
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private string nextMinigameScene;
    [SerializeField] private string winScene;
    [SerializeField] private float delaySeconds;

    private void Start()
    {
        ListScores();
    }

    private void ListScores()
    {
        Dictionary<Player, int> scores = GameData.Instance.GetAllScores();

        foreach (KeyValuePair<Player, int> pair in scores.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
        {
            GameObject score = Instantiate(scorePrefab, container);
            score.GetComponent<PlayerScoreListEntryUI>().SetData(pair.Key, pair.Value);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(delaySeconds);

        if (PhotonNetwork.PlayerList.Length > 1)
        {
            string nextScene = GameData.Instance.CheckWin() ? winScene : nextMinigameScene;
            PhotonNetwork.LoadLevel(nextScene);
        }
        else
        {
            PhotonNetwork.LoadLevel(winScene);
        }   
    }
}
