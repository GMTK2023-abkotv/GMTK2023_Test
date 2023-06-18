using System.Collections;

using UnityEngine;
using Unity.Mathematics;

// TODO: add coyote time and input buffering
public class JumpGroundMovementState : PlayerMovementState
{
    [SerializeField]
    float _jumpHeight;

    [SerializeField]
    [Tooltip("Set to zero if not air control needed")]
    float _airHorizontalMoveSpeed;

    [SerializeField]
    float OnEntryGroundedIgnoranceTime = 0.3f;

    [Header("Quality of life")]
    [SerializeField]
    float _coyoteTime;

    [SerializeField]
    float _inputBufferingDuration;

    bool _isJumping;

    const float TIME_IS_GROUNDED_IGNORANCE_AFTER_JUMP_START = 0.2f;

    float _fallingGravityAmount;
    float _currentSpeed;

    float _groundedIgnoranceTimer;

    JumpCommand _jumpCommand;
    MoveCommand _moveCommand;

    float _jumpTriggerBufferTimer = -1;

    void Awake()
    {
        PlayerDelegatesContainer.EventInputBufferShouldReset += OnInputBufferShouldReset;

        PlayerDelegatesContainer.IsGroundJumpTrigering += IsGroundJumpTriggering;
        PlayerDelegatesContainer.IsCoyoteGroundJumpTriggering += IsCoyoteGroundJumpTriggering;
        PlayerDelegatesContainer.IsBufferGroundJumpTriggering += IsBufferGroundJumpTriggering;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventInputBufferShouldReset -= OnInputBufferShouldReset;

        PlayerDelegatesContainer.IsGroundJumpTrigering -= IsGroundJumpTriggering;
        PlayerDelegatesContainer.IsCoyoteGroundJumpTriggering -= IsCoyoteGroundJumpTriggering;
        PlayerDelegatesContainer.IsBufferGroundJumpTriggering -= IsBufferGroundJumpTriggering;
    }

    void Start()
    {
        _fallingGravityAmount = PlayerDelegatesContainer.GetFallingGravityAmount();
        
        _jumpCommand = InputDelegatesContainer.GetJumpCommand();
        _moveCommand = InputDelegatesContainer.GetMoveCommand();
    }

    public override bool CheckForTransitions()
    {
        if (base.CheckForTransitions())
        {
            return true;
        }

        if (_groundedIgnoranceTimer > TIME_IS_GROUNDED_IGNORANCE_AFTER_JUMP_START)
        { 
            if ( PlayerDelegatesContainer.IsGrounded())
            {
                PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Idle);
                return true;
            }
        }
        else
        {
            _groundedIgnoranceTimer += Time.deltaTime;
        }

        if (PlayerDelegatesContainer.IsDashTriggering())
        { 
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Dash);
            return true;
        }

        if ( PlayerDelegatesContainer.IsCollidingAbove() || _currentSpeed < 0)
        {
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Falling);
            return true;
        }

        return false;
    }

    public override void OnEntry()
    {
        _jumpCommand.IsConsumed = true;

        _groundedIgnoranceTimer = 0;
        _currentSpeed = Mathf.Sqrt(2 * _jumpHeight * _fallingGravityAmount);

        StartCoroutine(GroundedIgnoranceTime());
    }

    IEnumerator GroundedIgnoranceTime()
    {
        yield return null;
        PlayerDelegatesContainer.EventGroundedIgnoranceStart?.Invoke();
        yield return new WaitForSeconds(OnEntryGroundedIgnoranceTime);
        PlayerDelegatesContainer.EventGroundedIgnoranceEnd?.Invoke();
    }

    public override void GetDisplacement(out float3 displacement)
    {
        displacement = float3.zero;
        displacement.y = _currentSpeed * Time.deltaTime;

        _currentSpeed -= _fallingGravityAmount * Time.deltaTime;
    }

    bool IsGroundJumpTriggering()
    {
        if (!_jumpCommand.IsTriggered)
        {
            return false;
        }

        if (PlayerDelegatesContainer.IsCollidingAbove() || 
            !PlayerDelegatesContainer.IsGrounded())
        {
            return false;
        }

        return true;
    }

    bool IsCoyoteGroundJumpTriggering()
    {
        if (!_jumpCommand.IsTriggered)
        {
            return false;
        }

        if (PlayerDelegatesContainer.IsCollidingAbove())
        {
            return false;
        }

        if (PlayerDelegatesContainer.GetPreviousStateType() != PlayerMovementStateType.Walking)
        {
            return false;
        }
        if (PlayerDelegatesContainer.GetTimeSinceLastGrounded() < _coyoteTime)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Should be updated each frame to work.
    /// </summary>
    bool IsBufferGroundJumpTriggering()
    {
        if (_jumpTriggerBufferTimer < 0 || _jumpTriggerBufferTimer > _inputBufferingDuration)
        { 
            if (_jumpCommand.IsTriggered)
            { 
                _jumpTriggerBufferTimer = 0;
            }
            return false;
        }

        if (PlayerDelegatesContainer.IsGrounded())
        {
            _jumpTriggerBufferTimer = -1;
            return true;
        }

        _jumpTriggerBufferTimer += Time.deltaTime;
        return false;
    }

    void OnInputBufferShouldReset()
    {
        _jumpTriggerBufferTimer = -1;
    }

    public override string ToString()
    {
        return "GroundJump ab";
    }
}