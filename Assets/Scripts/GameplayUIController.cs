using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Button pauseButton;
    [SerializeField] private MenuClickSound clickSound;

    [Header("Timing")]
    [SerializeField] private float levelDurationSeconds = 300f;

    private float remainingTime;
    private bool timeExpired;

    private void Start()
    {
        ResetForLevel();
        HookPauseButton();
    }

    private void Update()
    {
        UpdateTimer();
        UpdateStats();
    }

    private void ResetForLevel()
    {
        remainingTime = Mathf.Max(0f, levelDurationSeconds);
        timeExpired = false;
        UpdateStarsMax();
        UpdateTimeText();
    }

    private void UpdateTimer()
    {
        if (timeExpired || remainingTime <= 0f)
        {
            return;
        }

        remainingTime = Mathf.Max(0f, remainingTime - Time.deltaTime);
        UpdateTimeText();

        if (remainingTime <= 0f)
        {
            timeExpired = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private void UpdateStats()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (coinsText != null)
        {
            coinsText.text = GameManager.Instance.coins.ToString();
        }

        if (livesText != null)
        {
            int maxLives = GameManager.Instance.maxLives;
            livesText.text = $"{GameManager.Instance.lives}/{maxLives}";
        }
    }

    private void UpdateTimeText()
    {
        if (timeText == null)
        {
            return;
        }

        int totalSeconds = Mathf.CeilToInt(remainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateStarsMax()
    {
        int maxStars = GetMaxStarsForLevel();

        if (starImages == null)
        {
            return;
        }

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
            {
                starImages[i].gameObject.SetActive(i < maxStars);
            }
        }
    }

    private int GetMaxStarsForLevel()
    {
        int world = 0;
        int stage = 0;

        if (GameManager.Instance != null)
        {
            world = GameManager.Instance.world;
            stage = GameManager.Instance.stage;
        }
        else
        {
            string[] parts = SceneManager.GetActiveScene().name.Split('-');
            if (parts.Length == 2)
            {
                int.TryParse(parts[0], out world);
                int.TryParse(parts[1], out stage);
            }
        }

        return (world == 1 && stage == 1) ? 1 : 3;
    }

    private void HookPauseButton()
    {
        if (pauseButton == null)
        {
            return;
        }

        pauseButton.onClick.RemoveListener(OpenPauseMenu);
        pauseButton.onClick.AddListener(OpenPauseMenu);
    }

    public void OpenPauseMenu()
    {
        PlayClick();

        if (PauseMenuSceneLoader.Active != null)
        {
            PauseMenuSceneLoader.Active.OpenPauseMenu();
        }
    }

    private void PlayClick()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }
}
