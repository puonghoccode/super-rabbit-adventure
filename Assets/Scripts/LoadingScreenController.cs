using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private LoadingRunner runner;
    [SerializeField] private string fallbackScene = "MainMenu";

    private void Start()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        string targetScene = LoadingScreenData.TargetScene;
        if (string.IsNullOrEmpty(targetScene))
        {
            if (!string.IsNullOrEmpty(fallbackScene))
            {
                SceneManager.LoadScene(fallbackScene);
            }
            yield break;
        }

        if (runner != null)
        {
            runner.ResetRun();
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f)
        {
            yield return null;
        }

        while (runner != null && !runner.Completed)
        {
            yield return null;
        }

        LoadingScreenData.Clear();
        loadOperation.allowSceneActivation = true;
    }
}
