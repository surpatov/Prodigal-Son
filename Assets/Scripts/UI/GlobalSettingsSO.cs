using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings", fileName = "NewSettings")]
public class GlobalSettingsSO : ScriptableObject
{
    [Range(0f, 1f)] public float volumeMusic;
    [Range(0f, 1f)] public float volumeSounds;
    [Range(2f, 300f)] public float mouseSensivity;

    private void Awake()
    {
        volumeMusic = 1f;
        volumeSounds = 1f;
        mouseSensivity = 20f;
    }
}