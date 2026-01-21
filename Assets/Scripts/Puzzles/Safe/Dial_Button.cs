using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial_Button : MonoBehaviour, IZoomable
{
    [SerializeField] Dial_Safe dial;
    [SerializeField] enum Appointment
    {
        valuable,
        reset,
        accept
    }
    [SerializeField] private Appointment appoitment;
    [SerializeField] private string value;
    public void Click()
    {
        switch (appoitment)
        {
            case Appointment.valuable:
                dial.SetValue(value);
                break;

                case Appointment.reset:
                dial.ResetValue();
                break;

            case Appointment.accept:
                dial.AcceptValue();
                break;
        }
    }
    private void Start()
    {
        dial = GetComponentInParent<Dial_Safe>();
    }
}