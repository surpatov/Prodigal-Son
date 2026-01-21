using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour, IZoomable
{
    [SerializeField] private bool isOpen;
    [SerializeField] private List<int> canOpen;
    public void Open(int itemID)
    {
        if (!isOpen && canOpen.Contains(itemID))
        {

        }
    }
}