using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuUI canvas;
    [SerializeField] private int thisId;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip selectClip;

    public void OnMouseEnter()
    {
        if (canvas.currentItem != thisId)
            source.PlayOneShot(selectClip);
        canvas.currentItem = thisId;
    }
}