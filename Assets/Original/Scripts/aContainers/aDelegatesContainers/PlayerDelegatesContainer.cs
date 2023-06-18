using System;
using UnityEngine;

public static class PlayerDelegatesContainer
{
    public static Func<bool> IsGrounded;
    public static Func<bool> IsCollidingAbove;
    public static Func<bool> IsCollidingSides;
    public static Func<bool> IsDashTriggering;
    public static Func<bool> IsGroundJumpTrigering;
    public static Func<bool> IsCoyoteGroundJumpTriggering;
    public static Func<bool> IsBufferGroundJumpTriggering;

    public static Func<float> GetTimeSinceLastGrounded;
    public static Func<float> GetSlopeAngleBelowRaycast;
    public static Func<float> GetFallingGravityAmount;
    public static Func<float> GetSlopeAngleMinThreshold;
    public static Func<float> GetSlopeAngleMaxThreshold;
    public static Func<float> GetFallSpeed;

    public static Func<Transform> GetTransform;
    public static Func<PlayerMovementStateType> GetPreviousStateType;
    public static Func<PlayerCharacter> GetPlayerCharacter;
    public static Func<PlayerController> GetPlayerController;
    public static Func<PlayerMovementState> GetInitialMovementState;


    public static Action<PlayerMovementStateType> EventEntryNewMovementState;
    public static Action<PlayerMovementStateType> EventFinalFrameMovementState;

    #region LifeDeath
    public static Action<CheckPoint> EventSpawnStart;
    public static Action EventSpawnEnd;

    public static Action EventDeath;
    #endregion//LifeDeath

    #region PlayerMovementStateNeeded
    public static Action EventGroundedIgnoranceStart;
    public static Action EventGroundedIgnoranceEnd;

    public static Action EventJustGotGrounded;

    public static Action EventInputBufferShouldReset;
    #endregion//PlayerMovementStateNeeded

    #region TriggerEnters
    public static Action<Collider2D> EventPlayerOnTriggerEnter2D;
    public static Action<Collider2D> EventPlayerOnTriggerStay2D;
    public static Action<Collider2D> EventPlayerOnTriggerExit2D;
    #endregion
}