using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuController : MonoBehaviour
{
    [Header("Outcome")]
    [SerializeField] private GameObject victoryRoot;
    [SerializeField] private GameObject gameOverRoot;

    [Header("Stats")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private Image[] starImages;

    [Header("Buttons")]
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private string levelSelectScene = "LevelSelect";
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private MenuClickSound clickSound;
    [SerializeField] private bool resetStatsOnMainMenu = true;

    private void Start()
    {
        ApplyOutcome();
        int earnedStars = ApplyStats();
        ConfigureButtons();

        if (EndMenuData.Outcome == EndMenuOutcome.Victory)
        {
            RecordProgress(earnedStars);
        }
    }

    private void ApplyOutcome()
    {
        bool victory = EndMenuData.Outcome == EndMenuOutcome.Victory;
        bool gameOver = EndMenuData.Outcome == EndMenuOutcome.GameOver;

        if (victoryRoot != null)
        {
            victoryRoot.SetActive(victory);
        }

        if (gameOverRoot != null)
        {
            gameOverRoot.SetActive(gameOver);
        }
    }

    private int ApplyStats()
    {
        int coins = EndMenuData.Coins;

        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }

        int stars = EndMenuData.Stars;
        int maxStars = GetMaxStarsForLevel();
        stars = Mathf.Min(stars, maxStars);

        if (starImages == null)
        {
            return stars;
        }

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
            {
                starImages[i].enabled = i < stars;
            }
        }

        return stars;
    }

    private void ConfigureButtons()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(EndMenuData.HasNextLevel);
        }
    }

    private int GetMaxStarsForLevel()
    {
        if (TryGetCurrentLevel(out int world, out int stage))
        {
            return LevelProgress.GetMaxStars(world, stage);
        }

        return 3;
    }

    private void RecordProgress(int stars)
    {
        if (TryGetCurrentLevel(out int world, out int stage))
        {
            LevelProgress.RecordCompletion(world, stage, stars);
        }
    }

    private bool TryGetCurrentLevel(out int world, out int stage)
    {
        world = EndMenuData.CurrentWorld;
        stage = EndMenuData.CurrentStage;

        if (world > 0 && stage > 0)
        {
            return true;
        }

        if (GameManager.Instance != null)
        {
            world = GameManager.Instance.world;
            stage = GameManager.Instance.stage;
            return true;
        }

        world = 0;
        stage = 0;
        return false;
    }

    public void NextLevel()
    {
        PlayClick();

        if (!EndMenuData.HasNextLevel)
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadLevel(EndMenuData.NextWorld, EndMenuData.NextStage);
        }
        else
        {
            SceneManager.LoadScene($"{EndMenuData.NextWorld}-{EndMenuData.NextStage}");
        }
    }

    public void GoToLevelSelect()
    {
        PlayClick();

        if (!string.IsNullOrEmpty(levelSelectScene))
        {
            SceneManager.LoadScene(levelSelectScene);
        }
    }

    public void GoToMainMenu()
    {
        PlayClick();

        if (resetStatsOnMainMenu && GameManager.Instance != null)
        {
            GameManager.Instance.ResetStats();
        }

        if (!string.IsNullOrEmpty(mainMenuScene))
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }

    private void PlayClick()
    {
        MenuClickSound sound = clickSound != null ? clickSound : MenuClickSound.Get();
        sound?.Play();
    }
}
