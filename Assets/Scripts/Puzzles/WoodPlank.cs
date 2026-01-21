using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlank : MonoBehaviour
{
    public bool isOpen;
    [SerializeField] WoodNail[] nails;
    private void Start()
    {
        isOpen = false;
    }
    private void Update()
    {
        bool isAllOpen = true;
        foreach (var nail in nails)
        {
            if (nail != null)
                isAllOpen = false;
        }
        if (isAllOpen)
            Open();
    }
    private void Open()
    {
        GetComponent<Rigidbody>().useGravity = true;
        isOpen = true;
    }
}
