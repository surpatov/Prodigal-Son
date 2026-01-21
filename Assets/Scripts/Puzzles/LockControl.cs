using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockControl : MonoBehaviour, IZoomable
{
    private int[] result;
    [SerializeField] private int[] correctCombination;
    [SerializeField] Animator animatorStck;
    [SerializeField] private Collider colliderWall;
    private void Start()
    {
        result = new int[] { 0, 0, 0, 0};
        RotateLock.Rotated += CheckResults;
    }

    private void CheckResults(string wheelName, int number)
    {
        switch (wheelName)
        {
            case "wheel_1":
                result[0] = number;
                break;

            case "wheel_2":
                result[1] = number;
                break;

            case "wheel_3":
                result[2] = number;
                break;

            case "wheel_4":
                result[3] = number;
                break;
        }
        if(result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2] && result[3] == correctCombination[3])
            Open();
    }

    private void OnDestroy()
    {
        RotateLock.Rotated -= CheckResults;
    }
    private void Open()
    {
        animatorStck.SetTrigger("OPEN");
        colliderWall.enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}