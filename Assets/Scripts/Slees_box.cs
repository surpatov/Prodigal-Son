using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slees_box : MonoBehaviour
{
    [SerializeField] private GameObject slee;
    [SerializeField] private GameObject currentSlee;
    public void SpawnSlee()
    {
        var _slee = Instantiate(slee, transform.position, Quaternion.identity);
        currentSlee = _slee;
    }
    public GameObject GetCurrentSlee()
    {
        var _slee = currentSlee;
        //currentSlee = null;
        //return _slee;
        return currentSlee;
    }
}
