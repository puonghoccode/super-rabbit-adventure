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
    [SerializeField] private Image[] lifeImages;
    [SerializeField] private Sprite lifeOnSprite;
    [SerializeField] private Sprite lifeOffSprite;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starOnSprite;
    [SerializeField] private Color starOnColor = Color.white;
    [SerializeField] private Color starOffColor = new Color(0.5f, 0.5f, 0.5f, 1f);
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
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
        UpdateLifeIcons();
        UpdateStarIcons();
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

        UpdateLifeIcons();
        UpdateStarIcons();

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

    private void UpdateLifeIcons()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        int lives = GameManager.Instance.lives;
        int maxLives = GameManager.Instance.maxLives;
        UpdateIcons(lifeImages, lives, maxLives, lifeOnSprite, lifeOffSprite);
    }

    private void UpdateStarIcons()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        int maxStars = GetMaxStarsForLevel();
        int stars = GameManager.Instance.stars;
        stars = Mathf.Min(stars, maxStars);
        UpdateStarIcons(stars, maxStars);
    }

    private void UpdateIcons(Image[] images, int filled, int maxVisible, Sprite onSprite, Sprite offSprite)
    {
        if (images == null || images.Length == 0)
        {
            return;
        }

        int visibleCount = maxVisible > 0 ? Mathf.Min(images.Length, maxVisible) : images.Length;

        for (int i = 0; i < images.Length; i++)
        {
            Image image = images[i];
            if (image == null)
            {
                continue;
            }

            bool isVisible = i < visibleCount;
            image.gameObject.SetActive(isVisible);

            if (!isVisible)
            {
                continue;
            }

            if (onSprite != null && offSprite != null)
            {
                image.sprite = i < filled ? onSprite : offSprite;
            }
        }
    }

    private void UpdateStarIcons(int filled, int maxVisible)
    {
        if (starImages == null || starImages.Length == 0)
        {
            return;
        }

        int visibleCount = maxVisible > 0 ? Mathf.Min(starImages.Length, maxVisible) : starImages.Length;
        int showCount = Mathf.Min(filled, visibleCount);

        for (int i = 0; i < starImages.Length; i++)
        {
            Image image = starImages[i];
            if (image == null)
            {
                continue;
            }

            bool isVisible = i < showCount;
            image.gameObject.SetActive(isVisible);
            if (!isVisible)
            {
                continue;
            }

            if (starOnSprite != null)
            {
                image.sprite = starOnSprite;
            }

            image.color = starOnColor;
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

        return LevelProgress.GetMaxStars(world, stage);
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
        MenuClickSound sound = clickSound != null ? clickSound : MenuClickSound.Get();
        sound?.Play();
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsGameplayScene(scene.name))
        {
            ResetForLevel();
        }
    }

    private bool IsGameplayScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return false;
        }

        string[] parts = sceneName.Split('-');
        if (parts.Length != 2)
        {
            return false;
        }

        return int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _);
    }
}
