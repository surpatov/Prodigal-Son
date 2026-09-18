using TMPro;
using UnityEngine;
using DG.Tweening;

public class Note : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] private bool isInvinsible;
    [SerializeField] private float timeLeft;
    [SerializeField] private float timeToPressed;
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 rotateOffset;
    public string GetMessageText => !isInvinsible ? message : " ";
    public bool IsInvinsible => isInvinsible;
    public float TimeLeft => timeLeft;
    bool isReading = false;
    UnityEngine.UI.Image panel;
    TMP_Text text;
    PlayerController playerController;

    private void Start()
    {
        timeLeft = timeToPressed;
    }
    private void Update()
    {
        if(timeLeft <= 0)
            isInvinsible = false;

        if(isReading && (Input.GetKeyDown(KeyCode.Q) || ControllerInput.BDown()))
        {
            StopRead();
            ControllerInput.Update();
        }
    }
    public void Pressing()
    {
        timeLeft -= Time.deltaTime;
    }
    public void Read(UnityEngine.UI.Image _panel, TMP_Text _text, PlayerController _playerController)
    {
        isReading = true;
        panel = _panel;
        text = _text;
        playerController = _playerController;
        panel.gameObject.SetActive(true);
        text.text = GetMessageText;
        playerController.canWalk = false;
        playerController.canRotation = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        transform.DOLocalMove(moveOffset, 0.5f);
        transform.DOLocalRotate(rotateOffset, 0.5f);
    }
    private void StopRead()
    {
        panel.gameObject.SetActive(false);
        text.text = " ";
        playerController.canWalk = true;
        playerController.canRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        transform.DOLocalMove(Vector3.zero, 0.5f);
        transform.DOLocalRotate(Vector3.zero, 0.5f);
    }
}