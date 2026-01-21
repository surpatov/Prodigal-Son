using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrical_switch : MonoBehaviour, IZoomable
{
    private bool isOn;
    public void Turn() { isOn = !isOn; }
    public bool GetValue() => isOn;
    public AudioClip switchOnOff;
}