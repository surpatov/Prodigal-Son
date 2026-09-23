using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] public GameObject[] bullets;
    [SerializeField] public int maxBullets;
    [SerializeField] private Transform bulletsPlace;
    [SerializeField] public GameObject currentSell;
    [SerializeField] private AudioSource source;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private List<Item> items;
    public bool IsEmpty => items.Count == 0;
    [SerializeField] private int indexCurrent;
    [SerializeField] private Transform hand;
    private bool canSet;
    [SerializeField] private float itemSetSpeed;
    [SerializeField] Vector3 offset;
    [SerializeField] public bool canRoot;

    [SerializeField] private Image[] slotsUI;

    private void Start()
    {
        canSet = true;
        canRoot = true;
    }
    private void Update()
    {
        SetRootSells();
        UpdateItems();
        if (items[indexCurrent] && canRoot)
        {
            ItemSetRoot(items[indexCurrent], items[indexCurrent].GetRootPosition, items[indexCurrent].GetRootRotation);
        }
        if(canSet)
            SetIndex();
    }
    public int CountSellsNotNull()
    {
        if (bullets == null)
            return 0;

        int count = 0;

        foreach (var item in bullets)
        {
            if (item != null)
            {
                count++;
            }
        }
        return count;
    }
    private void SetRootSells()
    {
        foreach(var sell in bullets)
        {
            if(sell != null)
            {
                var step = itemSetSpeed / 2 * Time.deltaTime;
                sell.transform.localPosition = Vector3.MoveTowards(sell.transform.position, bulletsPlace.position, step);
                if (Vector3.Distance(sell.transform.localPosition, bulletsPlace.position) < 0.1f)
                {
                    sell.transform.parent = bulletsPlace.transform;
                }
            }
        }
    }
    public int GetCurrentIndex() => indexCurrent;
    public int GetCurrentItemID => items[indexCurrent].GetID;
    public Item GetCurrentItem => items[indexCurrent];
    public void PickUpItem(Item item)
    {
        if (items[indexCurrent] == null) // если ДАННЫЙ слот не занят, в него подбирается предмет
            SetUpItem(indexCurrent, item);
        else if (indexCurrent == 0 && items[1] == null) // если ДРУГОЙ слот не занят, в него подбирается предмет (во вторую ячейку)
            SetUpItem(1, item);
        else if (indexCurrent == 1 && items[0] == null) // если ДРУГОЙ слот не занят, в него подбирается предмет (в первую ячейку)
            SetUpItem(0, item);
        else // если все слоты заняты, то выкидывается из ДАННОГО слота и в ДАННЫЙ слот подбирается предмет
        {
            DropItem(indexCurrent);

            SetUpItem(indexCurrent, item);
        }
    }
    private void SetUpItem(int index, Item _item)
    {
        items[index] = _item;
        items[index].transform.parent = hand.transform;
        items[index].GetComponent<Rigidbody>().useGravity = false;
        items[index].GetComponent<Rigidbody>().isKinematic = true;
        items[index].GetComponent<Rigidbody>().freezeRotation = false;
        Collider[] colliders = items[index].GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
        Transform[] children = items[index].GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Hands");
        }
        if (items[index].GetComponent<Shotgun>())
        {
            Invoke("ShootGunTake", 0.5f);
            items[index].GetComponent<Shotgun>().playerCamera = GetComponent<Animator>();
            items[index].GetComponent<Shotgun>().source = source;
            items[index].GetComponent<Shotgun>().cameraCenter = transform.parent;
            items[index].GetComponent<Shotgun>().inventory = GetComponent<Inventory>();
        }
        if (items[index].GetComponent<Detector>())
            items[index].GetComponent<Detector>().HandsTake(true);
        else if (items[index].GetComponent<Flashlight>())
            items[index].GetComponent<Flashlight>().TakeInHands();
    }
    private void ShootGunTake()
    {
        items[GetCurrentIndex()].GetComponent<Shotgun>().TakeInHands();
    }
    private void UpdateItems()
    {
        if(indexCurrent == 0)
        {
            if(items[0])
                items[0].gameObject.SetActive(true);
            if (items[1])
                items[1].gameObject.SetActive(false);
        }
        else
        {
            if (items[0])
                items[0].gameObject.SetActive(false);
            if (items[1])
                items[1].gameObject.SetActive(true);
        }
    }
    private void ItemSetRoot(Item item, Vector3 moveTo, Quaternion rotateTo)
    {
        if (item)
        {
            var step = itemSetSpeed / 2 * Time.deltaTime;
            var rotStep = itemSetSpeed * 100 * Time.deltaTime;
            item.transform.localPosition = Vector3.MoveTowards(item.gameObject.transform.localPosition, moveTo, step);
            item.transform.localRotation = Quaternion.RotateTowards(item.transform.localRotation, rotateTo, rotStep);
            item.gameObject.layer = LayerMask.NameToLayer("Hands");

            if (item.transform.childCount == 0) return;
            foreach(Transform child in item.transform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Hands");
            }
        }
    }
    public void DropItem(int index)
    {
        if(items[index] != null)
        {
            if (items[index].GetComponent<Shotgun>())
                items[index].GetComponent<Shotgun>().TakeInHands();
            else if (items[index].GetComponent<Detector>())
                items[index].GetComponent<Detector>().HandsTake(false);
            else if (items[index].GetComponent<Flashlight>())
                items[index].GetComponent<Flashlight>().TakeInHands();

            items[index].GetComponent<Rigidbody>().useGravity = true;
            Collider[] colliders = items[index].GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.isTrigger = false;
            }
            items[index].GetComponent<Rigidbody>().isKinematic = false;
            items[index].transform.SetParent(null);
            Transform[] children = items[index].GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                child.gameObject.layer = LayerMask.NameToLayer("IgnoreCollider");
            }
            //items[index].gameObject.transform.position = transform.position + new Vector3(0f, 0.4f, 0f);
            if (items[index].GetComponent<Dropeble>().IsInWalls)
                items[index].gameObject.transform.position = transform.position;
            else items[index].GetComponent<Rigidbody>().AddForce(transform.forward * 150f);
            items[index] = null;
        }
    }
    public void SetIndex()
    {
        if ((Input.GetKeyUp(KeyCode.Alpha1) || ControllerInput.LBDown()) && indexCurrent != 0)
        {
            handAnimator.SetTrigger("SETITEM");
            indexCurrent = 0;
            slotsUI[0].gameObject.SetActive(true);
            slotsUI[1].gameObject.SetActive(false);
        }
        if ((Input.GetKeyUp(KeyCode.Alpha2) || ControllerInput.RBDown()) && indexCurrent != 1)
        {
            handAnimator.SetTrigger("SETITEM");
            indexCurrent = 1;
            slotsUI[1].gameObject.SetActive(true);
            slotsUI[0].gameObject.SetActive(false);
        }
    }
}
