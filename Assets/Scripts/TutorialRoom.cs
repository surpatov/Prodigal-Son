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
    private int currentMessageIndex;
    private Collider roomCollider;

    private void Start()
    {
        roomCollider = GetComponent<Collider>();
        currentMessageIndex = -1;
        if(tutorialRoom)
            tutorialRoom.enabled = false;
    }
    private void OnEnable()
    {
        roomCollider = GetComponent<Collider>();
        currentMessageIndex = -1;
        if (tutorialRoom)
            tutorialRoom.enabled = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            ShowNextMessage();
        }
    }

    public void Enter()
    {
        roomCollider.enabled = false;
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
