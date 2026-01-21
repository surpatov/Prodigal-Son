using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlanksWall : MonoBehaviour
{
    [SerializeField] Collider wallCollider;
    [SerializeField] WoodPlank[] planks;
    private void Update()
    {
        bool flag = false;
        foreach(var plank in planks)
        {
            if (!plank.isOpen)
                flag = true;
        }
        if (!flag)
            wallCollider.enabled = false;
    }
}