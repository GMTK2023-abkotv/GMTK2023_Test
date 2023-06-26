using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EnemiesSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject blobPrefab;

    [SerializeField]
    Transform spawnPointsParent;

    [SerializeField]
    float spawnTime;

    [SerializeField]
    int maxBlobsCount;

    List<Transform> spawnPoints;
    float lastSpawnTime;

    IEnumerator spawnCoroutine;

    void Awake()
    {
        spawnPoints = new();
        for (int i = 0; i < spawnPointsParent.childCount; i++)
        {
            spawnPoints.Add(spawnPointsParent.GetChild(i));
        }

        PlayerDelegatesContainer.EventPlayerAlive += OnPlayerAlive;
        PlayerDelegatesContainer.EventPlayerDead += OnPlayerDead;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventPlayerAlive -= OnPlayerAlive;
        PlayerDelegatesContainer.EventPlayerDead -= OnPlayerDead;
    }

    void OnPlayerAlive()
    {
        spawnCoroutine = SpawnCoroutine();
        StartCoroutine(spawnCoroutine);
    }

    void OnPlayerDead()
    {
        StopCoroutine(spawnCoroutine);
    }

    // stops on player death
    IEnumerator SpawnCoroutine()
    {
        yield return null;
        while (true)
        { 
            lastSpawnTime += Time.deltaTime;
            if (lastSpawnTime > spawnTime)
            {
                Spawn();
                lastSpawnTime = 0;
            }
            yield return null;
        }
    }

    void Spawn()
    {
        int rnd = Random.Range(0, spawnPoints.Count);
        Instantiate(blobPrefab, spawnPoints[rnd].position, Quaternion.identity);
    }
}