using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int ID;
    [SerializeField] protected string Name;
    [SerializeField] protected Vector3 rootPosition;
    [SerializeField] protected Quaternion rootRotation;

    [SerializeField] public bool isTook;
    [SerializeField] protected Transform shelfParent;
    protected Vector3 offset;

    public Vector3 GetRootPosition => rootPosition;
    public Quaternion GetRootRotation => rootRotation;
    public int GetID => ID;
    public string GetName => Name;
    public virtual void Use() { }

    protected virtual void Start()
    {
        if(shelfParent != null)
            offset = transform.position - shelfParent.position;
    }
    protected virtual void Update()
    {
        if (!isTook && shelfParent != null)
        {
            transform.position = shelfParent.position + offset;
        }
    }
}

