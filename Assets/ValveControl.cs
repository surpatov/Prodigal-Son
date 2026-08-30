using UnityEngine;

public class ValveControl : MonoBehaviour, IZoomable
{
    [SerializeField] Inventory inventory;
    [SerializeField] Transform targetPos;
    [SerializeField] Quaternion targetRot;
    private bool isValved;
    [SerializeField] int openID;
    [SerializeField] float goalRotation;
    [SerializeField] private Collider toiletTriggerCollider;
    PlayerController playerController;

    [SerializeField] Transform parentPoint;
    Vector3 offset;

    Item key;
    public float speed = 1.0f;

    private void Start()
    {
        if (parentPoint == null) return;
        offset = transform.position - parentPoint.position;
    }
    private void Update()
    {
        if (key != null)
            key.transform.position = transform.position;

        if (isValved)
            SetRoot();
        if (goalRotation - transform.localEulerAngles.z < 1f)
        {
            if(toiletTriggerCollider != null)
                toiletTriggerCollider.enabled = false;
            GetComponent<Collider>().enabled = false;
            playerController.canWalk = true;
            playerController.canRotation = true;
        }

        if (parentPoint == null) return;
        transform.position = parentPoint.position + offset;
    }
    public void Rotating(PlayerController playerControl, Item item, Inventory inv)
    {
        if (item != null)
        {
            if (item.GetID == openID && !isValved)
            {
                inventory = inv;
                key = item;
                isValved = true;
                inventory.DropItem(inventory.GetCurrentIndex());
                key.GetComponent<Rigidbody>().useGravity = false;
                key.GetComponent<Rigidbody>().isKinematic = true;
                key.gameObject.layer = 0;
                key.GetComponent<Collider>().enabled = false;
                key.gameObject.transform.parent = transform;
            }
        }
        if (isValved)
        {
            playerController = playerControl;
            playerControl.canWalk = false;
            playerControl.canRotation = false;

            float direction = Input.GetAxis("Mouse X");
            float newRotation = Mathf.Clamp(direction * Time.deltaTime * 700f, 0f, 90f);

            transform.Rotate(new Vector3(0f, 0f, 1f), newRotation);
        }
    }
    private void SetRoot()
    {
        var step = speed * 4 * Time.deltaTime;
        var rotStep = speed * 400 * Time.deltaTime;
        key.transform.position = Vector3.MoveTowards(key.transform.position, targetPos.position, step);
        key.transform.localRotation = Quaternion.RotateTowards(key.transform.localRotation, targetRot, rotStep);
    }
}
