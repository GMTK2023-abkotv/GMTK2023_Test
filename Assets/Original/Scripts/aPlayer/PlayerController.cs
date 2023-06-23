using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _dashForceAmount;

    [SerializeField]
    float _jumpForceAmount;

    [SerializeField]
    float _moveForceAmount;

    [SerializeField]
    float _dashCooldown;

    float _jumpCooldown = 1;

    Rigidbody _rigidBody;

    MoveCommand _moveCommand;
    JumpCommand _jumpCommand;
    DashCommand _dashCommand;

    float3 _moveDirection;

    float _jumpCooldownTimer;
    float _dashCooldownTimer;

    bool _isDashTriggered;
    bool _isJumpTriggered;
    bool _isMoveTriggered;

    void Awake()
    {
        TryGetComponent(out _rigidBody);
    }

    void OnDestroy()
    {
    }

    void Start()
    {
        _moveCommand = InputDelegatesContainer.GetMoveCommand();
        _jumpCommand = InputDelegatesContainer.GetJumpCommand();
        _dashCommand = InputDelegatesContainer.GetDashCommand();
    }

    void Update()
    {
        _dashCooldownTimer -= Time.deltaTime;
        _jumpCooldownTimer -= Time.deltaTime;

        if (_dashCommand.IsTriggered && _dashCooldownTimer < 0)
        {
            _isDashTriggered = true;
            return;
        }

        if (_jumpCommand.IsTriggered)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 1))
            {
                if (_jumpCooldownTimer < 0)
                { 
                    _isJumpTriggered = true;
                }
            }
            return;
        }

        if (_moveCommand.IsTriggered)
        {
            _moveDirection = _moveCommand.Direction;
            _isMoveTriggered = true;
        }
    }

    void FixedUpdate()
    {
        if (_isMoveTriggered && _isDashTriggered)
        { 
            ApplyForce(_dashForceAmount * _moveDirection);
            _isMoveTriggered = false;
            _isDashTriggered = false;
            _isJumpTriggered = false;

            _dashCooldownTimer = _dashCooldown;
            return;
        }

        if (_isJumpTriggered)
        { 
            ApplyForce(_jumpForceAmount * math.up());
            _isJumpTriggered = false;

            _jumpCooldownTimer = _jumpCooldown;
            return;
        }

        if (_isMoveTriggered)
        {
            ApplyForce(_moveForceAmount * _moveDirection);
            _isMoveTriggered = false;
        }
    }

    public void ApplyForce(float3 force)
    {
        _rigidBody.AddForce(force);
    }
}