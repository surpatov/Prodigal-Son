using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateLock : MonoBehaviour, IZoomable
{
    public static event Action<string, int> Rotated = delegate { };

    private bool coroutineAllowed;
    [SerializeField] private int numberShown;
    [SerializeField] private float rotateAngle;

    private void Start()
    {
        coroutineAllowed = true;
        numberShown = 0;
    }

    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;
        float tempAngle = rotateAngle / 11;

        for (int i = 0; i < 11; i++)
        {
            transform.Rotate(0f, 0f, tempAngle);
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;
        numberShown += 1;

        if(numberShown > 9)
            numberShown = 0;

        Rotated(name, numberShown);
    }
    public void Rotate()
    {
        StartCoroutine("RotateWheel");
    }
}