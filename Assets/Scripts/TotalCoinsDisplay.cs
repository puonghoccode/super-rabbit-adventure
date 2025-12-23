using TMPro;
using UnityEngine;

public class TotalCoinsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private string prefix = "";

    private void OnEnable()
    {
        PlayerWallet.CoinsChanged += HandleCoinsChanged;
        UpdateText(PlayerWallet.Coins);
    }

    private void OnDisable()
    {
        PlayerWallet.CoinsChanged -= HandleCoinsChanged;
    }

    private void HandleCoinsChanged(int coins)
    {
        UpdateText(coins);
    }

    private void UpdateText(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = $"{prefix}{coins}";
        }
    }
}
