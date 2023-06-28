using UnityEngine;
using Unity.Mathematics;

public class EnemyAI : MotionController
{
    Transform _player;
    Vector3 _lastPlayerPos;
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        _player = PlayerDelegatesContainer.GetTransform();
        _lastPlayerPos = _player.position;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 dir = math.normalize(_lastPlayerPos - transform.position);
        MoveCommand moveCommand = new() { Motion = MotionType.Walk, Direction = dir };
        OnMoveCommand(moveCommand);
        _lastPlayerPos = _player.position;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == Layers.Holes)
        {
            gameObject.SetActive(false);
        }
    }
}