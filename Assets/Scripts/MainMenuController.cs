using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string levelSelectScene = "LevelSelect";
    [SerializeField] private string fallbackFirstLevelScene = "1-1";
    [SerializeField] private string rateUrl = "https://your.store.link.here";
    [SerializeField] private string joinUsUrl = "https://facebook.com/groups/yourgroup";
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private MenuClickSound clickSound;

    public void Play()
    {
        PlayClick();

        string targetScene = string.IsNullOrEmpty(levelSelectScene)
            ? fallbackFirstLevelScene
            : levelSelectScene;

        if (!string.IsNullOrEmpty(targetScene))
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

    public void Quit()
    {
        PlayClick();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void PlayClick()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }

    private void OpenUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }

        Application.OpenURL(url);
    }
}
