using UnityEngine;

public class GameplaySfxPlayer : MonoBehaviour
{
    public static GameplaySfxPlayer Active { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip starClip;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        Active = this;
    }

    private void OnDisable()
    {
        if (Active == this)
        {
            Active = null;
        }
    }

    public static GameplaySfxPlayer Get()
    {
        if (Active == null)
        {
            Active = FindObjectOfType<GameplaySfxPlayer>();
        }

        return Active;
    }

    public void PlayJump()
    {
        Play(jumpClip);
    }

    public void PlayHurt()
    {
        Play(hurtClip);
    }

    public void PlayCoin()
    {
        Play(coinClip);
    }

    public void PlayStar()
    {
        Play(starClip);
    }

    private void Play(AudioClip clip)
    {
        if (!GameAudioSettings.IsActionEnabled)
        {
            return;
        }

        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}
