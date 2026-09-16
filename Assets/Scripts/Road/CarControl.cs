using UnityEngine;

public class CarControl : MonoBehaviour
{
    [Header("Car Properties")]
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float steeringRange = 30f;
    public float steeringRangeAtMaxSpeed = 10f;
    public float centreOfGravityOffset = -1f;
    public Transform steeringWheel;
    public bool isCarAutomatic = false;

    [Header("Steering Wheel")]
    public float maxSteeringWheelAngle = 180f;   // максимальный угол поворота руля
    public float steeringWheelSpeed = 300f;      // скорость вращения руля (град/сек)

    private WheelControl[] wheels;
    private Rigidbody rigidBody;
    private float currentSteeringWheelAngle = 0f;

    private Quaternion initialSteeringWheelRotation;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        Vector3 centerOfMass = rigidBody.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rigidBody.centerOfMass = centerOfMass;

        wheels = GetComponentsInChildren<WheelControl>();

        if (steeringWheel != null)
            initialSteeringWheelRotation = steeringWheel.localRotation;
    }

    void FixedUpdate()
    {
        float vInput;
        if (isCarAutomatic) vInput = 1f;
        else vInput = Input.GetAxis("Vertical");

        float hInput = Input.GetAxis("Horizontal");

        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed));

        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
            }
        }

        UpdateSteeringWheel(hInput);
    }

    void UpdateSteeringWheel(float hInput)
    {
        if (steeringWheel == null) return;

        float targetAngle = hInput * maxSteeringWheelAngle;
        currentSteeringWheelAngle = Mathf.MoveTowards(
            currentSteeringWheelAngle,
            targetAngle,
            steeringWheelSpeed * Time.fixedDeltaTime
        );

        // Сохраняем исходный поворот и применяем вращение вокруг локальной оси
        steeringWheel.localRotation = initialSteeringWheelRotation;
        steeringWheel.Rotate(-Vector3.forward, currentSteeringWheelAngle, Space.Self);
    }
}