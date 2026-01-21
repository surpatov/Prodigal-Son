using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Televisor : MonoBehaviour, IZoomable, IClikable
{
    [SerializeField] int code;
    [SerializeField] Transform placeCassete;
    [SerializeField] Item cassete;
    [SerializeField] int canOpenID;
    [SerializeField] GameObject screen;
    private bool isCasseteIn;
    [SerializeField] private float speed;
    private Inventory inventory;
    [SerializeField] Quaternion rotateTo;

    private void Start()
    {
        screen.SetActive(false);
    }

    private void Update()
    {
        if (isCasseteIn)
            SetRoot();
    }

    public void Turn(Item item, Inventory inv)
    {
        if (isCasseteIn)
        {
            screen.SetActive(!screen.activeInHierarchy);
        }
        else if (item != null)
        {
            if (item.GetID == canOpenID && !isCasseteIn)
            {
                cassete = item;
                isCasseteIn = true;
                inventory = inv;
                inventory.DropItem(inventory.GetCurrentIndex());
                cassete.GetComponent<Rigidbody>().isKinematic = true;
                cassete.GetComponent<Collider>().enabled = false;
                cassete.transform.parent = placeCassete.transform;
            }
        }
    }
    private void SetRoot()
    {
        var step = speed * 2 * Time.deltaTime;
        var rotStep = speed * 2000 * Time.deltaTime;
        cassete.transform.position = Vector3.MoveTowards(cassete.transform.position, placeCassete.position, step);
        cassete.transform.localRotation = Quaternion.RotateTowards(cassete.transform.localRotation, rotateTo, rotStep);
    }
}