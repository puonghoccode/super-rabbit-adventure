using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public PlayerMovement movement { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }

    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer;
    private Vector2 smallColliderSize;
    private Vector2 smallColliderOffset;

    public bool big => bigRenderer != null && bigRenderer.isActiveAndEnabled;
    public bool dead => deathAnimation.enabled;
    public bool jellypower { get; private set; }

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        movement = GetComponent<PlayerMovement>();
        deathAnimation = GetComponent<DeathAnimation>();
        activeRenderer = smallRenderer;
        smallColliderSize = capsuleCollider.size;
        smallColliderOffset = capsuleCollider.offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage"))
        {
            Hit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage"))
        {
            Hit();
        }
    }

    private void Start()
    {
        SetSmallImmediate();
    }

    public void Hit()
    {
        if (!dead && !jellypower)
        {
            if (big) {
                Shrink();
            } else {
                Death();
            }
        }
    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    public void Grow()
    {
        EnsureRendererObjectsActive();
        SetRendererVisible(smallRenderer, false);
        SetRendererVisible(bigRenderer, true);
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(0.7f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    public void Shrink()
    {
        EnsureRendererObjectsActive();
        SetRendererVisible(smallRenderer, true);
        SetRendererVisible(bigRenderer, false);
        activeRenderer = smallRenderer;

        capsuleCollider.size = smallColliderSize;
        capsuleCollider.offset = smallColliderOffset;

        StartCoroutine(ScaleAnimation());
    }

    private void SetSmallImmediate()
    {
        EnsureRendererObjectsActive();
        SetRendererVisible(smallRenderer, true);
        SetRendererVisible(bigRenderer, false);

        if (smallRenderer != null) {
            activeRenderer = smallRenderer;

            if (capsuleCollider != null) {
                capsuleCollider.size = smallColliderSize;
                capsuleCollider.offset = smallColliderOffset;
            }
        } else if (bigRenderer != null) {
            SetRendererVisible(bigRenderer, true);
            activeRenderer = bigRenderer;
        }
    }

    private void EnsureRendererObjectsActive()
    {
        if (smallRenderer != null && !smallRenderer.gameObject.activeSelf) {
            smallRenderer.gameObject.SetActive(true);
        }
        if (bigRenderer != null && !bigRenderer.gameObject.activeSelf) {
            bigRenderer.gameObject.SetActive(true);
        }
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                bool showSmall = smallRenderer != null && smallRenderer.enabled;
                showSmall = !showSmall;
                SetRendererVisible(smallRenderer, showSmall);
                SetRendererVisible(bigRenderer, !showSmall);
            }

            yield return null;
        }

        SetRendererVisible(smallRenderer, false);
        SetRendererVisible(bigRenderer, false);
        SetRendererVisible(activeRenderer, true);
    }

    private void SetRendererVisible(PlayerSpriteRenderer renderer, bool visible)
    {
        if (renderer == null) {
            return;
        }

        renderer.enabled = visible;

        SpriteRenderer spriteRenderer = renderer.spriteRenderer;
        if (spriteRenderer == null) {
            spriteRenderer = renderer.GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null) {
            spriteRenderer.enabled = visible;
        }

        if (!visible && renderer.run != null) {
            renderer.run.enabled = false;
        }
    }

    public void JellyPower()
    {
        StartCoroutine(JellyPowerAnimation());
    }

    private IEnumerator JellyPowerAnimation()
    {
        jellypower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        jellypower = false;
    }

}
