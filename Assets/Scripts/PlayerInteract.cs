using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject holdObject;
    [SerializeField] PlayerController playerController;
    [SerializeField] Animator CameraObject;
    [SerializeField] Inventory inventory;
    [SerializeField] float doorForce;
    [SerializeField] GameObject cam;
    RaycastHit hit;
    [SerializeField] Transform hand_door;
    [SerializeField] public Vector3 offset;

    [SerializeField] AudioSource source;

    [SerializeField] AudioClip openComodeClip;
    [SerializeField] AudioClip openCupboardClip;
    int layerMaskOnlyPlayer;
    int layerMaskWithoutPlayer;
    [SerializeField] Transform impactPlace;
    [SerializeField] Quaternion impactRotation;
    [SerializeField] Camera[] cameras;
    [SerializeField] GameObject playerObject;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject mainUI;

    [SerializeField] private Slider progressBar;
    private HidingPlace currentHidingPlace;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        layerMaskOnlyPlayer = 1 << 6;
        layerMaskWithoutPlayer = ~layerMaskOnlyPlayer;
    }
    private void Update()
    {
        MenuViewer();
        offset = playerController.interactOffset;
        Debug.DrawRay(transform.position + offset, transform.forward, Color.red);

        DragAndDropItems();

        if (Physics.Raycast(transform.position + offset, transform.forward, out hit, 2f, layerMaskWithoutPlayer))
            HandleRaycastHit();
        else CameraObject.SetBool("isZoom", false);

        if (Input.GetKey(KeyCode.F))
            inventory.DropItem(inventory.GetCurrentIndex());
    }

    private void HandleRaycastHit()
    {
        // Проверка на увеличение
        bool isZooming = hit.collider.gameObject.TryGetComponent(out IZoomable zoomable) ? true : false;
        CameraObject.SetBool("isZoom", isZooming);

        // Основные взаимодействия по нажатию мыши
        bool a = !inventory.GetCurrentItem?.GetComponent<Shotgun>();
        bool b = !inventory.GetCurrentItem;
        if (b || !b && a)
        {
            if (Input.GetMouseButton(0))
                HandleLeftMouseButton();

            if (Input.GetMouseButtonUp(0))
                HandleLeftMouseButtonUp();

            if (Input.GetMouseButtonDown(0))
                HandleLeftMouseButtonDown();

            if (Input.GetMouseButton(1))
                HandleRightMouseButton();

            if (Input.GetMouseButtonDown(1))
                HandleRightMouseButtonDown();
        }
    }

    private void HandleLeftMouseButton()
    {
        // Работа с гвоздем
        if (TryGetComponent<WoodNail>(out var woodNail))
            woodNail.Open(inventory.GetCurrentItem, inventory);
        else if (TryGetComponent<Note>(out var note))
        {
            if (note.IsInvinsible)
                inventory.GetCurrentItem?.GetComponent<Iron>().Press(note, inventory);
        }
        else ResetInventoryState();

        // Работа с замком
        if (TryGetComponent<KeyholeControl>(out var keyholeControl))
            keyholeControl.Open(playerController, inventory.GetCurrentItem, inventory);
        if(TryGetComponent<ValveControl>(out var valveControl))
            valveControl.Rotating(playerController, inventory.GetCurrentItem, inventory);

        if (TryGetComponent<Rope>(out var rope))
            rope.Cut(inventory, inventory.GetCurrentItem.GetComponent<Knife>());
        if (TryGetComponent<CardControl>(out var cardControl))
            cardControl.OpenDoor(inventory, inventory.GetCurrentItem);
        if (TryGetComponent<BarrelControl>(out var barrelControl))
            barrelControl.FillBucket(inventory, inventory.GetCurrentItem.GetComponent<Bucket>());

        if (TryGetComponent<HidingPlace>(out var hidingPlace))
        {
            hidingPlace.GettingIn(cameras, playerObject, GetComponent<TextView>().GetTextItem);
            currentHidingPlace = hidingPlace;
            progressBar.gameObject.SetActive(true);
            progressBar.value = hidingPlace.GetProcessing();
        }

        // Взаимодействие с дверями по тегам
        switch (hit.collider.tag)
        {
            case "Door_Front":
            case "Door_Back":
                OpenDoor(hit.collider, 1);
                break;

            case "Door":
                MoveDoorToHand(hit.collider);
                break;
        }
    }

    private void HandleLeftMouseButtonUp()
    {
        if (TryGetComponent<KeyholeControl>(out var keyholeControl) || TryGetComponent<ValveControl>(out var valveControl))
        {
            playerController.canWalk = true;
            playerController.canRotation = true;
        }
        if (!inventory.IsEmpty)
        {
            ResetInventoryState();
        }
        progressBar.gameObject.SetActive(false);
        currentHidingPlace?.StopGettinIn();
    }

    private void HandleLeftMouseButtonDown()
    {
        // Взаимодействие с объектами по тегам
        switch (hit.collider.tag)
        {
            case "Item":
                inventory.PickUpItem(hit.collider.GetComponentInParent<Item>());
                hit.collider.GetComponentInParent<Item>().isTook = true;
                break;

            case "Comode":
                ToggleAnimation("isOpen", openComodeClip);
                break;

            case "Cupboard_door":
                ToggleAnimation("isOpen", openCupboardClip);
                break;

            case "CarBigDoor":
                hit.collider.GetComponent<Animator>().SetTrigger("OPEN");
                break;
        }
        if (TryGetComponent<Breakeble>(out var breakeble))
        {
            inventory.GetCurrentItem?.GetComponent<Hammer>().Hit(breakeble);
        }
        if(TryGetComponent<DadsButton>(out var dadsButton))
        {
            dadsButton.Open();
        }
        if (TryGetComponent<ToiletTrigger>(out var trigger))
        {
            trigger.Activate();
        }
        // Взаимодействие с компонентами
        HandleComponentInteractions();

        if (inventory.GetCurrentItem?.GetComponent<Bucket>())
            inventory.GetCurrentItem.GetComponent<Bucket>().DropWater(hit.collider.gameObject);
    }

    private void HandleRightMouseButton()
    {
        if (hit.collider.tag == "Door_Front" || hit.collider.tag == "Door_Back")
            OpenDoor(hit.collider, -1);
    }

    private void HandleRightMouseButtonDown()
    {
        if(hit.collider.GetComponent<Alarm>())
            hit.collider.GetComponent<Alarm>().Use();


        // Взаимодействие с ящиком для патронов
        if (TryGetComponent<Slees_box>(out var sleesBox) && inventory.CountSellsNotNull() < inventory.maxBullets)
        {
            sleesBox.SpawnSlee();
            inventory.currentSell = sleesBox.GetCurrentSlee();
            inventory.bullets[inventory.CountSellsNotNull()] = inventory.currentSell;
        }
    }

    private void HandleComponentInteractions()
    {
        // Вызов методов у различных компонентов
        TryCallComponentMethod<Dial_Button>(c => c.Click());
        TryCallComponentMethod<RotateLock>(c => c.Rotate());
        TryCallComponentMethod<Televisor>(c => c.Turn(inventory.GetCurrentItem, inventory));
        TryCallComponentMethod<Computer>(c => c.Turn(inventory.GetCurrentItem, inventory));

        if (TryGetComponent<Electrical_switch>(out var electricalSwitch))
        {
            bool isOn = !electricalSwitch.GetComponent<Animator>().GetBool("isOn");
            electricalSwitch.GetComponent<Animator>().SetBool("isOn", isOn);
            source.PlayOneShot(electricalSwitch.switchOnOff);
            electricalSwitch.Turn();
        }
    }

    // Вспомогательные методы
    private bool TryGetComponent<T>(out T component) where T : Component
    {
        component = hit.collider.GetComponent<T>();
        return component != null;
    }

    private void TryCallComponentMethod<T>(Action<T> action) where T : Component
    {
        if (TryGetComponent<T>(out var component))
            action(component);
    }

    private void ToggleAnimation(string boolName, AudioClip clip)
    {
        var animator = hit.collider.GetComponent<Animator>();
        bool currentState = animator.GetBool(boolName);
        animator.SetBool(boolName, !currentState);

        if (clip != null)
            source.PlayOneShot(clip);
    }

    private void MoveDoorToHand(Collider doorCollider)
    {
        doorCollider.GetComponent<Rigidbody>().MovePosition(
            Vector3.Lerp(doorCollider.transform.position, hand_door.position, 0.3f)
        );
    }

    private void ResetInventoryState()
    {
        inventory.canRoot = true;
        if (inventory.GetCurrentItem != null)
        {
            inventory.GetCurrentItem.GetComponent<Collider>().enabled = true;
            if (inventory.GetCurrentItem.GetName == "Scrap")
            {
                inventory.GetCurrentItem.GetComponent<Animator>().SetBool("isOpen", false);
                inventory.GetCurrentItem.transform.GetChild(0).gameObject.layer = 11;
            }
            if(inventory.GetCurrentItem.GetName == "Iron")
            {
                inventory.GetCurrentItem.GetComponent<Animator>().SetBool("isPressing", false);
            }
            if (inventory.GetCurrentItem.GetComponent<Knife>())
            {
                inventory.GetCurrentItem.GetComponent<Knife>().transform.GetChild(0).gameObject.layer = 11;
            }
        }

    }
    private void MenuViewer()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuUI.gameObject.SetActive(!pauseMenuUI.gameObject.activeInHierarchy);
            gameplayUI.SetActive(!gameplayUI.activeInHierarchy);
            if (!gameplayUI.activeInHierarchy) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; Time.timeScale = 0f; }
            else { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; Time.timeScale = 1f; }
        }
    }
    private void OpenDoor(Collider collider, int direction)
    {
        if (collider.tag == "Door_Front")
            collider.GetComponentInParent<Rigidbody>().AddTorque(0f, -doorForce * direction, 0f, ForceMode.Impulse);
        if (collider.tag == "Door_Back")
            collider.GetComponentInParent<Rigidbody>().AddTorque(0f, doorForce * direction, 0f, ForceMode.Impulse);
    }

    private void DragAndDropItems()
    {
        var t = transform;
        var pressed = Input.GetMouseButton(0);
        if (holdObject)
        {
            var rigidbody = holdObject.GetComponent<Rigidbody>();
            var difference = impactPlace.position - holdObject.transform.position;
            if (!playerController.IsCrounch)
            {
                rigidbody.AddForce(difference * 500);
                holdObject.transform.position = impactPlace.position;
                //rigidbody.MovePosition(impactPlace.position);
            }

            if (!pressed || playerController.IsCrounch)
            {
                rigidbody.drag = 1f;
                rigidbody.useGravity = true;
                holdObject.layer = LayerMask.NameToLayer("Default");
                holdObject = null;
            }

        }
        else if (pressed)
        {
            if (Physics.Raycast(transform.position + offset, transform.forward, out hit, 2f, layerMaskWithoutPlayer))
            {
                if (hit.collider.tag == "Pickapable" && !playerController.IsCrounch)
                {
                    impactPlace.position = hit.collider.transform.position; // можно доработать
                    holdObject = hit.transform.gameObject;
                    var rigidbody = holdObject.GetComponent<Rigidbody>();
                    rigidbody.drag = 25f;
                    rigidbody.useGravity = false;
                    holdObject.layer = LayerMask.NameToLayer("IgnoreCollider");
                }
            }
        }

    }
    public void DoFootStep()
    {
        playerController.FootStep();
    }
}
