using UnityEngine;

public class MenuBgmPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameAudioSettings.SettingsChanged += ApplySettings;
        ApplySettings();
    }

    private void OnDisable()
    {
        GameAudioSettings.SettingsChanged -= ApplySettings;
    }

    private void ApplySettings()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            return;
        }

        if (GameAudioSettings.IsBgmEnabled)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
