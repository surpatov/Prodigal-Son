using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dial_Safe : MonoBehaviour, IZoomable
{
    [SerializeField] Text value;
    [SerializeField] int length;
    [SerializeField] private string code;
    [SerializeField] private Collider lockCollider;
    [SerializeField] private Collider[] buttons;
    [SerializeField] public bool isWorkin;

    public void SetValue(string symb)
    {
        if (value.text.Length + 1 <= length && isWorkin)
        {
            value.text += symb;
        }
    }
    public void ResetValue()
    {
        if(isWorkin)
            value.text = "";
    }
    public void AcceptValue()
    {
        if(value.text == code && isWorkin)
        {
            lockCollider.enabled = false;
            value.text = "open";
            GetComponent<Collider>().enabled = false;
            foreach(var button in buttons)
            {
                button.enabled = false;
            }
        }
    }
}