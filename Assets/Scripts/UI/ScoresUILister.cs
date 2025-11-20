using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoresUILister : MonoBehaviourPun
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private List<Sprite> sprites;

    private Dictionary<Player, TMP_Text> scores = new();

    private void Awake()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject obj = Instantiate(entryPrefab, transform);

            if (p.CustomProperties.TryGetValue("characterIndex", out object index))
            {
                obj.transform.GetChild(0).GetComponent<Image>().sprite = sprites[(int)index];
                scores.Add(p, obj.transform.GetChild(1).GetComponent<TMP_Text>());
            }
        }
    }

    public void UpdateScores()
    {
        foreach (KeyValuePair<Player, int> kvp in ((ReachScoreGameManager)GameManager.Instance).Scores)
        {
            scores[kvp.Key].text = kvp.Value.ToString();
        }
    }
}
