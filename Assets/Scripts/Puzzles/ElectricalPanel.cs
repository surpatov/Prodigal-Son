using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalPanel : MonoBehaviour, IZoomable
{
    [SerializeField] private Electrical_switch[] switches;
    [SerializeField] private bool[] sequence;
    [SerializeField] private bool isOpen;
    [SerializeField] private Dial_Safe entryPhone;
    private void Update()
    {
        CheckSequence();
    }

    private void CheckSequence()
    {
        bool isCurrect = true;
        for (int i = 0; i < 8; i++)
        {
            if (switches[i].GetValue() != sequence[i])
                isCurrect = false;
        }
        if (isCurrect)
        {
            isOpen = true;
            entryPhone.isWorkin = true;
            entryPhone.enabled = true;
        }
        else
        {
            isOpen = false;
            entryPhone.isWorkin = false;
            entryPhone.enabled = false;
        }
    }
}