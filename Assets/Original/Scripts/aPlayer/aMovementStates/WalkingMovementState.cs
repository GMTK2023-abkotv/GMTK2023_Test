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
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.Falling);
            return true;
        }

        if (PlayerDelegatesContainer.IsGroundJumpTrigering())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.GroundJumping);
            return true;
        }

        if (!IsInputTriggeringAbility())
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

        _currentSpeed = 0;
    }

    public override void GetDisplacement(out float3 displacement)
    {
        _currentSpeed += _acceleration * Time.deltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);

        displacement = _inputDirection * _currentSpeed * Time.deltaTime;
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