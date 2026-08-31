using UnityEngine;

public class CardControl : MonoBehaviour, IZoomable
{
    [SerializeField] private Collider wallCollider;
    [SerializeField] private Transform pointCardTo;

    [SerializeField] private float timeToOpen;
    [SerializeField] private float timeLeft;
    [SerializeField] private float speed;

    [SerializeField] private Item card;
    [SerializeField] private Inventory inventory;

    [SerializeField] private int idToOpen;
    [SerializeField] private Material sphereMaterialGreen;
    [SerializeField] private Renderer sphere;

    private void Start()
    {
        timeLeft = timeToOpen;
    }
    public void OpenDoor(Inventory _inventory, Item _card)
    {
        if(_card.GetID != idToOpen) return;

        _inventory.canRoot = false;
        inventory = _inventory;
        card = _card;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            SetRoot();
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            sphere.material = sphereMaterialGreen;
            wallCollider.enabled = false;
        }
    }
    private void SetRoot()
    {
        card.GetComponent<Collider>().enabled = false;
        var step = speed * 2 * Time.deltaTime;
        var rotStep = speed * 200 * Time.deltaTime;
        card.transform.position = Vector3.MoveTowards(card.transform.position, pointCardTo.transform.position, step);
        card.transform.rotation = Quaternion.RotateTowards(card.transform.rotation, pointCardTo.transform.rotation, rotStep);
    }
}
