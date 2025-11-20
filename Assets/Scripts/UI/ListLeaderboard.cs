using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;

    private void OnEnable()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        WinLeaderboards.Instance.GetTop10(ListTop10);
    }

    private void ListTop10(LootLockerLeaderboardMember[] obj)
    {
        foreach (LootLockerLeaderboardMember member in obj)
        {
            GameObject entry = Instantiate(entryPrefab, transform);
            entry.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = $"#{member.rank}";
            entry.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = member.member_id;
            entry.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = member.score.ToString();
        }
    }
}
