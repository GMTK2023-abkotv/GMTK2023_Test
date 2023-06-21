using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    float _delayBeforeSpawn;

    void Awake()
    {
        PlayerDelegatesContainer.EventDeath += OnPlayerDeath;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventDeath -= OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        CheckPoint lastCheckPoiint = ApplicationDelegatesContainer.GetLastCheckPoint();
        StartCoroutine(SpawnSequence(lastCheckPoiint));
    }

    IEnumerator SpawnSequence(CheckPoint validCheckPoint)
    {
        PlayerDelegatesContainer.EventSpawnStart?.Invoke(validCheckPoint);
        yield return new WaitForSeconds(_delayBeforeSpawn);
        PlayerDelegatesContainer.EventSpawnEnd?.Invoke();
    }
}
