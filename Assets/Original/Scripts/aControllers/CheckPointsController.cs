using UnityEngine;

public class CheckPointsController : MonoBehaviour
{
    [SerializeField]
    CheckPoint _initiallyActiveCheckPoint;

    CheckPoint _currentActiveCheckPoint;

    void Awake()
    {
#if UNITY_EDITOR
        if (_initiallyActiveCheckPoint == null)
        {
            Debug.LogError("Assign _initiallyActiveCheckPoint");
        }
#endif
        _currentActiveCheckPoint = _initiallyActiveCheckPoint;
        _currentActiveCheckPoint.ShowActivation();

        ApplicationDelegatesContainer.EventCheckPointEntered += OnCheckPointEntered;
        ApplicationDelegatesContainer.GetLastCheckPoint += GetAppropriateCheckPoint;
    }

    void OnDestroy()
    {
        ApplicationDelegatesContainer.EventCheckPointEntered -= OnCheckPointEntered;
        ApplicationDelegatesContainer.GetLastCheckPoint -= GetAppropriateCheckPoint;
    }

    void OnCheckPointEntered(CheckPoint checkPoint)
    {
        if (_currentActiveCheckPoint.GetInstanceID() == checkPoint.GetInstanceID())
        {
            return;
        }

        _currentActiveCheckPoint.ShowDeactivation();
        _currentActiveCheckPoint = checkPoint;
        _currentActiveCheckPoint.ShowActivation();
    }

    CheckPoint GetAppropriateCheckPoint()
    {
        return _currentActiveCheckPoint;
    }
}