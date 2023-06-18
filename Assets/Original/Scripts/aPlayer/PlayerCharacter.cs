using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    float _modelRotationSpeedDeg;

    [SerializeField]
    float _minimumDistanceToGroundToBeConsideredAirborne = 0.5f;

    [SerializeField]
    Transform _modelTransformToRotate;

    #region EnumStates
    bool _isRotating;
    Quaternion _targetRotation = Quaternion.identity;

    void UpdateModelRotation()
    {
        if (_modelRotationSpeedDeg > 0)
        {
            _modelTransformToRotate.localRotation = Quaternion.Lerp(_modelTransformToRotate.localRotation, _targetRotation, Time.deltaTime * _modelRotationSpeedDeg);
        }
        else
        {
            _modelTransformToRotate.localRotation = _targetRotation;
        }
    }

    #endregion


    protected void Awake()
    {
        Initialization();

        PlayerDelegatesContainer.EventDeath += OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnStart += OnPlayerSpawnStart;

        PlayerDelegatesContainer.GetTransform += GetTransform;
        PlayerDelegatesContainer.GetPlayerCharacter += GetPlayerCharacterInstance;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventDeath -= OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnStart -= OnPlayerSpawnStart;

        PlayerDelegatesContainer.GetTransform -= GetTransform;
        PlayerDelegatesContainer.GetPlayerCharacter -= GetPlayerCharacterInstance;
    }

    void Initialization()
    {
    }

    #region UpdateLoop
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        UpdateModelRotation();
    }
    #endregion

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
