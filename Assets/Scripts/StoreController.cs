using UnityEngine;

public class StoreController : MonoBehaviour
{
    [SerializeField] private MenuClickSound clickSound;

    public void Open()
    {
        PlayClick();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        PlayClick();
        gameObject.SetActive(false);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        PlayClick();
        PlayerWallet.AddCoins(amount);
    }

    private void PlayClick()
    {
        MenuClickSound sound = clickSound != null ? clickSound : MenuClickSound.Get();
        sound?.Play();
    }
}
