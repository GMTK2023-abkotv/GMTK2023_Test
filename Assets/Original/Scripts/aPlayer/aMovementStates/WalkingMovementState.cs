using UnityEngine;
using Unity.Mathematics;

public class WalkingMovementState : PlayerMovementState
{
    [SerializeField]
    float _acceleration = 3;
    [SerializeField]
    float _maxSpeed = 3;

    const float VelocityThreshold = 0.1f;

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

        _isAccelerating = _moveCommand.IsTriggered;

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

    public override void OnEntry()
    {
        _currentVelocity = float3.zero;
    }

    public override void GetDisplacement(out float3 displacement)
    {
        if (!_isAccelerating)
        {
            _currentVelocity -= math.normalize(_currentVelocity) * _acceleration * Time.deltaTime;
            if (math.lengthsq(_currentVelocity) < VelocityThreshold)
            {
                _currentVelocity = float3.zero;
            }
        }
        else
        { 
            _currentVelocity += _inputDirection * _acceleration * Time.deltaTime;
        }
        if (math.lengthsq(_currentVelocity) > _maxSpeed * _maxSpeed)
        {
            _currentVelocity = _inputDirection * _maxSpeed;
        }

        displacement = _currentVelocity * Time.deltaTime;
        displacement.y = -0.1f;
    }

    public override string ToString()
    {
        return "Walking";
    }
}