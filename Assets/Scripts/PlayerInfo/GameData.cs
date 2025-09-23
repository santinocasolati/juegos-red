using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviourPun
{
    public static GameData Instance;

    private Dictionary<Player, int> scores = new Dictionary<Player, int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeScores()
    {
        scores.Clear();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!scores.ContainsKey(p))
                scores.Add(p, 0);
        }
    }

    public void AddScore(Player p, int points)
    {
        if (!scores.ContainsKey(p))
            scores[p] = 0;

        scores[p] += points;
    }

    public int GetScore(Player p)
    {
        return scores.ContainsKey(p) ? scores[p] : 0;
    }

    public Dictionary<Player, int> GetAllScores()
    {
        return new Dictionary<Player, int>(scores);
    }
}
