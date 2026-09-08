using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TelephoneIntro : MonoBehaviour
{
    [SerializeField] private Collider[] collidersBlock;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip callEndClip;
    [SerializeField] private Transform pointToLook;
    [SerializeField] private float lookDuration = 1f;
    [SerializeField] private Monster monster;
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private Transform monsterGoPoint;
    [SerializeField] private Transform monsterRotatePoint;

    private PlayerController playerController;

    public void Activate(PlayerController _playerController)
    {
        playerController = _playerController;

        audioSource.Stop();
        audioSource.loop = false;
        audioSource.PlayOneShot(callEndClip);

        GetComponent<Collider>().enabled = false;
        foreach (var collider in collidersBlock)
        {
            collider.enabled = false;
        }

        playerController.canWalk = false;
        playerController.canRotation = false;
        playerController.canInteract = false;

        StartCoroutine(MonsterSpawn());
    }

    private IEnumerator MonsterSpawn()
    {
        yield return new WaitForSeconds(1f);

        Vector3 directionToTarget = pointToLook.position - playerController.CameraHead.transform.position;
        directionToTarget.y = 0f;

        Quaternion targetPlayerRotation = Quaternion.LookRotation(directionToTarget);
        playerController.transform.DORotateQuaternion(targetPlayerRotation, lookDuration).SetEase(Ease.InOutSine);

        playerController.cameraObject.transform.DOLookAt(pointToLook.position, lookDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                UpdateCameraXRotation();
                playerController.CameraHead.GetComponent<Animator>().SetBool("isZoom", true);
            });

        Monster spawnedMonster = Instantiate(monster, monsterSpawnPoint.position, monsterSpawnPoint.rotation);
        spawnedMonster.enabled = false;
        spawnedMonster.animator.SetTrigger("WALK");
        spawnedMonster.transform.DOMove(monsterGoPoint.position, 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            spawnedMonster.animator.SetTrigger("STAY");
            Vector3 direction = Quaternion.LookRotation(monsterRotatePoint.position - spawnedMonster.transform.position).eulerAngles;
            direction.x = 0f;
            direction.z = 0f;
            spawnedMonster.transform.DORotate(direction, 1f).SetEase(Ease.InOutSine);
        });

        yield return new WaitForSeconds(lookDuration + 4f);

        spawnedMonster.enabled = true;
        spawnedMonster.animator.SetTrigger("RUN");

        playerController.CameraHead.GetComponent<Animator>().SetBool("isZoom", false);
        playerController.canWalk = true;
        playerController.canRotation = true;
        playerController.canInteract = true;
    }

    private void UpdateCameraXRotation()
    {
        Vector3 currentRotation = playerController.cameraObject.transform.localRotation.eulerAngles;
        float currentXRotation = currentRotation.x;

        if (currentXRotation > 180f) currentXRotation -= 360f;

        var field = typeof(PlayerController).GetField("xRotation",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            field.SetValue(playerController, currentXRotation);
        }
    }
}