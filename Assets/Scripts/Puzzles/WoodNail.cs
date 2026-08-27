using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodNail : MonoBehaviour, IZoomable
{
    [SerializeField] Inventory inventory;
    [SerializeField] Transform rootPos;
    [SerializeField] float speed;
    [SerializeField] GameObject woodPlank;
    [SerializeField] GameObject anotherNail;
    [SerializeField] float timeToOpen;
    [SerializeField] float timeLeft;
    [SerializeField] int canOpen;
    private void Start()
    {
        timeLeft = timeToOpen;
    }
    public void ResetTime()
    {
        timeLeft = timeToOpen;
    }
    public void Open(Item item, Inventory inv)
    {
        inventory = inv;
        if (item != null && item.GetID == canOpen)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                RootPosition(item.gameObject);
            }
            else Destroy(gameObject);
        }
    }
    private void RootPosition(GameObject scrap)
    {
        inventory.canRoot = false;

        scrap.GetComponent<Animator>().SetBool("isOpen", true);
        scrap.transform.GetChild(0).gameObject.layer = 9;

        var colliders = scrap.GetComponents<Collider>();
        foreach(var collider in colliders)
            collider.enabled = false;
        var step = speed * 2 * Time.deltaTime;
        var rotStep = speed * 200 * Time.deltaTime;
        scrap.transform.position = Vector3.MoveTowards(scrap.transform.position, rootPos.position, step);
        scrap.transform.rotation = Quaternion.RotateTowards(scrap.transform.rotation, rootPos.rotation, rotStep);
    }
}