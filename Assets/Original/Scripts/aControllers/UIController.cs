using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Transform _indicatorsParent;

    GameObject _groundedIndicator;
    GameObject _sidesCollisionIndicator;
    GameObject _dashCooldownIndicator;
    
    void Awake()
    {
        _groundedIndicator = _indicatorsParent.GetChild(0).gameObject;
        _sidesCollisionIndicator = _indicatorsParent.GetChild(1).gameObject;
        _dashCooldownIndicator = _indicatorsParent.GetChild(2).gameObject;

        UIDelegatesContainer.GetGroundedIndicator += GetGroundedIndicator;
        UIDelegatesContainer.GetSidesCollisionIndicator += GetSidesCollisionIndicator;
        UIDelegatesContainer.GetDashCoolDownIndicator += GetDashCooldownIndicator;
    }

    void OnDestroy()
    {
        UIDelegatesContainer.GetGroundedIndicator -= GetGroundedIndicator;
        UIDelegatesContainer.GetSidesCollisionIndicator -= GetSidesCollisionIndicator;
        UIDelegatesContainer.GetDashCoolDownIndicator -= GetDashCooldownIndicator;
    }

    GameObject GetGroundedIndicator()
    {
        return _groundedIndicator;
    }

    GameObject GetSidesCollisionIndicator()
    {
        return _sidesCollisionIndicator;
    }

    GameObject GetDashCooldownIndicator()
    {
        return _dashCooldownIndicator;
    }
}