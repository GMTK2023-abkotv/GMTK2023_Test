using System;

/// <summary>
/// Portal events are not included here
/// </summary>
public static class ApplicationDelegatesContainer
{
    public static Action GameStart;
    public static Action GameEnd;

    public static Action<CheckPoint> EventCheckPointEntered;

    public static Action ShouldStartLoadingNextScene;
    public static Action EventStartedLoadingNextScene;

    public static Func<CheckPoint> GetLastCheckPoint;
}