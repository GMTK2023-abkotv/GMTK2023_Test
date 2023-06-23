using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    protected void Awake()
    {
        PlayerDelegatesContainer.EventDeath += OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnStart += OnPlayerSpawnStart;

        PlayerDelegatesContainer.GetTransform += GetTransform;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventDeath -= OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnStart -= OnPlayerSpawnStart;

        PlayerDelegatesContainer.GetTransform -= GetTransform;
    }

    #region EventsHandling
    void OnPlayerDeath()
    {
        //_conditionState.SetState(PlayerConditionStateType.Dead);
    }

    void OnPlayerSpawnStart(CheckPoint spawnCheckPoint)
    {
        transform.position = spawnCheckPoint.transform.position;
    }
    #endregion

    #region Getters
    Transform GetTransform()
    {
        return transform;
    }

    PlayerCharacter GetPlayerCharacterInstance()
    {
        return this;
    }
    #endregion
}
