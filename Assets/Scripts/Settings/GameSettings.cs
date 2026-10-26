using System;
using UnityEngine;

// Player settings, persisted in PlayerPrefs
public static class GameSettings
{
    private const string MusicKey = "Settings.MusicVolume";
    private const string SfxKey = "Settings.SfxVolume";
    private const string VibrationKey = "Settings.Vibration";

    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicKey, 0.7f);
        set => Save(() => PlayerPrefs.SetFloat(MusicKey, Mathf.Clamp01(value)));
    }

    public static float SfxVolume
    {
        get => PlayerPrefs.GetFloat(SfxKey, 1f);
        set => Save(() => PlayerPrefs.SetFloat(SfxKey, Mathf.Clamp01(value)));
    }

    public static bool Vibration
    {
        get => PlayerPrefs.GetInt(VibrationKey, 1) == 1;
        set => Save(() => PlayerPrefs.SetInt(VibrationKey, value ? 1 : 0));
    }

    // The platform remembers this one itself
    public static bool Fullscreen
    {
        get => Screen.fullScreen;
        set => Screen.fullScreen = value;
    }

    public static event Action OnChanged;

    private static void Save(Action write)
    {
        write();
        PlayerPrefs.Save();
        OnChanged?.Invoke();
    }
}