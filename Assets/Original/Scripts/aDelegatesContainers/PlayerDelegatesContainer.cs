using System;
using UnityEngine;
using Unity.Mathematics;

public static class PlayerDelegatesContainer
{
    public static Func<Transform> GetTransform;

    public static Action EventMove;
    public static Action EventJump;
    public static Action EventDash;

    #region LifeDeath
    public static Action<CheckPoint> EventSpawnStart;
    public static Action EventSpawnEnd;

    public static Action EventDeath;
    #endregion//LifeDeath

    #region TriggerEnters
    public static Action<Collider2D> EventPlayerOnTriggerEnter2D;
    public static Action<Collider2D> EventPlayerOnTriggerStay2D;
    public static Action<Collider2D> EventPlayerOnTriggerExit2D;
    #endregion
}