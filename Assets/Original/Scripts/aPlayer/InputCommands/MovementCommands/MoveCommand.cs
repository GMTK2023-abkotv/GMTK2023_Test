using UnityEngine;
using Unity.Mathematics;

public class MoveCommand : InputCommand
{
    public KeyCode TriggeringKeyCodeToLeft;
    public KeyCode TriggeringKeyCodeToRight;

    public KeyCode TriggeringKeyCodeToUp;
    public KeyCode TriggeringKeyCodeToDown;

    public bool CommandingToLeft;
    public bool CommandingToRight;

    public bool CommandingToUp;
    public bool CommandingToDown;

    public float3 Direction;
}