using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController _controller;

    GameObject _groundedIndicator;
    GameObject _sidesCollisionIndicator;

    bool _isGrounded;
    bool _ignoringGround;
    bool _isCollidedSides;

    void Awake()
    {
        TryGetComponent(out _controller);

        PlayerDelegatesContainer.GetPlayerController += GetThis;

        PlayerDelegatesContainer.IsGrounded += IsGrounded;
        PlayerDelegatesContainer.IsCollidingSides += IsCollidingSides;

        PlayerDelegatesContainer.EventGroundedIgnoranceStart += OnGroundedIgnoranceStart;
        PlayerDelegatesContainer.EventGroundedIgnoranceEnd += OnGroundedIgnoranceEnd;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.GetPlayerController -= GetThis;

        PlayerDelegatesContainer.IsGrounded -= IsGrounded;
        PlayerDelegatesContainer.IsCollidingSides -= IsCollidingSides;
        
        PlayerDelegatesContainer.EventGroundedIgnoranceStart -= OnGroundedIgnoranceStart;
        PlayerDelegatesContainer.EventGroundedIgnoranceEnd -= OnGroundedIgnoranceEnd;
    }

    void Start()
    {
        _groundedIndicator = UIDelegatesContainer.GetGroundedIndicator();
        _sidesCollisionIndicator = UIDelegatesContainer.GetSidesCollisionIndicator();
    }

    PlayerController GetThis()
    {
        return this;
    }

    bool IsGrounded()
    {
        return _isGrounded;
    }
    bool IsCollidingSides()
    {
        return _isCollidedSides;
    }

    void OnGroundedIgnoranceStart()
    {
        Debug.Log("ignoring ground");
        _ignoringGround = true;
        _isGrounded = false;
    }

    void OnGroundedIgnoranceEnd()
    {
        _ignoringGround = false;
    }

    public void ApplyDisplacement(float3 displacement)
    {
        CollisionFlags flags = _controller.Move(displacement);

        _isCollidedSides = flags == CollisionFlags.Sides;
        if (_sidesCollisionIndicator.activeSelf && !_isCollidedSides)
        {
            _sidesCollisionIndicator.SetActive(false);
        }
        else if (!_sidesCollisionIndicator.activeSelf && _isCollidedSides)
        {
            _sidesCollisionIndicator.SetActive(true);
        }

        if (!_ignoringGround)
        { 
            _isGrounded = flags == CollisionFlags.Below;
            if (_groundedIndicator.activeSelf && !_isGrounded)
            {
                _groundedIndicator.SetActive(false);
            }
            else if (!_groundedIndicator.activeSelf && _isGrounded)
            {
                _groundedIndicator.SetActive(true);
            }
        }
    }
}