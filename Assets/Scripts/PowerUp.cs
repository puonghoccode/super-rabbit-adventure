using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        Star,
        MagicPotion,
        JellyPower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player)) {
            Collect(player);
        }
    }

    private void Collect(Player player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                PlayerWallet.AddCoins(1);
                GameplaySfxPlayer.Get()?.PlayCoin();
                break;

            case Type.Star:
                GameManager.Instance?.AddStar();
                GameplaySfxPlayer.Get()?.PlayStar();
                break;

            case Type.MagicPotion:
                player.Grow();
                break;

            case Type.JellyPower:
                player.JellyPower();
                break;
        }

        Destroy(gameObject);
    }

}
