public class PlayerController : MotionController
{
    protected override void Awake()
    {
        base.Awake();
        PlayerDelegatesContainer.EventMoveCommand += OnMoveCommand;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventMoveCommand -= OnMoveCommand;
    }
}