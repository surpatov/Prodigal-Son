using UnityEngine;
using System.Collections;

public class Bucket : Item
{
    [SerializeField] private GameObject water;
    public bool HasWater => water?.activeSelf ?? false;
    [SerializeField] private Animator animator;
    public void FillWater()
    {
        water?.SetActive(true);
    }
    public void DropWater(GameObject hit)
    {
        if (!water.activeInHierarchy) return;

        audioSource.PlayOneShot(dropWaterClip);
        animator.SetTrigger("DROP");

        StartCoroutine(Activate(hit));
    }
    public IEnumerator Activate(GameObject _hit)
    {
        yield return new WaitForSeconds(0.02f);

        water?.SetActive(false);

        if (_hit == null) yield return null;

        if (_hit.CompareTag("WaterDamaged"))
        {
            _hit.GetComponent<Collider>().enabled = false;
            _hit.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    AudioSource audioSource;
    [SerializeField] private AudioClip dropWaterClip;
    [SerializeField] private AudioClip fillWaterClip;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 2f)
        {
            water?.SetActive(false);
            audioSource.PlayOneShot(dropWaterClip);
        }
    }
}
