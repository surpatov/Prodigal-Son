using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCamera : MonoBehaviour
{
    [SerializeField] private float sensivityRotation;
    [SerializeField] private float maxAngle;
    [SerializeField] private HidingPlace hideCamera;
    private void Update()
    {
        RotateCamera();
        if (Input.GetMouseButtonDown(1)) { hideCamera.GetOut(); }
    }
    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensivityRotation * Time.deltaTime;
        transform.Rotate(Vector2.up * mouseX * sensivityRotation);
    }
}