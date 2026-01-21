using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject itemUI;
    [SerializeField] float speedRotation;

    [SerializeField] MeshRenderer[] models;
    [SerializeField] public int currentItem;

    [SerializeField] GameObject settingsObject;
    [SerializeField] GameObject mainObject;
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
    void Update()
    {
        UpdateSources();
        settings.volumeMusic = sliderMusic.value;
        settings.volumeSounds = sliderSounds.value;
        settings.mouseSensivity = sliderMouse.value;
        itemUI.transform.Rotate(0f, speedRotation * Time.deltaTime, 0f);
        for (int i = 0; i < models.Length; i++)
        {
            if (i == currentItem)
                models[i].enabled = true;
            else models[i].enabled = false;
        }
    }
    public void StartGame()
    {
        Debug.Log("GAME STARTED");
        SceneManager.LoadScene("Demo");
    }
    public void SettingsButton()
    {
        settingsObject.SetActive(true);
        mainObject.SetActive(false);
    }
    public void ContentButton()
    {
        
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void ReturnButton()
    {
        settingsObject.SetActive(false);
        mainObject.SetActive(true);
    }
    private void UpdateSources()
    {
        foreach (var sound in soundsSources)
        {
            sound.GetComponent<AudioSource>().volume = settings.volumeSounds;
        }
        foreach (var music in musicSources)
        {
            music.GetComponent<AudioSource>().volume = settings.volumeMusic;
        }
    }
}