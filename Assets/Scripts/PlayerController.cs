using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool IsCrounch => isCrounch;
    [SerializeField] public Vector3 interactOffset;
    [SerializeField] public PlayerInteract interact;
    [SerializeField] private GameObject anchor;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip footStepClip;
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
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cameraHead;
    [SerializeField] private GameObject cameraObject;
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

    [SerializeField] private GlobalSettingsSO settings;

    float xRotation = 0f;
    float yRotation = 0f;
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
    }
    private void Update()
    {
        sensivityRotation = settings.mouseSensivity;
        sliderStamina.value = stamina;
        hand.transform.position = cameraObject.transform.position + offsetHands;
        hand.transform.rotation = cameraObject.transform.rotation;

        AnimatorChange();
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
        if (Input.GetKey(KeyCode.LeftShift))
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
        else if (!Input.GetKey(KeyCode.LeftShift) && !isCrounch)
        {
            speedMoveCurrent = speedMoveMin;
        }


        if (!Input.GetKey(KeyCode.LeftShift))
        {
            canUpStamina = true;
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
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
        float mouseX = Input.GetAxis("Mouse X") * sensivityRotation * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensivityRotation * Time.deltaTime;
        transform.Rotate(Vector2.up * mouseX * sensivityRotation);
        xRotation -= mouseY * sensivityRotation;
        yRotation += mouseX * sensivityRotation;
        xRotation = Mathf.Clamp(xRotation, -maxAngle, maxAngle);
        cameraObject.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
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

        if (Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
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
        if(!isCrounch)
            source.PlayOneShot(footStepClip);
    }
}