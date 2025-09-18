using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string playerPrefabName;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private List<Transform> spawnPoints;

    private List<GameObject> playerInstances = new();

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        Transform spawn = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber % spawnPoints.Count];

        playerInstances.Add(PhotonNetwork.Instantiate(playerPrefabName, spawn.position, spawn.rotation));
    }

    public void OnAllLoaded()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        countdownText.transform.parent.gameObject.SetActive(true);

        yield return StartCoroutine(ShowText("3"));
        yield return StartCoroutine(ShowText("2"));
        yield return StartCoroutine(ShowText("1"));
        yield return StartCoroutine(ShowText("GO!"));

        countdownText.transform.parent.gameObject.SetActive(false);

        foreach (GameObject player in playerInstances)
        {
            player.GetComponent<PlayerControls>().canMove = true;
        }
    }

    private IEnumerator ShowText(string text)
    {
        countdownText.text = text;
        countdownText.transform.localScale = Vector3.one * 0.75f;

        float elapsed = 0f;
        while (elapsed < 1)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 1);

            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            countdownText.transform.localScale = Vector3.Lerp(
                Vector3.one * 0.75f,
                Vector3.one,
                easedT
            );

            yield return null;
        }
    }
}
