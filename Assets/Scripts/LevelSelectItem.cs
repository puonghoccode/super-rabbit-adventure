using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button button;
    [SerializeField] private Image lockIcon;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starOnSprite;
    [SerializeField] private Color starOnColor = Color.white;
    [SerializeField] private Color starOffColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    [SerializeField] private Image[] tintImages;
    [SerializeField] private TMP_Text[] tintTexts;
    [SerializeField] private Color lockedTint = new Color(0.6f, 0.6f, 0.6f, 1f);
    [SerializeField] private Color unlockedTint = Color.white;

    public void Configure(string label, int stars, int maxStars, bool unlocked)
    {
        if (titleText != null)
        {
            titleText.text = label;
        }

        if (button != null)
        {
            button.interactable = unlocked;
        }

        if (lockIcon != null)
        {
            lockIcon.gameObject.SetActive(!unlocked);
        }

        UpdateStars(stars, maxStars);
        ApplyTint(unlocked);
    }

    public void SetOnClick(UnityAction action)
    {
        if (button == null)
        {
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    private void UpdateStars(int stars, int maxStars)
    {
        if (starImages == null || starImages.Length == 0)
        {
            return;
        }

        int visibleCount = maxStars > 0 ? Mathf.Min(starImages.Length, maxStars) : starImages.Length;

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

            image.color = i < stars ? starOnColor : starOffColor;
        }
    }

    private void ApplyTint(bool unlocked)
    {
        Color tint = unlocked ? unlockedTint : lockedTint;

        if (tintImages != null)
        {
            for (int i = 0; i < tintImages.Length; i++)
            {
                if (tintImages[i] != null)
                {
                    tintImages[i].color = tint;
                }
            }
        }

        if (tintTexts != null)
        {
            for (int i = 0; i < tintTexts.Length; i++)
            {
                if (tintTexts[i] != null)
                {
                    tintTexts[i].color = tint;
                }
            }
        }
    }
}
