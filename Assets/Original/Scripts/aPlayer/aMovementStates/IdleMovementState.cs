using UnityEngine;
using Unity.Mathematics;

public class IdleMovementState : PlayerMovementState
{
    MoveCommand mMoveCommand;

    void Awake()
    {
        PlayerDelegatesContainer.GetInitialMovementState += GetThis;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.GetInitialMovementState -= GetThis;
    }

    IdleMovementState GetThis()
    {
        return this;
    }

    void Start()
    {
        mMoveCommand = InputDelegatesContainer.GetMoveCommand();
    }

    public override bool CheckForTransitions()
    {
        if (base.CheckForTransitions())
        {
            return true;
        }

        if (!PlayerDelegatesContainer.IsGrounded())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState.Invoke(PlayerMovementStateType.Falling);
            return true;
        }

        if (PlayerDelegatesContainer.IsGroundJumpTrigering())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState.Invoke(PlayerMovementStateType.GroundJumping);
            return true;
        }

        if (PlayerDelegatesContainer.IsDashTriggering())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState.Invoke(PlayerMovementStateType.Dash);
            return true;
        }

        if (mMoveCommand.IsTriggered)
        {
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Walking);
            return true;
        }
        return false;
    }

    public override void GetDisplacement(out float3 displacement)
    {
        displacement = float3.zero;
    }
}