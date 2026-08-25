using UnityEngine;
using TMPro;
using DG.Tweening;

public class HidingPlace : MonoBehaviour
{
    [SerializeField] private Camera hidingCamera;
    [SerializeField] private AudioListener listener;
    [SerializeField] private Camera[] cameras;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI textGUI;
    [SerializeField] private Transform hidingPlace;
    [SerializeField] private float speed;

    [Header("Processing")]
    [SerializeField] private float step;
    [SerializeField] private float timeToOpen;
    private float currentTime;

    private bool inPlace;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public float GetProcessing() => currentTime;
    public void GettingIn(Camera[] cameraOff, GameObject playerObject, TextMeshProUGUI _text)
    {
        currentTime += Time.deltaTime * step;
        if (currentTime >= timeToOpen)
        {
            GetIn(cameraOff, playerObject, _text);
            currentTime = 0f;
        }
    }
    public void StopGettinIn()
    {
        currentTime = 0f;
    }

    private void GetIn(Camera[] cameraOff, GameObject playerObject, TextMeshProUGUI _text)
    {
        listener.enabled = true;
        cameras = cameraOff;
        player = playerObject;
        hidingCamera.enabled = true;

        hidingCamera.transform.position = cameras[0].transform.position;
        hidingCamera.transform.rotation = cameras[0].transform.rotation;

        SetRootIn();

        hidingCamera.transform.GetChild(0).GetComponent<Camera>().enabled = true;
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
        hidingCamera.enabled = false;
        hidingCamera.transform.GetChild(0).GetComponent<Camera>().enabled = false;
    }
    private void SetRootIn()
    {
        Vector3 targetAngles = hidingPlace.transform.rotation.eulerAngles;
        targetAngles.x = 0;
        targetAngles.z = 0;

        hidingCamera.transform.DOMove(hidingPlace.position, 0.7f);
        hidingCamera.transform.DOLocalRotate(targetAngles, 0.7f);
    }
}