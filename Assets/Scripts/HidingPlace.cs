using UnityEngine;
using TMPro;

public class HidingPlace : MonoBehaviour
{
    [SerializeField] private Camera hidingCamera;
    [SerializeField] private AudioListener listener;
    [SerializeField] private Camera[] cameras;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI textGUI;
    [SerializeField] private Transform hidingPlace;
    [SerializeField] private float speed;

    private bool inPlace;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }
    private void Update()
    {
        if (inPlace) SetRootIn();
    }

    public void GetIn(Camera[] cameraOff, GameObject playerObject, TextMeshProUGUI _text)
    {
        listener.enabled = true;
        cameras = cameraOff;
        player = playerObject;
        hidingCamera.enabled = true;

        hidingCamera.transform.position = cameras[0].transform.position;
        hidingCamera.transform.rotation = cameras[0].transform.rotation;

        inPlace = true;

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
        if(hidingCamera.transform.position != hidingPlace.position && hidingCamera.transform.rotation != hidingPlace.transform.rotation)
        {
            var step = speed * 2 * Time.deltaTime;
            var rotStep = speed * 100 * Time.deltaTime;
            hidingCamera.transform.position = Vector3.MoveTowards(hidingCamera.transform.position, hidingPlace.position, step);
            hidingCamera.transform.rotation = Quaternion.RotateTowards(hidingCamera.transform.rotation, hidingPlace.transform.rotation, rotStep);
        }  
    }
}