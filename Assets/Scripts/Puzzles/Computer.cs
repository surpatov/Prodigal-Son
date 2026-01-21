using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Computer : MonoBehaviour, IZoomable
{
    [SerializeField] int code;
    [SerializeField] Transform placeDisc;
    [SerializeField] Item disc;
    [SerializeField] int canOpenID;
    [SerializeField] GameObject screen;
    private bool isDiscIn;
    [SerializeField] private float speed;
    private Inventory inventory;
    [SerializeField] Quaternion rotateTo;
    [SerializeField] AudioClip turnOnClip;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] private VideoClip videoClip;

    private void Start()
    {
        screen.SetActive(false);
    }

    private void Update()
    {
        if (isDiscIn)
            SetRoot();
    }

    public void Turn(Item item, Inventory inv)
    {
        if (!screen.activeInHierarchy)
        {
            screen.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(turnOnClip);
        }
        else TurnVideo(item, inv);
    }
    private void TurnVideo(Item item, Inventory inv)
    {
        if (item != null)
        {
            if (item.GetID == canOpenID && !isDiscIn)
            {
                disc = item;
                isDiscIn = true;
                inventory = inv;
                inventory.DropItem(inventory.GetCurrentIndex());
                disc.GetComponent<Rigidbody>().isKinematic = true;
                disc.GetComponent<Collider>().enabled = false;
                disc.transform.parent = placeDisc.transform;
                isDiscIn = true;
                videoPlayer.clip = videoClip;
            }
        }
    }
    private void SetRoot()
    {
        var step = speed * 2 * Time.deltaTime;
        var rotStep = speed * 2000 * Time.deltaTime;
        disc.transform.position = Vector3.MoveTowards(disc.transform.position, placeDisc.position, step);
        disc.transform.localRotation = Quaternion.RotateTowards(disc.transform.localRotation, rotateTo, rotStep);
    }
}
