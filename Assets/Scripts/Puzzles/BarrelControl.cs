using UnityEngine;

public class BarrelControl : MonoBehaviour, IZoomable
{
    [SerializeField] private Transform pointBucketTo;

    [SerializeField] private float timeToFill;
    [SerializeField] private float timeLeft;
    [SerializeField] private float speed;

    [SerializeField] private Bucket bucket;
    [SerializeField] private Inventory inventory;

    private void Start()
    {
        timeLeft = timeToFill;
    }
    public void FillBucket(Inventory _inventory, Bucket _bucket)
    {
        if (_bucket == null) return;

        _inventory.canRoot = false;
        inventory = _inventory;
        bucket = _bucket;
        bucket.gameObject.layer = LayerMask.NameToLayer("Default");
        bucket.gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            SetRoot();
        }
        else
        {
            bucket.FillWater();
            timeLeft = timeToFill;
            bucket.gameObject.layer = LayerMask.NameToLayer("Hands");
            bucket.gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Hands");
        }
    }
    private void SetRoot()
    {
        bucket.GetComponent<Collider>().enabled = false;
        var step = speed * 2 * Time.deltaTime;
        var rotStep = speed * 200 * Time.deltaTime;
        bucket.transform.position = Vector3.MoveTowards(bucket.transform.position, pointBucketTo.transform.position, step);
        bucket.transform.rotation = Quaternion.RotateTowards(bucket.transform.rotation, pointBucketTo.transform.rotation, rotStep);
    }
}
