using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    void Start()
    {
        playerController.canWalk = false;
        playerController.canRotation = false;
        playerController.canInteract = false;
        playerController.canvas.GetComponent<CanvasGroup>().alpha = 0f;
    }
    public void Activate()
    {
        playerController.canWalk = true;
        playerController.canRotation = true;
        playerController.canInteract = true;
        gameObject.SetActive(false);
        playerController.canvas.GetComponent<CanvasGroup>().alpha = 1f;
    }
}
