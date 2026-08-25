using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyholeControl : MonoBehaviour, IZoomable
{
    [SerializeField] Inventory inventory;
    [SerializeField] Transform targetPos;
    [SerializeField] Quaternion targetRot;
    private bool isKeyed;
    [SerializeField] int openID;
    [SerializeField] float goalRotation;
    [SerializeField] Collider[] colliders;
    PlayerController playerController;

    [SerializeField] private Transform parentPoint;
    private Vector3 offset;

    Item key;
    public float speed = 1.0f;

    private void Start()
    {
        if (parentPoint != null)
            offset = transform.parent.position - parentPoint.position;
    }
    private void Update()
    {
        if(parentPoint != null)
            transform.position = parentPoint.position + offset;

        if (key != null)
            key.transform.position = transform.position;

        if (isKeyed)
            SetRoot();
        if(goalRotation - transform.localEulerAngles.z < 1f)
        {
            colliders[0].enabled = false;
            colliders[1].enabled = false;
            playerController.canWalk = true;
            playerController.canRotation = true;
        }
    }
    public void Open(PlayerController playerControl, Item item, Inventory inv)
    {
        if(item != null)
        {
            if (item.GetID == openID && !isKeyed)
            {
                inventory = inv;
                key = item;
                isKeyed = true;
                inventory.DropItem(inventory.GetCurrentIndex());
                key.GetComponent<Rigidbody>().useGravity = false;
                key.gameObject.layer = 0;
                key.GetComponent<Collider>().enabled = false;
                key.gameObject.transform.parent = transform;
            }
        }
        if (isKeyed)
        {
            playerController = playerControl;
            playerControl.canWalk = false;
            playerControl.canRotation = false;

            float direction = Input.GetAxis("Mouse X");
            float newRotation = Mathf.Clamp(direction * Time.deltaTime * 700f, 0f, 90f);

            transform.Rotate(new Vector3(0f, 0f, 1f), newRotation);
        }
    }
    private void SetRoot()
    {
        var step = speed * 4 * Time.deltaTime;
        var rotStep = speed * 400 * Time.deltaTime;
        key.transform.position = Vector3.MoveTowards(key.transform.position, targetPos.position, step);
        key.transform.localRotation = Quaternion.RotateTowards(key.transform.localRotation, targetRot, rotStep);
    }
}
