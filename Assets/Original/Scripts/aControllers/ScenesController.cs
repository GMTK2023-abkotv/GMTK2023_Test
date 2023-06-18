using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    int _sceneIndexToTest;
#endif

    int _currentSceneIndex;
    AsyncOperation _loadingScene;

    void Awake()
    {
        ApplicationDelegatesContainer.ShouldStartLoadingNextScene += StartLoadingNextScene;

        UIDelegatesContainer.EventExitToMainMenuPressed += LoadMainMenuScene;
        UIDelegatesContainer.EventContinueButtonPressed += FinishLoadingScene;

        UIDelegatesContainer.GetSceneLoadingProgress += GetSceneLoadingProgress;
        StartLoadingSavedScene();
    }

    void OnDestroy()
    {
        ApplicationDelegatesContainer.ShouldStartLoadingNextScene -= StartLoadingNextScene;

        UIDelegatesContainer.EventExitToMainMenuPressed -= LoadMainMenuScene;
        UIDelegatesContainer.EventContinueButtonPressed -= FinishLoadingScene;

        UIDelegatesContainer.GetSceneLoadingProgress -= GetSceneLoadingProgress;
    }

    void StartLoadingNextScene()
    { 
        _loadingScene = SceneManager.LoadSceneAsync(++_currentSceneIndex);
        ApplicationDelegatesContainer.EventStartedLoadingNextScene?.Invoke();
    }

    void StartLoadingSavedScene()
    { 
#if UNITY_EDITOR
        _loadingScene = SceneManager.LoadSceneAsync(_sceneIndexToTest);
        ApplicationDelegatesContainer.EventStartedLoadingNextScene?.Invoke();
        return;
#endif
        int _currentSceneIndex = PlayerPrefs.GetInt(PlayerPrefsContainer.LAST_SCENE_INDEX, -1);
        if (_currentSceneIndex < 0)
        {
            Debug.LogError("Incorrect player pref");
        }

        _loadingScene = SceneManager.LoadSceneAsync(_currentSceneIndex);
        ApplicationDelegatesContainer.EventStartedLoadingNextScene?.Invoke();
    }

    float GetSceneLoadingProgress()
    {
        return _loadingScene.progress;
    }

    void FinishLoadingScene()
    { 
        _loadingScene.allowSceneActivation = true;
    }

    void LoadMainMenuScene()
    {
        _currentSceneIndex = 0;
        SceneManager.LoadScene(_currentSceneIndex);
    }
}