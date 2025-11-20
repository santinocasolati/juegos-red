using LootLocker.Requests;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLeaderboards : MonoBehaviour
{
    public static WinLeaderboards Instance;

    private const string leaderboardKey = "wins";
    private int currentScore = 0;
    private string playerID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            Login();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Login()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.LogError("Failed to login");
                return;
            }

            playerID = response.player_id.ToString();
            Debug.Log("Logged in as " + playerID);

            FetchPlayerScore();
        });
    }

    private void FetchPlayerScore()
    {
        if (string.IsNullOrEmpty(playerID))
        {
            Debug.LogWarning("playerUlid is empty, cannot fetch leaderboard entry.");
            currentScore = 0;
            return;
        }

        LootLockerSDKManager.GetMemberRank(
            leaderboardKey,
            playerID,
            (response) =>
            {
                if (!response.success)
                {
                    Debug.Log("No existing leaderboard entry or failed to fetch member rank. Setting currentScore = 0");
                    currentScore = 0;
                    return;
                }
                try
                {
                    currentScore = response.score;
                    Debug.Log($"Fetched existing leaderboard score: {currentScore}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning("Couldn't parse member score from response: " + ex.Message);
                    currentScore = 0;
                }
            }
        );
    }

    public void AddOneToScore()
    {
        currentScore += 1;

        LootLockerSDKManager.SubmitScore(
            playerID,
            currentScore,
            leaderboardKey,
            (response) =>
            {
                if (response.success)
                    Debug.Log("Score updated to " + currentScore);
                else
                    Debug.LogError("Failed to submit score");
            }
        );
    }

    public void GetTop10(System.Action<LootLockerLeaderboardMember[]> onResult)
    {
        LootLockerSDKManager.GetScoreList(
            leaderboardKey,
            10,
            (response) =>
            {
                if (!response.success)
                {
                    Debug.LogError("Failed to retrieve leaderboard");
                    onResult?.Invoke(null);
                    return;
                }

                onResult?.Invoke(response.items);
            }
        );
    }
}
