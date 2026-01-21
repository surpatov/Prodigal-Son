using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject mainUI;
    [SerializeField] GlobalSettingsSO settings;

    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSounds;
    [SerializeField] private Slider sliderMouse;

    [SerializeField] private GameObject[] soundsSources;
    [SerializeField] private GameObject[] musicSources;

    private void Start()
    {
        soundsSources = GameObject.FindGameObjectsWithTag("Sounds");
        musicSources = GameObject.FindGameObjectsWithTag("Music");
        sliderMusic.value = settings.volumeMusic;
        sliderSounds.value = settings.volumeSounds;
        sliderMouse.value = settings.mouseSensivity;
    }
    private void Update()
    {
        UpdateSources();
        settings.volumeMusic = sliderMusic.value;
        settings.volumeSounds = sliderSounds.value;
        settings.mouseSensivity = sliderMouse.value;
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
        mainUI.SetActive(true);
    }
    public void SettingsButton()
    {
        settingsUI.SetActive(true);
        mainUI.SetActive(false);
    }
    public void ContentButton()
    {

    }
    public void QuitButton()
    {
        Application.Quit();
    }
    private void UpdateSources()
    {
        foreach(var sound in soundsSources)
        {
            sound.GetComponent<AudioSource>().volume = settings.volumeSounds;
        }
        foreach(var music in musicSources)
        {
            music.GetComponent<AudioSource>().volume = settings.volumeMusic;
        }
    }
}