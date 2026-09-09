using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FinishOutro : MonoBehaviour
{
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip surpriseClip;
    [SerializeField] private AudioClip creditsClip;
    [SerializeField] private AudioClip musicClip;
    private PlayerController playerController;
    [SerializeField] private float creditsDuration = 10f;

    public void Activate(PlayerController _playerController)
    {
        playerController = _playerController;
        GetComponent<Collider>().enabled = false;
        soundSource?.PlayOneShot(creditsClip);

        playerController.canWalk = false;
        playerController.canRotation = false;
        playerController.canInteract = false;
        playerController.CameraHead.GetComponent<Animator>().SetBool("isRunning", false);
        playerController.CameraHead.GetComponent<Animator>().SetBool("isWalking", false);

        StartCoroutine(Reaction());
    }

    private IEnumerator Reaction()
    {
        yield return new WaitForSeconds(1.5f);

        soundSource?.PlayOneShot(surpriseClip, GameSettings.SoundsVolume);
        playerController.creditsCanvas.gameObject.SetActive(true);
        musicSource?.PlayOneShot(musicClip, GameSettings.MusicVolume);

        yield return new WaitForSeconds(2f);

        // Получаем панель с титрами (logoRect — это credits_panel)
        RectTransform panelRect = playerController.creditsCanvas.transform.GetChild(1).GetComponent<RectTransform>();

        // Получаем сам текст титров (credits)
        RectTransform creditsRect = panelRect.GetChild(0).GetComponent<RectTransform>();

        // Вычисляем, насколько нужно поднять панель, чтобы она полностью ушла за экран
        // Высота Canvas'а + высота панели
        RectTransform canvasRect = playerController.creditsCanvas.GetComponent<RectTransform>();
        float targetY = canvasRect.rect.height + panelRect.rect.height;

        // Анимируем панель вверх через DOAnchorPosY
        panelRect.DOAnchorPosY(targetY, creditsDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                StartCoroutine(GoMenu());
            });
    }
    private IEnumerator GoMenu()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Main menu");
    }
}