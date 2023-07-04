using UnityEngine;

public class SoundsController : MonoBehaviour
{
    [SerializeField]
    AK.Wwise.Event Roll;

    [SerializeField]
    string jumpSound;

    [SerializeField]
    AK.Wwise.Event Dash;

    MotionController motionController;

    void Awake()
    {
        TryGetComponent(out motionController);

        motionController.OnStartWalk += StartWalk;
        motionController.OnStopWalk += StopWalk;
        motionController.OnJump += Jump;
        motionController.OnDash += PlayDash;
    }

    void OnDestroy()
    {
        motionController.OnStartWalk -= StartWalk;
        motionController.OnStopWalk -= StopWalk;
        motionController.OnJump -= Jump;
        motionController.OnDash -= PlayDash;
    }

    void StartWalk()
    {
        Roll.Post(gameObject);
    }

    void StopWalk()
    {
   
    }

    void Jump()
    { 

    }

    void PlayDash()
    {
        Dash.Post(gameObject);
    }
}