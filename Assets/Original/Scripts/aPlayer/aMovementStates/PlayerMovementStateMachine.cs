using UnityEngine;
using Unity.Mathematics;

using TMPro;

public enum PlayerMovementStateType
{ 
	Idle,
	Dash,
	Walking,
	Falling,
    Jumping,
}

public class PlayerMovementStateMachine : MonoBehaviour
{
    PlayerController _playerController;

    IdleMovementState        _idleMovementState;
    DashMovementState        _dashMovementState;
    WalkingMovementState     _walkingMovementState;
    FallingMovementState     _fallingMovementState;

    JumpGroundMovementState   _jumpGroundMovementState;

    const float CLEAR_LOG_TIME = 1f;

    const int MAX_CHANGES_COUNT_IN_ONE_FRAME = 10;
    int _changesCountOneFrame = 0;

    bool _areAbilitiesActive;

    PlayerMovementState _currentState;
    PlayerMovementStateType _currentStateType;
    float3 _currentDisplacement;

    PlayerMovementStateType _previousStateType;

    TextMeshProUGUI _stateText;

    void Awake()
    {
        bool areAllHere =
            TryGetComponent(out _idleMovementState) &&
            TryGetComponent(out _dashMovementState) &&
            TryGetComponent(out _walkingMovementState) &&
            TryGetComponent(out _fallingMovementState) &&

            TryGetComponent(out _jumpGroundMovementState);


        if (!areAllHere)
        {
            Debug.LogError("Some movement state is missing. Check the " + gameObject.name + " or the script itself");
        }

        _areAbilitiesActive = true;

        PlayerDelegatesContainer.EventEntryNewMovementState += OnEntryNewMovementState;

        PlayerDelegatesContainer.EventDeath += OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnEnd += OnPlayerSpawn;

        PlayerDelegatesContainer.GetPreviousStateType += GetPreviousStateType;
    }

    void OnDestroy()
    { 
        PlayerDelegatesContainer.EventEntryNewMovementState -= OnEntryNewMovementState;

        PlayerDelegatesContainer.EventDeath -= OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnEnd -= OnPlayerSpawn;

        PlayerDelegatesContainer.GetPreviousStateType -= GetPreviousStateType;
    }

    void Start()
    {
        _currentState = PlayerDelegatesContainer.GetInitialMovementState();

        _playerController = PlayerDelegatesContainer.GetPlayerController();

        _stateText = UIDelegatesContainer.GetStateText();
    }

    void OnEntryNewMovementState(PlayerMovementStateType newStateType)
    {
        _previousStateType = _currentStateType;
        _changesCountOneFrame++;

        if (_changesCountOneFrame > MAX_CHANGES_COUNT_IN_ONE_FRAME)
        {
            Debug.LogError("Changes in one frame exceeded max count");
            Debug.Break();
            return;
        }

        _currentState.OnTransition();
        SetCurrentStateFromStateType(newStateType, out bool shouldReturn);
        if (shouldReturn)
        {
            return;
        }

        _currentStateType = newStateType;

        _currentState.OnEntry();
        if (_currentState.CheckForTransitions())
        {
            return;
        }

        _stateText.text = newStateType.ToString();
        _currentState.GetDisplacement(out _currentDisplacement);
        _playerController.ApplyDisplacement(_currentDisplacement);
    }

    /// <summary>
    /// Almost the same as OnNewMovementState, except that is does not call OnEntry();
    /// </summary>
    void RestorePreviousState()
    {
        SetCurrentStateFromStateType(_previousStateType, out bool shouldReturn);
        if (shouldReturn)
        {
            Debug.LogError("returning on restoring");
            return;
        }       

        _currentStateType = _previousStateType;

        if (_currentState.CheckForTransitions())
        {
            return;
        }

        _currentState.GetDisplacement(out _currentDisplacement);
        _playerController.ApplyDisplacement(_currentDisplacement);
    }

    void Update()
    { 
        if (Time.timeScale == 0f || !_areAbilitiesActive)
        {
            return;
        }

        if (_currentState.CheckForTransitions())
        {
            PlayerDelegatesContainer.EventFinalFrameMovementState?.Invoke(_currentStateType);
            //System.GC.Collect();
            return;
        }

        _currentState.GetDisplacement(out _currentDisplacement);
        _playerController.ApplyDisplacement(_currentDisplacement);
    }

    void OnTeleportingMovementEntry()
    {
        _previousStateType = _currentStateType;
        _currentState.OnEntry();
    }

    void OnPlayerDeath()
    {
        _areAbilitiesActive = false;
    }

    void OnPlayerSpawn()
    {
        _areAbilitiesActive = true;
        _currentState = _idleMovementState;
        _currentStateType = PlayerMovementStateType.Idle;
        PlayerDelegatesContainer.EventFinalFrameMovementState?.Invoke(_currentStateType);
    }

    void SetCurrentStateFromStateType(PlayerMovementStateType stateType, out bool shouldReturn)
    { 
        switch (stateType)
        { 
            case PlayerMovementStateType.Idle:
                _currentState = _idleMovementState;
                break;
            case PlayerMovementStateType.Dash:
                _currentState = _dashMovementState;
                break;
            case PlayerMovementStateType.Walking:
                _currentState = _walkingMovementState;
                break;
            case PlayerMovementStateType.Falling:
                _currentState = _fallingMovementState;
                break;
            case PlayerMovementStateType.Jumping:
                _currentState = _jumpGroundMovementState;
                break;
        }
        shouldReturn = false;
    }

    PlayerMovementStateMachine GetAbilitiesControllerInstance()
    {
        return this;
    }

    PlayerMovementStateType GetPreviousStateType()
    {
        return _previousStateType;
    }

    void LateUpdate()
    {
        _changesCountOneFrame = 0;
    }

    void Log(string msg)
    {
        Debug.Log("StateMachine: " + msg);
    }
}