using UnityEngine;

public class Hammer : Item
{
    [SerializeField] private Breakeble glassObject;
    public void Hit(Breakeble glass)
    {
        GetComponent<Animator>().Play("hammer_hit");
        glassObject = glass;
    }
    public void HitEvent()
    {
        glassObject.SpawnPieces();
    }
}
