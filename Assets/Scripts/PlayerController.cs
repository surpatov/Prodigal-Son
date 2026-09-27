using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private PostProcessProfile profile;
    private MotionBlur blur;
    private DepthOfField depth;
    public bool IsCrounch => isCrounch;
    [SerializeField] public Vector3 interactOffset;
    [SerializeField] public PlayerInteract interact;
    [SerializeField] private GameObject anchor;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] footStepClips;
    [SerializeField] private float speedMoveCurrent;
    [SerializeField] private float speedMoveMin;
    [SerializeField] private float speedMoveMax;
    [SerializeField] private float speedMoveSneak;
    [SerializeField] private float sensivityRotation;
    [SerializeField] private float maxAngle;
    [SerializeField] private float stamina;
    [SerializeField] private float staminaMax;
    public bool canWalk;
    public bool canRotation;
    public bool canInteract;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cameraHead;
    [SerializeField] public GameObject cameraObject; //
    public GameObject CameraHead => cameraHead.gameObject;
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private Animator animatorCamera;
    [SerializeField] private bool isCrounch;
    Vector3 newDirection;
    float moveX;
    float moveY;
    [SerializeField] Vector3 offset;
    [SerializeField] float lerpRotation;
    [SerializeField] GameObject hand;
    [SerializeField] Vector3 offsetHands;
    [SerializeField] private bool canUpStamina;
    [SerializeField] private bool isUpNotFree;
    [SerializeField] private Slider sliderStamina;
    [SerializeField] public Canvas laterCanvas;
    [SerializeField] public Canvas canvas;
    [SerializeField] public Canvas creditsCanvas;
    [SerializeField] public Slider progressBarHold;
    [SerializeField] public Transform holdMenu;
    [SerializeField] public Transform gameplayMenu;

    [Header("Gamepad Settings")]
    [SerializeField] private float gamepadSensitivity = 1f; // Чувствительность геймпада
    [SerializeField] private float gamepadDeadzone = 0.15f;   // Мёртвая зона стика
    [SerializeField] private string gamepadXAxis = "Right Stick X"; // Или "Gamepad Right Stick X" 
    [SerializeField] private string gamepadYAxis = "Right Stick Y";

    private int tempIndex;

    private float xRotation = 0f;
    private float yRotation = 0f;
    void Start()
    {
        stamina = staminaMax;
        //isCrounch = false;
        speedMoveCurrent = speedMoveMin;
        //controller = GetComponent<CharacterController>();
        //canWalk = true;
        //canRotation = true;
        sliderStamina.maxValue = staminaMax;
        sliderStamina.minValue = 0f;
        //canInteract = true;

        //profile.TryGetSettings(out depth);
        profile.TryGetSettings(out blur);
        blur.active = false;
    }
    private void Update()
    {
        sensivityRotation = GameSettings.MouseSensitivity;
        sliderStamina.value = stamina;
        hand.transform.position = cameraObject.transform.position + offsetHands;
        hand.transform.rotation = cameraObject.transform.rotation;

        StaminaChange();

        if (canWalk)
        {
            PlayerMove();
            SpeedChange();
            Sneak();
        }
        if (canRotation)
        {
            PlayerRotation();
            AnimatorChange();
        }

        if (IsMonsterNearby())
        {
            //depth.active = true;
            blur.active = true;
        }
        else
        {
            //depth.active = false;
            blur.active = false;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Door_Front")
        {
            hit.gameObject.GetComponentInParent<Rigidbody>().AddTorque(0f, -1f, 0f, ForceMode.Force);
        }
        else if (hit.collider.tag == "Door_Back")
        {
            hit.gameObject.GetComponentInParent<Rigidbody>().AddTorque(0f, 1f, 0f, ForceMode.Force);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Monster_trap>())
        {
            other.GetComponent<Monster_trap>().MakeSound();
        }

        if (other.GetComponent<TutorialRoom>())
        {
            other.GetComponent<TutorialRoom>().Enter();
        }
        if(other.GetComponent<BasementOutro>())
        {
            other.GetComponent<BasementOutro>().Activate(this);
        }
        if(other.GetComponent<FinishOutro>())
        {
            other.GetComponent<FinishOutro>().Activate(this);
        }
        if(other.GetComponent<ActivateTrigger>())
        {
            other.GetComponent<ActivateTrigger>().Activate();
        }
    }
    private void StaminaChange()
    {
        if(speedMoveCurrent == speedMoveMax && (moveX != 0 || moveY != 0))
        {
            if(stamina > 0)
            {
                if(stamina - Time.deltaTime/2 > 0)
                    stamina -= Time.deltaTime/2;
                else stamina = 0;
            }
        }
        else
        {
            if(stamina < staminaMax && speedMoveCurrent != speedMoveMax && canUpStamina)
            {
                if (stamina + Time.deltaTime/2 < staminaMax)
                    stamina += Time.deltaTime/2;
                else stamina = staminaMax;
            }
                
        }
    }
    private void SpeedChange()
    {
        if (Input.GetKey(KeyCode.LeftShift) || ControllerInput.L3())
        {
            canUpStamina = false;
            if (!isCrounch) // если игрок стоит
            {
                if (stamina > 0 && !isUpNotFree)
                {
                    speedMoveCurrent = speedMoveMax;
                    animatorPlayer.SetBool("isCrounch", false);
                }
                else if (!isUpNotFree)
                {
                    speedMoveCurrent = speedMoveMin;
                }

                else
                {
                    speedMoveCurrent = speedMoveSneak;
                }

            }
            else 
            {
                if(!isUpNotFree)
                    isCrounch = false;
            }

        }
        else if ((!Input.GetKey(KeyCode.LeftShift) || ControllerInput.L3()) && !isCrounch)
        {
            speedMoveCurrent = speedMoveMin;
        }


        if (!Input.GetKey(KeyCode.LeftShift) || !ControllerInput.L3())
        {
            canUpStamina = true;
        }


        if (Input.GetKeyDown(KeyCode.LeftControl) || ControllerInput.R3Down())
        {
            if (!isCrounch)
            {
                speedMoveCurrent = speedMoveSneak;
                animatorPlayer.SetBool("isCrounch", true);
            }
            else
            {
                if (!isUpNotFree)
                {
                    speedMoveCurrent = speedMoveMin;
                    animatorPlayer.SetBool("isCrounch", false);
                }
            }
        }
    }
    private void PlayerMove()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        newDirection = transform.forward * moveY + transform.right * moveX;
        newDirection.Normalize();
        newDirection.y -= speedMoveCurrent * 2;
        controller.Move(newDirection * speedMoveCurrent * Time.deltaTime);
        //anchor.transform.position = Vector3.MoveTowards(anchor.transform.position, newDirection * speedMoveCurrent * Time.deltaTime, 1f);
        cameraObject.transform.position = controller.transform.position + offset;
    }
    private void PlayerRotation()
    {
        // === Ввод мыши ===
        float mouseX = Input.GetAxis("Mouse X") * GameSettings.MouseSensitivity * Time.deltaTime * (GameSettings.IsXVert ? -1 : 1);
        float mouseY = Input.GetAxis("Mouse Y") * GameSettings.MouseSensitivity * Time.deltaTime * (GameSettings.IsYVert ? -1 : 1);

        // === Ввод геймпада ===
        float gamepadX = Input.GetAxis(gamepadXAxis);
        float gamepadY = Input.GetAxis(gamepadYAxis) * -1;

        // Применяем мёртвую зону
        gamepadX = ApplyDeadzone(gamepadX, gamepadDeadzone);
        gamepadY = ApplyDeadzone(gamepadY, gamepadDeadzone);

        // Геймпад использует "сырое" значение со временем, т.к. стик уже даёт плавное значение
        gamepadX *= gamepadSensitivity * Time.deltaTime * (GameSettings.IsXVert ? -1 : 1);
        gamepadY *= gamepadSensitivity * Time.deltaTime * (GameSettings.IsYVert ? -1 : 1);

        // === Суммируем оба ввода ===
        float totalX = mouseX + gamepadX;
        float totalY = mouseY + gamepadY;

        // === Применяем вращение ===
        transform.Rotate(Vector2.up * totalX * GameSettings.MouseSensitivity);

        xRotation -= totalY * GameSettings.MouseSensitivity;
        yRotation += totalX * GameSettings.MouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -maxAngle, maxAngle);

        cameraObject.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
    private float ApplyDeadzone(float value, float deadzone)
    {
        if (Mathf.Abs(value) < deadzone) return 0f;

        // Перенормировка: плавный переход от deadzone к 1
        float sign = Mathf.Sign(value);
        return sign * ((Mathf.Abs(value) - deadzone) / (1f - deadzone));
    }
    private void AnimatorChange()
    {
        if (moveX != 0f | moveY != 0f)
        {
            if(speedMoveCurrent == speedMoveMax)
            {
                animatorCamera.SetBool("isWalking", false);
                animatorCamera.SetBool("isRunning", true);
            } 
            else
            {
                animatorCamera.SetBool("isWalking", true);
                animatorCamera.SetBool("isRunning", false);
            }
        }
        else
        {
            animatorCamera.SetBool("isWalking", false);
            animatorCamera.SetBool("isRunning", false);
        }
    }
    private void Sneak()
    {
        int layerMaskOnlyPlayer = 1 << 6;
        int layerMaskWithoutPlayer = ~layerMaskOnlyPlayer;
        isUpNotFree = Physics.Raycast(transform.position, new Vector3(0f, 1f, 0f), 1f, layerMaskWithoutPlayer);

        Debug.DrawRay(transform.position, new Vector3(0f, 1f, 0f), Color.green);

        if ((Input.GetKeyDown(KeyCode.LeftControl) || ControllerInput.R3Down()) && (!Input.GetKey(KeyCode.LeftShift) || ControllerInput.L3()))
        {
            if (!isCrounch)
            {
                isCrounch = true;
                animatorPlayer.SetBool("isCrounch", true);
            }

            else 
            {
                if (!isUpNotFree)
                {
                    isCrounch = false;
                    animatorPlayer.SetBool("isCrounch", false);
                }
            }
        }
    }
    public void FootStep()
    {
        if (!isCrounch)
        {
            int index = Random.Range(0, footStepClips.Length);
            if (index == tempIndex) FootStep();
            else
            {
                source.PlayOneShot(footStepClips[index], GameSettings.SoundsVolume);
                tempIndex = index;
            }
        }
    }
    public bool IsMonsterNearby()
    {
        // OverlapSphere возвращает все коллайдеры в радиусе
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayer);

        foreach (var hit in hits)
        {
            // Проверяем, есть ли на объекте (или его родителях) компонент Monster
            if (hit.GetComponentInParent<Monster>() != null)
                return true;
        }
        return false;
    }
}