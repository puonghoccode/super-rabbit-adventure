using UnityEngine;

public class MenuClickSound : MonoBehaviour
{
    public static MenuClickSound Active { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
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

    public static MenuClickSound Get()
    {
        if (Active == null)
        {
            Active = FindObjectOfType<MenuClickSound>();
        }

        return Active;
    }

    public void Play()
    {
        if (!GameAudioSettings.IsClickEnabled)
        {
            return;
        }

        if (audioSource == null || clickClip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clickClip);
    }

    public void Play(bool isOn)
    {
        Play();
    }
}
