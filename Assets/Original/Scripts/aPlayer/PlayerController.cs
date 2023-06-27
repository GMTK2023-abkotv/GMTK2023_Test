using UnityEngine;

public class PlayerController : MotionController
{
    [SerializeField]
    Transform start;

    protected override void Awake()
    {
        base.Awake();
        PlayerDelegatesContainer.EventPlayerAlive += OnAlive;
        PlayerDelegatesContainer.EventPlayerDead += OnDead;
        PlayerDelegatesContainer.GetTransform += GetTransform;
    }

    void OnAlive()
    {
        PlayerDelegatesContainer.EventMoveCommand += OnMoveCommand;

        transform.position = start.position;
        ActivateRigidbody();
    }

    void OnDead()
    {
        PlayerDelegatesContainer.EventMoveCommand -= OnMoveCommand;
        DeactivateRigidbody();
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventPlayerAlive -= OnAlive;
        PlayerDelegatesContainer.EventPlayerDead -= OnDead;
        PlayerDelegatesContainer.GetTransform -= GetTransform;
    }

    void DeactivateRigidbody()
    {
        // https://forum.unity.com/threads/reset-rigidbody.39998/
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = true;
    }

    void ActivateRigidbody()
    {
        // to fix that prob where changing iskinematic can make the balls appear to drag, deactivate then
        // activate the ball which seems to fix it
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        _rigidBody.useGravity = true;
        _rigidBody.isKinematic = false;
    }

    Transform GetTransform()
    {
        return transform;
    }
}