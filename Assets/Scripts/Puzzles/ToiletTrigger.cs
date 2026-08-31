using UnityEngine;
using DG.Tweening;

public class ToiletTrigger : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private Transform pointTo;
    private bool isActive = false;
    public void CanActivate() => isActive = true;

    public void Activate()
    {
        //if (!isActive) return;

        item.transform.DOMove(pointTo.position, 0.5f);
        // звук смывания
        GetComponent<Collider>().enabled = false;
    }
}
