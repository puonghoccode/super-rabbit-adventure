using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private string gameSelectScene = "GameSelect";
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private MenuClickSound clickSound;
    [SerializeField] private bool pauseTimeIfNoLoader = true;

    private float previousTimeScale = 1f;

    private void OnEnable()
    {
        if (pauseTimeIfNoLoader && PauseMenuSceneLoader.Active == null)
        {
            Pause();
        }
    }

    private void OnDisable()
    {
        if (pauseTimeIfNoLoader && PauseMenuSceneLoader.Active == null)
        {
            Resume();
        }
    }

    public void ContinueGame()
    {
        PlayClick();
        ClosePauseMenu();
    }

    public void RestartGame()
    {
        PlayClick();
        ResumeForSceneChange();

        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.LoadLevel(GameManager.Instance.world, GameManager.Instance.stage, true);
    }

    public void QuitToGameSelect()
    {
        PlayClick();
        ResumeForSceneChange();

        if (!string.IsNullOrEmpty(gameSelectScene))
        {
            SceneManager.LoadScene(gameSelectScene);
        }
    }

    public void OpenSettings()
    {
        PlayClick();

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        PlayClick();

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        PlayClick();

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
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

    private void PlayClick()
    {
        MenuClickSound sound = clickSound != null ? clickSound : MenuClickSound.Get();
        sound?.Play();
    }

    private void ClosePauseMenu()
    {
        PauseMenuSceneLoader loader = PauseMenuSceneLoader.Active;
        if (loader != null)
        {
            loader.ClosePauseMenu();
            return;
        }

        if (pauseTimeIfNoLoader)
        {
            Resume();
        }

        gameObject.SetActive(false);
    }

    private void ResumeForSceneChange()
    {
        PauseMenuSceneLoader loader = PauseMenuSceneLoader.Active;
        if (loader != null)
        {
            loader.ResumeTime();
            return;
        }

        if (pauseTimeIfNoLoader)
        {
            Resume();
        }
    }
}
