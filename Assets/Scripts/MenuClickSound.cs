using UnityEngine;

public class MenuClickSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
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
