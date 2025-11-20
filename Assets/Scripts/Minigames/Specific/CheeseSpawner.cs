using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseSpawner : MonoBehaviourPun
{
    [SerializeField] private string cheesePrefabName;
    [SerializeField] private float spawnInterval = .5f;

    private Coroutine spawnRoutine;

    public void StartSpawning()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnCheese();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCheese()
    {
        PhotonNetwork.Instantiate(cheesePrefabName, new Vector3(0, 100, 0), Quaternion.identity);
    }
}
