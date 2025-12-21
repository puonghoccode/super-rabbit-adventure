using System;
using UnityEngine;

public static class GameAudioSettings
{
    public static event Action SettingsChanged;

    private const string BgmKey = "audio_bgm";
    private const string ActionKey = "audio_action";
    private const string ClickKey = "audio_click";

    public static bool IsBgmEnabled => PlayerPrefs.GetInt(BgmKey, 1) != 0;
    public static bool IsActionEnabled => PlayerPrefs.GetInt(ActionKey, 1) != 0;
    public static bool IsClickEnabled => PlayerPrefs.GetInt(ClickKey, 1) != 0;

    public static void SetBgmEnabled(bool enabled)
    {
        Set(BgmKey, enabled);
    }

    public static void SetActionEnabled(bool enabled)
    {
        Set(ActionKey, enabled);
    }

    public static void SetClickEnabled(bool enabled)
    {
        Set(ClickKey, enabled);
    }

    private static void Set(string key, bool enabled)
    {
        PlayerPrefs.SetInt(key, enabled ? 1 : 0);
        PlayerPrefs.Save();
        SettingsChanged?.Invoke();
    }
}
