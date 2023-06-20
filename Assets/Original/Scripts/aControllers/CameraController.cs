using System.Collections;

using UnityEngine;

using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera _renderingCamera;

    [SerializeField]
    CinemachineVirtualCamera _vcam;

    [SerializeField]
    float _targetChangeSpeed;


    CinemachineFramingTransposer _framingTransposer;

    float _defaultDampX;
    float _defaultDampY;

    BoxCollider2D _boundingBox;

    IEnumerator _lookingCoroutine;

    void Awake()
    {
        _framingTransposer = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        _defaultDampX = _framingTransposer.m_XDamping;
        _defaultDampY = _framingTransposer.m_YDamping;

        PlayerDelegatesContainer.EventSpawnStart += OnPlayerSpawnStart;
        PlayerDelegatesContainer.EventSpawnEnd   += OnPlayerSpawnEnd;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventSpawnStart -= OnPlayerSpawnStart;
        PlayerDelegatesContainer.EventSpawnEnd   -= OnPlayerSpawnEnd;
    }

    void Start()
    {
        float prevDampX = _framingTransposer.m_XDamping;
        _framingTransposer.m_XDamping = 0;
        float prevDampY = _framingTransposer.m_YDamping;
        _framingTransposer.m_YDamping = 0;
        _framingTransposer.m_XDamping = prevDampX;
        _framingTransposer.m_YDamping = prevDampY;

    }

    void OnPlayerSpawnStart(CheckPoint t)
    { 
        _framingTransposer.m_XDamping = 0;
        _framingTransposer.m_YDamping = 0;
    }

    void OnPlayerSpawnEnd()
    { 
        _framingTransposer.m_XDamping = _defaultDampX;
        _framingTransposer.m_YDamping = _defaultDampX;
    }
}
