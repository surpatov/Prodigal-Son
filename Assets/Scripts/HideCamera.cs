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
        float mouseX = Input.GetAxis("Mouse X") * GameSettings.MouseSensitivity * Time.deltaTime * (GameSettings.IsXVert ? -1 : 1);
        transform.Rotate(Vector2.up * mouseX * GameSettings.MouseSensitivity);
    }
}