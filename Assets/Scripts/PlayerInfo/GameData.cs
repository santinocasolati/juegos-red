using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [SerializeField] private int pointsToWin = 3;

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

    public void ClearPlayers()
    {
        scores.Clear();
    }

    public void RegisterPlayer(Player p)
    {
        Debug.Log("Register " + p.NickName);

        if (!scores.ContainsKey(p))
            scores.Add(p, 0);
    }

    public void UnregisterPlayer(Player p)
    {
        Debug.Log("Unregister " + p.NickName);

        if (scores.ContainsKey(p))
            scores.Remove(p);
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

    public bool CheckWin()
    {
        return scores.Where(kvp => kvp.Value >= pointsToWin).ToList().Count > 0;
    }

    public Player GetWinner()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
            return scores.Where(kvp => kvp.Value >= pointsToWin).ToList()[0].Key;
        else
            return PhotonNetwork.PlayerList[0];
    }
}
