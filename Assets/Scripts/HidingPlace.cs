using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HidingPlace : MonoBehaviour
{
    [SerializeField] private Transform place;
    [SerializeField] private AudioListener listener;
    [SerializeField] private Camera[] cameras;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI textGUI;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public void GetIn(Camera[] cameraOff, GameObject playerObject, TextMeshProUGUI _text)
    {
        listener.enabled = true;
        cameras = cameraOff;
        player = playerObject;
        place.GetComponent<Camera>().enabled = true;
        place.GetChild(0).GetComponent<Camera>().enabled = true;
        foreach (var camera in cameras) { camera.enabled = false; }
        player.gameObject.SetActive(false);
        textGUI = _text;
        textGUI.text = "get out [RMB]";
    }
    public void GetOut()
    {
        listener.enabled = false;
        player.gameObject.SetActive(true);
        foreach (var camera in cameras) { camera.enabled = true; }
        place.GetComponent<Camera>().enabled = false;
        place.GetChild(0).GetComponent<Camera>().enabled = false;
    }
}