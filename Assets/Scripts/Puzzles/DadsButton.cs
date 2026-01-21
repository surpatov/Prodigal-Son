using UnityEngine;

public class DadsButton : MonoBehaviour
{
    [SerializeField] private Animator secretComode;

    public void Open()
    {
        secretComode.SetTrigger("OPEN");
    }
}
