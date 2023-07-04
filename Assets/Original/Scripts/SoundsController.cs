using UnityEngine;

public class SoundsController : MonoBehaviour
{
    [SerializeField]
    string walkSound;

    [SerializeField]
    string jumpSound;

    [SerializeField]
    string dashSound;

    MotionController motionController;

    void Awake()
    {
        TryGetComponent(out motionController);

        motionController.OnStartWalk += StartWalk;
        motionController.OnStopWalk += StopWalk;
        motionController.OnJump += Jump;
        motionController.OnDash += Dash;
    }

    void OnDestroy()
    {
        motionController.OnStartWalk -= StartWalk;
        motionController.OnStopWalk -= StopWalk;
        motionController.OnJump -= Jump;
        motionController.OnDash -= Dash;
    }

    void StartWalk()
    {
        // start walk sound
    }

    void StopWalk()
    {
        // stop walk sound
    }

    void Jump()
    { 

    }

    void Dash()
    { 

    }
}