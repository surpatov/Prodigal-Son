using UnityEngine;

public class Rope : MonoBehaviour, IZoomable
{
    [SerializeField] private Collider wallCollider;
    [SerializeField] private Transform pointKnifeTo;

    [SerializeField] private float timeToCut;
    [SerializeField] private float timeLeft;
    [SerializeField] private float speed;

    [SerializeField] private Knife knife;
    [SerializeField] private Inventory inventory;

    private void Start()
    {
        timeLeft = timeToCut;
    }
    public void Cut(Inventory _inventory, Knife _knife)
    {
        if(knife == null || inventory == null) return;

        _inventory.canRoot = false;
        inventory = _inventory;
        knife = _knife;
        knife.GetComponent<Animator>().Play("cut");
        knife.transform.GetChild(0).gameObject.layer = 0;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            SetRoot();
        }
        else Destroy(gameObject);
    }
    private void SetRoot()
    {
        knife.GetComponent<Collider>().enabled = false;
        var step = speed * 2 * Time.deltaTime;
        var rotStep = speed * 200 * Time.deltaTime;
        knife.transform.position = Vector3.MoveTowards(knife.transform.position, pointKnifeTo.transform.position, step);
        knife.transform.rotation = Quaternion.RotateTowards(knife.transform.rotation, pointKnifeTo.transform.rotation, rotStep);
    }
}
