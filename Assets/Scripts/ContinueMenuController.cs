using TMPro;
using UnityEngine;

public class ContinueMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private int continueCost = 50;
    [SerializeField] private float durationSeconds = 30f;
    [SerializeField] private MenuClickSound clickSound;

    private float remaining;
    private bool expired;

    private void OnEnable()
    {
        remaining = Mathf.Max(0f, durationSeconds);
        expired = false;
        UpdateTimerText();
        UpdateCoinsText(PlayerWallet.Coins);
        UpdateCostText();
        PlayerWallet.CoinsChanged += UpdateCoinsText;
    }

    private void OnDisable()
    {
        PlayerWallet.CoinsChanged -= UpdateCoinsText;
    }

    private void Update()
    {
        if (expired)
        {
            return;
        }

        remaining = Mathf.Max(0f, remaining - Time.unscaledDeltaTime);
        UpdateTimerText();

        if (remaining <= 0f)
        {
            expired = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SkipContinue();
            }
        }
    }

    public void BuyContinue()
    {
        PlayClick();

        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.TryContinueWithCoins(continueCost);
    }

    public void WatchAdContinue()
    {
        PlayClick();
        GameManager.Instance?.ContinueWithAd();
    }

    public void Skip()
    {
        PlayClick();
        GameManager.Instance?.SkipContinue();
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
        {
            return;
        }

        int seconds = Mathf.CeilToInt(remaining);
        timerText.text = seconds.ToString();
    }

    private void UpdateCoinsText(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
    }

    private void UpdateCostText()
    {
        if (costText != null)
        {
            costText.text = continueCost.ToString();
        }
    }

    private void PlayClick()
    {
        MenuClickSound sound = clickSound != null ? clickSound : MenuClickSound.Get();
        sound?.Play();
    }
}
