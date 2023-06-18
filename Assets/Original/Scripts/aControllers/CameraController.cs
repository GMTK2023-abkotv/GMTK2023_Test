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
    float _offsetInFacingDirection = 5;

    [SerializeField]
    float _lookUpYOffset = 4;

    [SerializeField]
    float _defaultYOffset = 1;

    [SerializeField]
    float _lookDownYOffset = -2;

    [SerializeField]
    [Tooltip("Should not be modified")]
    float _defaultXOffset = 0;

    [SerializeField]
    float _targetChangeSpeed;


    CinemachineFramingTransposer _framingTransposer;

    float _defaultDampX;
    float _defaultDampY;

    Vector2 _currentLookPos;
    Vector2 _desiredLookPos;
    Vector2 _targetLookPos;

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
        _currentLookPos = new Vector2(_defaultXOffset, _defaultYOffset);
        _desiredLookPos.y = _defaultYOffset;

        float prevDampX = _framingTransposer.m_XDamping;
        _framingTransposer.m_XDamping = 0;
        float prevDampY = _framingTransposer.m_YDamping;
        _framingTransposer.m_YDamping = 0;
        _framingTransposer.m_XDamping = prevDampX;
        _framingTransposer.m_YDamping = prevDampY;

    }

    void LateUpdate()
    {
        _targetLookPos = _desiredLookPos;

        float deltaScalarMax = _targetChangeSpeed * Time.deltaTime;
        _currentLookPos = Vector3.MoveTowards(_currentLookPos, _targetLookPos, deltaScalarMax);
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
