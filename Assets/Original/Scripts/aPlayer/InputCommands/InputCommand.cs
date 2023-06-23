using UnityEngine;

/// <summary>
/// Should be created only by AbilitesInput.
/// </summary>
public class InputCommand
{ 
    public bool IsTriggered { get; set; } 
    public KeyCode TriggeringKeyCode { get; set; }

}