using UnityEngine;
using Unity.Mathematics;

/// <summary>
/// It is tied to the falling movementState.
/// </summary>
public class FallingMovementState : PlayerMovementState
{
    [SerializeField]
    float _fallingGravityAmount;

    [SerializeField]
    float _maxVerticalSpeed = 20;

    bool _isFalling;
    float _currentVerticalSpeed;

    float _minSlopeAngleForSliding;
    float _maxSlopeAngleForSliding;

    MoveCommand mMoveCommand;

    void Awake()
    {
        PlayerDelegatesContainer.GetFallingGravityAmount += GetFallingGravityAmount;
    }

    protected void OnDestroy()
    {
        PlayerDelegatesContainer.GetFallingGravityAmount -= GetFallingGravityAmount;
    }

    void Start()
    {
        // _minSlopeAngleForSliding = PlayerDelegatesContainer.GetSlopeAngleMinThreshold();
        // _maxSlopeAngleForSliding = PlayerDelegatesContainer.GetSlopeAngleMaxThreshold();

        mMoveCommand = InputDelegatesContainer.GetMoveCommand();
    }

    float GetFallingGravityAmount()
    {
        return _fallingGravityAmount;
    }

    float GetFallSpeed()
    {
        return _currentVerticalSpeed;
    }

    public override void OnEntry()
    {
        _currentVerticalSpeed = 0;
    }

    public override bool CheckForTransitions()
    {
        if (base.CheckForTransitions())
        {
            return true;
        }

        if (PlayerDelegatesContainer.IsBufferGroundJumpTriggering())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.GroundJumping);
            return true;
        }

        if (PlayerDelegatesContainer.IsGrounded())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.Idle);
            return true;
        }

        if (PlayerDelegatesContainer.IsCoyoteGroundJumpTriggering())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.GroundJumping);
            return true;
        }

        if (PlayerDelegatesContainer.IsDashTriggering())
        { 
            PlayerDelegatesContainer.EventEntryNewMovementState(PlayerMovementStateType.Dash);
            return true;
        }

        return false;
    }

    public override void OnTransition()
    {
        PlayerDelegatesContainer.EventInputBufferShouldReset?.Invoke();
    }

    public override void GetDisplacement(out float3 displacement)
    {
        _currentVerticalSpeed += _fallingGravityAmount * Time.deltaTime;
        _currentVerticalSpeed = Mathf.Clamp(_currentVerticalSpeed, 0, _maxVerticalSpeed);

        displacement = float3.zero;
        displacement.y = -_currentVerticalSpeed * Time.deltaTime; //_gravityFromMovingPlatforms
    }

    public override string ToString()
    {
        return "Gravity Fall";
    }
}