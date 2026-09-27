using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCamera : MonoBehaviour
{
    [SerializeField] private float maxAngle;
    [SerializeField] private HidingPlace hideCamera;
    private void Update()
    {
        RotateCamera();
        if (Input.GetMouseButtonDown(1) || ControllerInput.BDown()) { hideCamera.GetOut(); }
    }
    private void RotateCamera()
    {
        // === Mouse input ===
        float mouseX = Input.GetAxis("Mouse X") * GameSettings.MouseSensitivity * Time.deltaTime * (GameSettings.IsXVert ? -1 : 1);

        // === Gamepad input ===
        float gamepadX = Input.GetAxis("Right Stick X") * GameSettings.MouseSensitivity; // or "Gamepad Right Stick X"
        gamepadX = ApplyDeadzone(gamepadX, 0.15f);
        gamepadX *= 1f * Time.deltaTime * (GameSettings.IsXVert ? -1 : 1); // gamepadSensitivity * Time.deltaTime

        // === Combine both inputs ===
        float totalX = mouseX + gamepadX;

        // Apply rotation
        transform.Rotate(Vector2.up * totalX * GameSettings.MouseSensitivity);
    }
    private float ApplyDeadzone(float value, float deadzone)
    {
        if (Mathf.Abs(value) < deadzone) return 0f;
        float sign = Mathf.Sign(value);
        return sign * ((Mathf.Abs(value) - deadzone) / (1f - deadzone));
    }
}