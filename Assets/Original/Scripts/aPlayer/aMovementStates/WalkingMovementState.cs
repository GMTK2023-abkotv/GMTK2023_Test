using UnityEngine;
using Unity.Mathematics;

public class WalkingMovementState : PlayerMovementState
{
    [SerializeField]
    float _acceleration = 3;
    [SerializeField]
    float _maxSpeed = 3;

    float3 _inputDirection;
    float3 _currentVelocity;

    MoveCommand _moveCommand;

    bool _isAccelerating;

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
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.Falling);
            return true;
        }

        if (PlayerDelegatesContainer.IsGroundJumpTrigering())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.Jumping);
            return true;
        }

        _isAccelerating = IsInputTriggeringAbility();

        if (!_isAccelerating && math.lengthsq(_currentVelocity) == 0)
        {
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.Idle);
            return true;
        }

        if (PlayerDelegatesContainer.IsDashTriggering())
        {
            PlayerDelegatesContainer.EventDashFromWalking(_inputDirection);
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.Dash);
            return true;
        }

        return false;
    }

    public override void OnTransition()
    {
        base.OnTransition();

        _currentVelocity = float3.zero;
    }

    public override void GetDisplacement(out float3 displacement)
    {
        float a = _isAccelerating ? _acceleration : -_acceleration;
        _currentVelocity += _inputDirection * a * Time.deltaTime;
        if (math.lengthsq(_currentVelocity) > _maxSpeed * _maxSpeed)
        {
            _currentVelocity = _inputDirection * _maxSpeed;
        }

        displacement = _currentVelocity * Time.deltaTime;
        displacement.y = -0.1f;
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