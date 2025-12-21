using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenu : MonoBehaviour
{
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle actionToggle;
    [SerializeField] private Toggle clickToggle;

    private void OnEnable()
    {
        SyncToggles();
    }

    private void SyncToggles()
    {
        if (bgmToggle != null)
        {
            bgmToggle.SetIsOnWithoutNotify(GameAudioSettings.IsBgmEnabled);
        }

        if (actionToggle != null)
        {
            actionToggle.SetIsOnWithoutNotify(GameAudioSettings.IsActionEnabled);
        }

        if (clickToggle != null)
        {
            clickToggle.SetIsOnWithoutNotify(GameAudioSettings.IsClickEnabled);
        }
    }

    public void SetBgm(bool isOn)
    {
        GameAudioSettings.SetBgmEnabled(isOn);
    }

    public void SetActionSound(bool isOn)
    {
        GameAudioSettings.SetActionEnabled(isOn);
    }

    public void SetClickSound(bool isOn)
    {
        GameAudioSettings.SetClickEnabled(isOn);
    }
}
