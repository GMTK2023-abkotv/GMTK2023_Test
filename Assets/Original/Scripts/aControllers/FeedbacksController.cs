using System.Collections;
using UnityEngine;

public class FeedbacksController : MonoBehaviour
{
    [SerializeField]
    Transform _particlesParent;

    [SerializeField]
    Transform _audioSourcesParent;

    [SerializeField]
    ParticleSystem _walkingStateParticles;

    [SerializeField]
    ParticleSystem _groundJumpParticles;

    Transform _playerTransformData;
    Transform _playerTransform
    {
        get
        {
            if (_playerTransformData == null)
            {
                _playerTransformData = PlayerDelegatesContainer.GetTransform();
            }
            return _playerTransformData;
        }
    }

    PlayerMovementStateType _previousMovementState;

    void Awake()
    {
        PlayerDelegatesContainer.EventFinalFrameMovementState += FinalFrameMovementState;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventFinalFrameMovementState -= FinalFrameMovementState;
    }

    void FinalFrameMovementState(PlayerMovementStateType newStateType)
    {
        StopPreviousFeedbackIfNeeded();
        switch (newStateType)
        {
            case PlayerMovementStateType.Walking:
                _walkingStateParticles.transform.SetParent(_playerTransform, false);
                _walkingStateParticles.Play();
                break;
            case PlayerMovementStateType.Jumping:
                _groundJumpParticles.transform.SetParent(_playerTransform, false);
                _groundJumpParticles.Play();
                break;
        }

        _previousMovementState = newStateType;
    }

    void StopPreviousFeedbackIfNeeded()
    {
        switch (_previousMovementState)
        {
            case PlayerMovementStateType.Walking:
                _walkingStateParticles.transform.SetParent(_particlesParent, false);
                _walkingStateParticles.Stop();
                return;
            case PlayerMovementStateType.Jumping:
                _groundJumpParticles.transform.SetParent(_particlesParent, false);
                return;
        }
    }
}