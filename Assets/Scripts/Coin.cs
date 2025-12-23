using UnityEngine;

// Collects when the player touches the coin (no need to hit from below).
public class Coin : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.05f;
    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player")) {
            return;
        }

        collected = true;
        GameManager.Instance?.AddCoin();
        PlayerWallet.AddCoins(1);
        GameplaySfxPlayer.Get()?.PlayCoin();

        // Disable visuals and collisions immediately to avoid double pickups.
        var collider = GetComponent<Collider2D>();
        if (collider != null) {
            collider.enabled = false;
        }

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            spriteRenderer.enabled = false;
        }

        Destroy(gameObject, destroyDelay);
    }
}
