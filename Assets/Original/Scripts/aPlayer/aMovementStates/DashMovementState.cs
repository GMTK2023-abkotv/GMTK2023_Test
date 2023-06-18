using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class DashMovementState : PlayerMovementState
{
    [SerializeField]
    float _distanceOfDash;

    [SerializeField]
    float _dashSpeed;

    [SerializeField]
    float _dashCoolDown = 2;

    GameObject _coolDownIndicator;

    float _movedDistance;
    bool _isDashExhausted;

    DashCommand _dashCommandReference;

    bool _isInCoolDown;
    int _enteredDashThroughZonesCount;

    void Awake()
    {
        _enteredDashThroughZonesCount = 0;

        PlayerDelegatesContainer.EventJustGotGrounded += OnJustGotGrounded;
        PlayerDelegatesContainer.EventPlayerOnTriggerEnter2D += OnPlayerTriggerEnter2D;
        PlayerDelegatesContainer.EventPlayerOnTriggerExit2D += OnPlayerTriggerExit2D;

        PlayerDelegatesContainer.IsDashTriggering += IsDashTriggering;
    }

    void Start()
    {
        _dashCommandReference = InputDelegatesContainer.GetDashCommand();
        // _coolDownIndicator = UIDelegatesContainer.GetDashCoolDownIndicator();
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventJustGotGrounded -= OnJustGotGrounded;
        PlayerDelegatesContainer.EventPlayerOnTriggerEnter2D -= OnPlayerTriggerEnter2D;
        PlayerDelegatesContainer.EventPlayerOnTriggerExit2D -= OnPlayerTriggerExit2D;

        PlayerDelegatesContainer.IsDashTriggering -= IsDashTriggering;
    }

    void OnJustGotGrounded()
    {
        if (!_isInCoolDown && _isDashExhausted)
        {
            StartCoroutine(DashCoolDown());
        }
    }

    void OnPlayerTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayersContainer.DASH_THROUGH_LAYER)
        {
            _enteredDashThroughZonesCount++;
        }
    }

    void OnPlayerTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayersContainer.DASH_THROUGH_LAYER)
        { 
            _enteredDashThroughZonesCount--;
        }
    }

    public override void OnEntry()
    {
        PlayerDelegatesContainer.EventGroundedIgnoranceStart?.Invoke();
        _coolDownIndicator?.SetActive(false);
        if (!_isInCoolDown)
        {
            StartCoroutine(DashCoolDown());
        }
        _movedDistance = 0;
        _isDashExhausted = true;
    }

    public override bool CheckForTransitions()
    {
        if (base.CheckForTransitions())
        {
            return true;
        }

        if (IsCollidingInFacingDirection())
        {
            // if (!PlayerQueriesContainer.QueryIsCollidingWithDashThroughZone(direction))
            // { 
                EndDashState();
                return true;
            // }
        }

        if (_enteredDashThroughZonesCount == 0 && _movedDistance > _distanceOfDash)
        { 
            EndDashState();
            return true;
        }

        return false;
    }

    void EndDashState()
    { 
        if (PlayerDelegatesContainer.IsGrounded())
        {
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Idle);
        }
        else
        {
            PlayerDelegatesContainer.EventEntryNewMovementState?.Invoke(PlayerMovementStateType.Falling);
        }
    }

    public override void OnTransition()
    {
        PlayerDelegatesContainer.EventGroundedIgnoranceEnd?.Invoke();
    }

    public override void GetDisplacement(out float3 displacement)
    {
        displacement = float3.zero;
        displacement.x = _dashSpeed * Time.deltaTime;
        _movedDistance += displacement.x;
    }

    bool IsDashTriggering()
    {
        if (_isDashExhausted)
        {
            return false;
        }

        if (!_dashCommandReference.IsTriggered)
        {
            return false;
        }

        if (IsCollidingInFacingDirection())
        {
            // if (!PlayerQueriesContainer.QueryIsCollidingWithDashThroughZone(direction))
            // { 
                return false;
            // }
        }
        return true;
    }

    bool IsCollidingInFacingDirection()
    {
        return false;
            // _playerCharacter.GetFacingDirection() == PlayerCharacter.FacingDirectionType.Left &&
            // PlayerQueriesContainer.QueryIsCollidingToTheLeft()
            // ||
            // _playerCharacter.GetFacingDirection() == PlayerCharacter.FacingDirectionType.Right &&
            // PlayerQueriesContainer.QueryIsCollidingToTheRight();
    }

    IEnumerator DashCoolDown()
    {
        _isInCoolDown = true;
        yield return new WaitForSeconds(_dashCoolDown);
        _isInCoolDown = false;
        _isDashExhausted = false;
        _coolDownIndicator?.SetActive(true);
    }
}