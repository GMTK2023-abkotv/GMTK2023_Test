using UnityEngine;

/// <summary>
/// Does not use TriggeringKeyCode;
/// </summary>
public class MoveCommand : InputCommand
{
    public KeyCode TriggeringKeyCodeToLeft  { get; set; }
    public KeyCode TriggeringKeyCodeToRight { get; set; }

    public KeyCode TriggeringKeyCodeToUp { get; set; }
    public KeyCode TriggeringKeyCodeToDown { get; set; }

    public bool CommandingToLeft  { get; set; }
    public bool CommandingToRight { get; set; }

    public bool CommandingToUp { get; set; }
    public bool CommandingToDown { get; set; }
}