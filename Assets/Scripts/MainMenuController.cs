using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string levelSelectScene = "LevelSelect";
    [SerializeField] private string fallbackFirstLevelScene = "1-1";
    [SerializeField] private string rateUrl = "https://store.link";
    [SerializeField] private string joinUsUrl = "https://facebook.com/groups/";
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private MenuClickSound clickSound;
    [SerializeField] private bool forceBgmOn = true;
    [SerializeField] private bool forceClickOn = true;

    private void Start()
    {
        if (forceBgmOn)
        {
            GameAudioSettings.SetBgmEnabled(true);
        }

        if (forceClickOn)
        {
            GameAudioSettings.SetClickEnabled(true);
        }
    }

    public void Play()
    {
        PlayClick();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetStats();
        }

        string targetScene = string.IsNullOrEmpty(levelSelectScene)
            ? fallbackFirstLevelScene
            : levelSelectScene;

        if (!CanLoadScene(targetScene))
        {
            targetScene = fallbackFirstLevelScene;
        }

        if (CanLoadScene(targetScene))
        {
            SceneManager.LoadScene(targetScene);
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

    public void RateMyApp()
    {
        PlayClick();
        OpenUrl(rateUrl);
    }

    public void JoinUs()
    {
        PlayClick();
        OpenUrl(joinUsUrl);
    }

    private void PlayClick()
    {
        MenuClickSound sound = clickSound != null ? clickSound : MenuClickSound.Get();
        sound?.Play();
    }

    private void OpenUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }

        Application.OpenURL(url);
    }

    private bool CanLoadScene(string sceneName)
    {
        return !string.IsNullOrEmpty(sceneName) && Application.CanStreamedLevelBeLoaded(sceneName);
    }
}
