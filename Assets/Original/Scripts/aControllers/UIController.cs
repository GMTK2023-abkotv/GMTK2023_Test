using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Transform _indicatorsParent;

    GameObject _groundedIndicator;
    GameObject _sidesCollisionIndicator;
    GameObject _dashCooldownIndicator;

    TextMeshProUGUI _stateText;

    void Awake()
    {
        _groundedIndicator = _indicatorsParent.GetChild(0).gameObject;
        _sidesCollisionIndicator = _indicatorsParent.GetChild(1).gameObject;
        _dashCooldownIndicator = _indicatorsParent.GetChild(2).gameObject;
        _stateText = _indicatorsParent.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();

        UIDelegatesContainer.GetGroundedIndicator += GetGroundedIndicator;
        UIDelegatesContainer.GetSidesCollisionIndicator += GetSidesCollisionIndicator;
        UIDelegatesContainer.GetDashCoolDownIndicator += GetDashCooldownIndicator;

        UIDelegatesContainer.GetStateText += GetStateText;
    }

    void OnDestroy()
    {
        UIDelegatesContainer.GetGroundedIndicator -= GetGroundedIndicator;
        UIDelegatesContainer.GetSidesCollisionIndicator -= GetSidesCollisionIndicator;
        UIDelegatesContainer.GetDashCoolDownIndicator -= GetDashCooldownIndicator;

        UIDelegatesContainer.GetStateText -= GetStateText;
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

    TextMeshProUGUI GetStateText()
    {
        return _stateText;
    }
}