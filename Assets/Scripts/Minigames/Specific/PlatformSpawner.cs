using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviourPun
{
    [SerializeField] private string platformPrefabName;
    [SerializeField] private float spawnHeight = 2f;
    [SerializeField] private float horizontalRange = 8f;
    [SerializeField] private float spawnInterval = .5f;
    [SerializeField] private int platformsPerSpawn = 3;

    private float nextY;
    private Coroutine spawnRoutine;

    public void StartSpawning()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        nextY = spawnHeight;

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
            SpawnPlatform();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnPlatform()
    {
        for (int i = 0; i < platformsPerSpawn; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(-horizontalRange, horizontalRange),
                nextY,
                0f
            );

            PhotonNetwork.Instantiate(platformPrefabName, spawnPos, Quaternion.identity);
        }

        nextY += spawnHeight;
    }
}
