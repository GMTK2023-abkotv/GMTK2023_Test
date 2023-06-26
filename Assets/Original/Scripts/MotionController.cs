using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MotionController : MonoBehaviour
{
    [SerializeField]
    protected float _dashForceAmount = 400;

    [SerializeField]
    protected float _jumpForceAmount = 400;

    [SerializeField]
    protected float _moveForceAmount = 4;

    Rigidbody _rigidBody;

    float _jumpCooldown = 1;
    [SerializeField]
    float _dashCooldown = 3;

    float _jumpCooldownTimer;
    float _dashCooldownTimer;

    MoveCommand _command;

    protected virtual void Awake()
    {
        TryGetComponent(out _rigidBody);
    }

    protected void OnMoveCommand(MoveCommand command)
    {
        switch (command.Motion)
        {
            case MotionType.Dash:
                if (_dashCooldownTimer < 0) _command = command;
                break;
            case MotionType.Jump:
                if (_jumpCooldownTimer < 0 &&
                    Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 1))
                {
                    _command = command;
                }
                break;
            case MotionType.Walk:
                _command = command;
                break;
        }
    }

    protected virtual void Update()
    {
        _dashCooldownTimer -= Time.deltaTime;
        _jumpCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        switch (_command.Motion)
        {
            case MotionType.Dash:
                _rigidBody.AddForce(_dashForceAmount * _command.Direction);
                _dashCooldownTimer = _dashCooldown;
                break;
            case MotionType.Jump:
                _rigidBody.AddForce(_jumpForceAmount * _command.Direction);
                _jumpCooldownTimer = _jumpCooldown;
                break;
            case MotionType.Walk:
                _rigidBody.AddForce(_moveForceAmount * _command.Direction);
                break;
        }
        _command.Motion = MotionType.Nihil;
    }
}