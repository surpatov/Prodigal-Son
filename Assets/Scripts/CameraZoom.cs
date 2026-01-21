using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomCurrent;
    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;
    [SerializeField] private float zoomSpeed;
    private bool canZoom;
    [SerializeField] private Camera cameraHead;

    void Start()
    {
        canZoom = true;
        zoomCurrent = zoomMin;
        cameraHead = GetComponent<Camera>();
    }
    void Update()
    {
        cameraHead.focalLength = zoomCurrent;
        if(canZoom)
            ZoomChange();
    }
    private void ZoomChange()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (zoomCurrent <= zoomMax)
                zoomCurrent += zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (zoomCurrent >= zoomMin)
                zoomCurrent -= zoomSpeed * Time.deltaTime;
        }
    }
}
