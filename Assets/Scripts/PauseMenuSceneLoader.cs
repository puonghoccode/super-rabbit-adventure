using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuSceneLoader : MonoBehaviour
{
    public static PauseMenuSceneLoader Active { get; private set; }

    [SerializeField] private string pauseMenuScene = "PauseScene";
    [SerializeField] private bool pauseTimeOnOpen = true;

    private float previousTimeScale = 1f;
    private bool isOpen;

    private void Awake()
    {
        if (Active != null && Active != this)
        {
            Destroy(gameObject);
            return;
        }

        Active = this;
    }

    private void OnDestroy()
    {
        if (Active == this)
        {
            Active = null;
        }
    }

    public void OpenPauseMenu()
    {
        if (isOpen || string.IsNullOrEmpty(pauseMenuScene))
        {
            return;
        }

        if (pauseTimeOnOpen)
        {
            Pause();
        }

        SceneManager.LoadSceneAsync(pauseMenuScene, LoadSceneMode.Additive);
        isOpen = true;
    }

    public void ClosePauseMenu()
    {
        if (!isOpen)
        {
            return;
        }

        Resume();

        if (!string.IsNullOrEmpty(pauseMenuScene))
        {
            Scene scene = SceneManager.GetSceneByName(pauseMenuScene);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(pauseMenuScene);
            }
        }

        isOpen = false;
    }

    public void TogglePauseMenu()
    {
        if (isOpen)
        {
            ClosePauseMenu();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    public void ResumeTime()
    {
        Resume();
    }

    private void Pause()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        Time.timeScale = previousTimeScale;
    }
}
