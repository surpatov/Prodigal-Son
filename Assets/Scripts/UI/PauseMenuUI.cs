using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject settingsUI;

    [Header("Audio Settings")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSounds;

    [Header("Controls Settings")]
    [SerializeField] private Slider sliderMouse;
    [SerializeField] private Toggle toggleInvertY;
    [SerializeField] private Toggle toggleInvertX;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown dropdownFPS;
    [SerializeField] private Toggle toggleVSync;
    [SerializeField] private Toggle toggleFullscreen;

    [SerializeField] private GameObject[] soundsSources;
    [SerializeField] private GameObject[] musicSources;

    private void OnEnable()
    {
        GameSettings.GetSettings();

        soundsSources = GameObject.FindGameObjectsWithTag("Sounds");
        musicSources = GameObject.FindGameObjectsWithTag("Music");

        sliderMusic.value = GameSettings.MusicVolume;
        sliderSounds.value = GameSettings.SoundsVolume;

        sliderMouse.value = GameSettings.MouseSensitivity;
        toggleInvertY.isOn = GameSettings.IsYVert;
        toggleInvertX.isOn = GameSettings.IsXVert;

        toggleFullscreen.isOn = GameSettings.IsFullscreen;
        toggleVSync.isOn = GameSettings.IsVSyncEnabled;
        dropdownFPS.value = GameSettings.CurrentFPSMode;
    }
    private void Start()
    {
        GameSettings.GetSettings();

        soundsSources = GameObject.FindGameObjectsWithTag("Sounds");
        musicSources = GameObject.FindGameObjectsWithTag("Music");

        sliderMusic.value = GameSettings.MusicVolume;
        sliderSounds.value = GameSettings.SoundsVolume;

        sliderMouse.value = GameSettings.MouseSensitivity;
        toggleInvertY.isOn = GameSettings.IsYVert;
        toggleInvertX.isOn = GameSettings.IsXVert;

        toggleFullscreen.isOn = GameSettings.IsFullscreen;
        toggleVSync.isOn = GameSettings.IsVSyncEnabled;
        dropdownFPS.value = GameSettings.CurrentFPSMode;
    }
    private void Update()
    {
        UpdateSources();
    }

    public void OnMusicSliderChanged()
    {
        GameSettings.MusicVolume = sliderMusic.value;
        GameSettings.SaveSettings();
    }
    public void OnSoundsSliderChanged()
    {
        GameSettings.SoundsVolume = sliderSounds.value;
        GameSettings.SaveSettings();
    }
    public void OnMouseSliderChanged()
    {
        GameSettings.MouseSensitivity = sliderMouse.value;
        GameSettings.SaveSettings();
    }
    public void OnInvertYToggleChanged()
    {
        GameSettings.IsYVert = toggleInvertY.isOn;
        GameSettings.SaveSettings();
    }
    public void OnInvertXToggleChanged()
    {
        GameSettings.IsXVert = toggleInvertX.isOn;
        GameSettings.SaveSettings();
    }
    public void OnFullscreenToggleChanged()
    {
        GameSettings.IsFullscreen = toggleFullscreen.isOn;
        Screen.fullScreen = GameSettings.IsFullscreen;
        GameSettings.SaveSettings();
    }
    public void OnVSyncToggleChanged()
    {
        GameSettings.IsVSyncEnabled = toggleVSync.isOn;
        QualitySettings.vSyncCount = GameSettings.IsVSyncEnabled ? 1 : 0;
        GameSettings.SaveSettings();
    }
    public void OnFPSDropdownChanged()
    {
        GameSettings.CurrentFPSMode = dropdownFPS.value;
        Application.targetFrameRate = 30 + GameSettings.CurrentFPSMode * 30;
        GameSettings.SaveSettings();
    }

    public void ReturnButton()
    {
        pauseMenu.SetActive(false);
        gameplayUI.SetActive(true);
        settingsUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        Time.timeScale = 1f;
    }
    public void SettingsReturn()
    {
        settingsUI.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
    }
    public void SettingsButton()
    {
        settingsUI.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(false);
    }
    public void ContentButton()
    {

    }
    public void MenuButton()
    {
        SceneManager.LoadScene("Main menu"); // сделать переход
    }
    private void UpdateSources()
    {
        foreach(var sound in soundsSources)
        {
            sound.GetComponent<AudioSource>().volume = GameSettings.SoundsVolume;
        }
        foreach(var music in musicSources)
        {
            music.GetComponent<AudioSource>().volume = GameSettings.MusicVolume;
        }
    }
}