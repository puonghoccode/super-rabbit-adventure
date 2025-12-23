using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuStatusUI : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private Image[] lifeImages;
    [SerializeField] private Sprite lifeOnSprite;
    [SerializeField] private Sprite lifeOffSprite;

    [Header("Stars")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starOnSprite;
    [SerializeField] private Color starOnColor = Color.white;
    [SerializeField] private Color starOffColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        int lives = GameManager.Instance != null ? GameManager.Instance.lives : 0;
        int maxLives = GameManager.Instance != null ? GameManager.Instance.maxLives : lifeImages.Length;
        UpdateLives(lives, maxLives);

        int stars = GameManager.Instance != null ? GameManager.Instance.stars : 0;
        int maxStars = GetMaxStarsForLevel();
        stars = Mathf.Min(stars, maxStars);
        UpdateStars(stars, maxStars);
    }

    private void UpdateLives(int filled, int maxVisible)
    {
        if (lifeImages == null || lifeImages.Length == 0)
        {
            return;
        }

        int visibleCount = maxVisible > 0 ? Mathf.Min(lifeImages.Length, maxVisible) : lifeImages.Length;

        for (int i = 0; i < lifeImages.Length; i++)
        {
            Image image = lifeImages[i];
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

            if (lifeOnSprite != null && lifeOffSprite != null)
            {
                image.sprite = i < filled ? lifeOnSprite : lifeOffSprite;
            }
        }
    }

    private void UpdateStars(int filled, int maxVisible)
    {
        if (starImages == null || starImages.Length == 0)
        {
            return;
        }

        int visibleCount = maxVisible > 0 ? Mathf.Min(starImages.Length, maxVisible) : starImages.Length;

        for (int i = 0; i < starImages.Length; i++)
        {
            Image image = starImages[i];
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

            if (starOnSprite != null)
            {
                image.sprite = starOnSprite;
            }

            image.color = i < filled ? starOnColor : starOffColor;
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
}
