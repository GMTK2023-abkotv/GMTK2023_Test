using System;

using UnityEngine;

using TMPro;

public static class UIDelegatesContainer
{
    public static Func<float> GetSceneLoadingProgress;
    public static Func<GameObject> GetGroundedIndicator;
    public static Func<GameObject> GetSidesCollisionIndicator;
    public static Func<GameObject> GetDashCoolDownIndicator;

    public static Func<TextMeshProUGUI> GetStateText;

    public static Action EscapePressed;
    public static Action EventSettingsEnter;
    public static Action EventSettingsExit;

    public static Action EventExitToMainMenuPressed;

    #region MainMenu
    public static Action EventContinueButtonPressed;
    #endregion

    public static Action<string> EventBuildLog;
}