using UnityEngine;

public static class GameSettings
{
    public static float MusicVolume;
    public static float SoundsVolume;

    public static float MouseSensitivity;
    public static bool IsXVert;
    public static bool IsYVert;

    public static int CurrentFPSMode;
    public static bool IsFullscreen;
    public static bool IsVSyncEnabled;

    public static void GetSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SoundsVolume = PlayerPrefs.GetFloat("SoundsVolume", 1f);
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 20f);
        IsXVert = PlayerPrefs.GetInt("IsXVert", 0) == 1;
        IsYVert = PlayerPrefs.GetInt("IsYVert", 0) == 1;
        CurrentFPSMode = PlayerPrefs.GetInt("CurrentFPSMode", 1);
        IsFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        IsVSyncEnabled = PlayerPrefs.GetInt("IsVSyncEnabled", 0) == 1;

        QualitySettings.vSyncCount = IsVSyncEnabled ? 1 : 0;
        Screen.fullScreen = IsFullscreen;
        Application.targetFrameRate = 30 + CurrentFPSMode * 30;
    }
    public static void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SoundsVolume", SoundsVolume);
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
        PlayerPrefs.SetInt("IsXVert", IsXVert ? 1 : 0);
        PlayerPrefs.SetInt("IsYVert", IsYVert ? 1 : 0);
        PlayerPrefs.SetInt("CurrentFPSMode", CurrentFPSMode);
        PlayerPrefs.SetInt("IsFullscreen", IsFullscreen ? 1 : 0);
        PlayerPrefs.SetInt("IsVSyncEnabled", IsVSyncEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
}