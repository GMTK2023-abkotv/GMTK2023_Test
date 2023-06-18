using UnityEngine;
using Unity.Mathematics;

public class WalkingMovementState : PlayerMovementState
{
    [SerializeField]
    float _acceleration = 3;
    [SerializeField]
    float _maxSpeed = 3;

    float3 _inputDirection;
    float _currentSpeed;

    MoveCommand _moveCommand;

    void Start()
    {
        _moveCommand = InputDelegatesContainer.GetMoveCommand();
    }

    public override bool CheckForTransitions()
    {
        if (base.CheckForTransitions())
        {
            return true;
        }

        if (!PlayerDelegatesContainer.IsGrounded())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Falling);
            return true;
        }

        if (PlayerDelegatesContainer.IsGroundJumpTrigering())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.GroundJumping);
            return true;
        }

        if (PlayerDelegatesContainer.IsDashTriggering())
        { 
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Dash);
            return true;
        }

        if (!IsInputTriggeringAbility())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Idle);
            return true;
        }

        return false;
    }

    public override void OnTransition()
    {
        base.OnTransition();

        _currentSpeed = 0;
    }

    public override void GetDisplacement(out float3 displacement)
    {
        _currentSpeed += _acceleration * Time.deltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);

        displacement = _inputDirection * _currentSpeed * Time.deltaTime;
    }

    bool IsInputTriggeringAbility()
    {
        if (!_moveCommand.IsTriggered)
        {
            return false;
        }

        if (_moveCommand.CommandingToUp)
        {
            _inputDirection.z = 1;
        }
        if (_moveCommand.CommandingToDown)
        {
            _inputDirection.z = -1;
        }
        if (_moveCommand.CommandingToRight)
        {
            _inputDirection.x = 1;
        }
        if (_moveCommand.CommandingToLeft)
        {
            _inputDirection.x = -1;
        }

        _inputDirection = math.normalize(_inputDirection);
        return true;
    }


    public override string ToString()
    {
        return "Walk ab";
    }
}