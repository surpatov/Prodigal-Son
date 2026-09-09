using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementOutro : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip noiseClip;
    [SerializeField] private AudioClip scaryClip;
    [SerializeField] private Transform door;
    [SerializeField] private Transform pointToLook1;
    [SerializeField] private Transform pointToLook2;
    [SerializeField] private float lookDuration1 = 1f;
    [SerializeField] private float lookDuration2 = 1f;
    [SerializeField] private GameObject monster;
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private Transform playerFinishSpawn;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Camera finishCamera;

    PlayerController playerController;

    public void Activate(PlayerController _playerController)
    {
        GetComponent<Collider>().enabled = false;
        playerController = _playerController;

        audioSource?.PlayOneShot(noiseClip);
        door.transform.rotation = Quaternion.Euler(-89.98f, 0f, 0f);
        playerController.canWalk = false;
        playerController.canRotation = false;
        playerController.canInteract = false;
        playerController.CameraHead.GetComponent<Animator>().SetBool("isRunning", false);
        playerController.CameraHead.GetComponent<Animator>().SetBool("isWalking", false);

        StartCoroutine(NoiseHear());
    }

    private IEnumerator NoiseHear()
    {
        yield return new WaitForSeconds(2.5f);

        Vector3 directionToTarget = pointToLook1.position - playerController.CameraHead.transform.position;
        directionToTarget.y = 0f;

        Quaternion targetPlayerRotation = Quaternion.LookRotation(directionToTarget);
        playerController.transform.DORotateQuaternion(targetPlayerRotation, lookDuration1).SetEase(Ease.InOutSine);

        playerController.cameraObject.transform.DOLookAt(pointToLook1.position, lookDuration1)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                StartCoroutine(MonsterSpawn());
            });

        
    }
    private IEnumerator MonsterSpawn()
    {
        yield return new WaitForSeconds(2f);

        GameObject spawnedMonster = Instantiate(monster, monsterSpawnPoint.position, monsterSpawnPoint.rotation);
        spawnedMonster.GetComponent<Animator>().SetTrigger("STAY");

        Vector3 directionToTarget = pointToLook2.position - playerController.CameraHead.transform.position;
        directionToTarget.y = 0f;

        Quaternion targetPlayerRotation = Quaternion.LookRotation(directionToTarget);
        playerController.transform.DORotateQuaternion(targetPlayerRotation, lookDuration2).SetEase(Ease.InOutSine);

        playerController.cameraObject.transform.DOLookAt(pointToLook2.position, lookDuration2)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                playerController.CameraHead.GetComponent<Animator>().SetTrigger("SHAKE");
                StartCoroutine(PlayerReaction());
            });
    }
    private IEnumerator PlayerReaction()
    {
        yield return new WaitForSeconds(1.5f);
        
        // анимация или что-то подобное смерти
        playerController.laterCanvas.gameObject.SetActive(true);
        playerController.canvas.gameObject.SetActive(false);
        // звук кинематографичный

        yield return new WaitForSeconds(2f);

        CanvasGroup canvasGroup = playerController.laterCanvas.GetComponent<CanvasGroup>();
        finishCamera.gameObject.SetActive(true);
        canvasGroup.DOFade(0f, 2f).OnComplete(() =>
        {
            Destroy(playerController.gameObject);
        });
        GameObject player = Instantiate(playerPrefab, playerFinishSpawn.position, playerFinishSpawn.rotation);
    }
}
