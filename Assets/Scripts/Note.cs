using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] private bool isInvinsible;
    [SerializeField] private float timeLeft;
    [SerializeField] private float timeToPressed;
    public string GetMessageText => !isInvinsible ? message : " ";
    public bool IsInvinsible => isInvinsible;
    public float TimeLeft => timeLeft;
    private void Start()
    {
        timeLeft = timeToPressed;
    }
    private void Update()
    {
        if(timeLeft <= 0)
            isInvinsible = false;
    }
    public void Pressing()
    {
        timeLeft -= Time.deltaTime;
    }
}