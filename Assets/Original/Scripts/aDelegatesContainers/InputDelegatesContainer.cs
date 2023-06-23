using System;

public static class InputDelegatesContainer
{
    public static Func<MoveCommand> GetMoveCommand;
    public static Func<JumpCommand> GetJumpCommand;
    public static Func<DashCommand> GetDashCommand;
}