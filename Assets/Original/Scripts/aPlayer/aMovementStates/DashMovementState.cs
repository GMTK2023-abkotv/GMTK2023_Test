using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class DashMovementState : PlayerMovementState
{
    [SerializeField]
    float _distanceOfDash;

    [SerializeField]
    float _startDashSpeed;

    [SerializeField]
    float _dashCoolDown = 2;

    [SerializeField]
    float _deceleration = 3;

    [SerializeField]
    float _decelerateMinSpeed = 10;

    float _currentSpeed;

    GameObject _coolDownIndicator;

    float _movedDistance;
    bool _isDashExhausted;

    DashCommand _dashCommand;
    MoveCommand _moveCommand;

    bool _isInCoolDown;
    float3 _movementDirection;
    bool _isFromWalking;

    void Awake()
    {
        PlayerDelegatesContainer.EventJustGotGrounded += OnJustGotGrounded;

        PlayerDelegatesContainer.IsDashTriggering += IsDashTriggering;

        PlayerDelegatesContainer.EventDashFromWalking += OnDashFromWalking;
    }

    void Start()
    {
        _dashCommand = InputDelegatesContainer.GetDashCommand();
        _moveCommand = InputDelegatesContainer.GetMoveCommand();
        _coolDownIndicator = UIDelegatesContainer.GetDashCoolDownIndicator();
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventJustGotGrounded -= OnJustGotGrounded;

        PlayerDelegatesContainer.IsDashTriggering -= IsDashTriggering;

        PlayerDelegatesContainer.EventDashFromWalking -= OnDashFromWalking;
    }

    void OnJustGotGrounded()
    {
        if (!_isInCoolDown && _isDashExhausted)
        {
            StartCoroutine(DashCoolDown());
        }
    }

    public override void OnEntry()
    {
        PlayerDelegatesContainer.EventGroundedIgnoranceStart?.Invoke();
        _coolDownIndicator?.SetActive(false);
        if (!_isInCoolDown)
        {
            StartCoroutine(DashCoolDown());
        }
        _movedDistance = 0;
        _isDashExhausted = true;

        if (!_isFromWalking)
        {
            _movementDirection = math.forward();
        }

        _currentSpeed = _startDashSpeed;
    }

    public override bool CheckForTransitions()
    {
        if (base.CheckForTransitions())
        {
            return true;
        }

        if (_movedDistance > _distanceOfDash)
        {
            if (PlayerDelegatesContainer.IsGrounded())
            {
                if (_moveCommand.IsTriggered)
                {
                    PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Walking);
                }
                else
                { 
                    PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Idle);
                }
            }
            else
            {
                PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Falling);
            }
            return true;
        }

        return false;
    }

    public override void OnTransition()
    {
        _isFromWalking = false;
        PlayerDelegatesContainer.EventGroundedIgnoranceEnd?.Invoke();
    }

    public override void GetDisplacement(out float3 displacement)
    {
        displacement = float3.zero;
        displacement = _movementDirection * _currentSpeed * Time.deltaTime;
        _movedDistance += math.length(displacement);

        _currentSpeed -= _deceleration * Time.deltaTime;
        if (_currentSpeed < _decelerateMinSpeed)
        {
            _currentSpeed = _decelerateMinSpeed;
        }
    }

    bool IsDashTriggering()
    {
        if (_isDashExhausted)
        {
            return false;
        }

        if (!_dashCommand.IsTriggered)
        {
            return false;
        }

        return true;
    }

    void OnDashFromWalking(float3 movementDirection)
    {
        _movementDirection = movementDirection;
        _isFromWalking = true;
        Debug.Log("From walking");
    }

    IEnumerator DashCoolDown()
    {
        _isInCoolDown = true;
        yield return new WaitForSeconds(_dashCoolDown);
        _isInCoolDown = false;
        _isDashExhausted = false;
        _coolDownIndicator?.SetActive(true);
    }
}