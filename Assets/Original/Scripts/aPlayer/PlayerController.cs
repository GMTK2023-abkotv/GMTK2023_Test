using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController _controller;

    void Awake()
    {
        TryGetComponent(out _controller);

        PlayerDelegatesContainer.GetPlayerController += GetThis;

        PlayerDelegatesContainer.IsGrounded += IsGrounded;
        PlayerDelegatesContainer.IsCollidingAbove += IsCollidingAbove;
        PlayerDelegatesContainer.IsCollidingSides += IsCollidingSides;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.GetPlayerController -= GetThis;

        PlayerDelegatesContainer.IsGrounded -= IsGrounded;
        PlayerDelegatesContainer.IsCollidingAbove -= IsCollidingAbove;
        PlayerDelegatesContainer.IsCollidingSides -= IsCollidingSides;
    }

    PlayerController GetThis()
    {
        return this;
    }

    bool IsGrounded()
    {
        return _controller.isGrounded;
    }
    bool IsCollidingAbove()
    {
        return _controller.collisionFlags == CollisionFlags.Above;
    }
    bool IsCollidingSides()
    {
        return _controller.collisionFlags == CollisionFlags.Sides;
    }

    public void Move(float3 displacement)
    {
        _controller.Move(displacement);
    }
}