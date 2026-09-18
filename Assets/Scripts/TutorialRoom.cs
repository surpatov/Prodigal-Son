using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialRoom : MonoBehaviour
{
    [SerializeField] private string[] messages;
    [SerializeField] private Collider doorCollider;
    [SerializeField] private Image messageImage;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TutorialRoom tutorialRoom;
    [SerializeField] private Arrow[] arrows;
    private int currentMessageIndex;
    private Collider roomCollider;

    bool canShow;

    private void Start()
    {
        roomCollider = GetComponent<Collider>();
        currentMessageIndex = -1;
    }
    private void OnEnable()
    {
        roomCollider = GetComponent<Collider>();
        currentMessageIndex = -1;
    }
    private void Update()
    {
        if (!canShow) return;
        if(Input.GetKeyDown(KeyCode.G) || ControllerInput.ADown())
        {
            ShowNextMessage();
        }
    }

    public void Enter()
    {
        if(roomCollider != null)
            roomCollider.enabled = false;
        canShow = true;
        ShowNextMessage();
        // звук
    }
    private void ShowNextMessage()
    {
        currentMessageIndex++;
        if (currentMessageIndex < messages.Length)
        {
            messageText.text = messages[currentMessageIndex];
            messageImage.gameObject.SetActive(true);
            arrows[currentMessageIndex]?.gameObject.SetActive(true);
        }
        else
        {
            messageImage.gameObject.SetActive(false);
            doorCollider.enabled = false;
            if (tutorialRoom)
                tutorialRoom.enabled = true;
        }
    }
}
