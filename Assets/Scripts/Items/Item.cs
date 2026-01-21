using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int ID;
    [SerializeField] protected string Name;
    [SerializeField] protected Vector3 rootPosition;
    [SerializeField] protected Quaternion rootRotation;
    public Vector3 GetRootPosition => rootPosition;
    public Quaternion GetRootRotation => rootRotation;
    public int GetID => ID;
    public string GetName => Name;
    public virtual void Use() { }
}
