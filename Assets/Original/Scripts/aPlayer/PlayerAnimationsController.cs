using System.Collections;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    [SerializeField]
    Animator _humAnimator;

    const string STATE_NAME_IDLE = "Idle";
    const string STATE_NAME_RUN  = "Walking";
    const string STATE_NAME_GROUND_JUMP = "Jumping";
    const string STATE_NAME_FALLING = "Falling";
    const string STATE_NAME_DASHING = "Dashing";

    const float FADE_DURATION = 0.05f;
    
    string _previousAnimationStateParameterName;
    PlayerMovementStateType _previousState;

    bool _shouldUpdateAnimatorThisFrame;
    
    string _stateToTransitionTo;
    string _afterTransitionState;

    IEnumerator _waitCoroutine;

    void Awake()
    {
        PlayerDelegatesContainer.EventFinalFrameMovementState += OnFinalFrameMovementState;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventFinalFrameMovementState -= OnFinalFrameMovementState;
    }

    void OnFinalFrameMovementState(PlayerMovementStateType newMovementState)
    {
        _shouldUpdateAnimatorThisFrame = true;

        switch (newMovementState)
        {
            case PlayerMovementStateType.Idle:
                _stateToTransitionTo = STATE_NAME_IDLE;
                break;
            case PlayerMovementStateType.Walking:
                _stateToTransitionTo = STATE_NAME_RUN;
                break;
            case PlayerMovementStateType.GroundJumping:
                _stateToTransitionTo = STATE_NAME_GROUND_JUMP;
                break;
            case PlayerMovementStateType.Falling:
                _stateToTransitionTo = STATE_NAME_FALLING;
                break;
            case PlayerMovementStateType.Dash:
                _stateToTransitionTo = STATE_NAME_DASHING;
                break;
        }

        _previousState = newMovementState;
    }

    void LateUpdate() 
    {
        if (_shouldUpdateAnimatorThisFrame)
        {
            _humAnimator.CrossFade(_stateToTransitionTo, FADE_DURATION);
            _shouldUpdateAnimatorThisFrame = false;
        }
    }

    void Log(string msg)
    {
        Debug.Log("PlayerAnimationsController: " + msg);
    }
}