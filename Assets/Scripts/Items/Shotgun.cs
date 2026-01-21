using UnityEngine;

public class Shotgun : Item
{
    [SerializeField] private Transform[] sleeHandlers;
    [SerializeField] public Inventory inventory;
    [SerializeField] public Transform cameraCenter;
    [SerializeField] private Animator animatorMuzzle;
    [SerializeField] private ParticleSystem[] particle;
    [SerializeField] public AudioSource source;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip openMuzzle;
    [SerializeField] private AudioClip closeMuzzle;
    [SerializeField] private AudioClip sleeIn;
    [SerializeField] public Animator playerCamera;
    [SerializeField] private int ammunitionLeft;
    [SerializeField] private int maxAmmunation;
    [SerializeField] private GameObject bulletHole;
    private bool isMuzzleOpen;
    private bool isInHands;
    private RaycastHit hit;
    [SerializeField] private float spreadLimitAngle;
    [SerializeField] private Transform muzzle;
    public void TakeInHands() { isInHands = !isInHands; }
    [SerializeField] private int sleesCount;
    private void Update()
    {
        DrawSpreadLimit();
        if (isMuzzleOpen)
        {
            Reload();
        }
        if (isInHands)
        {
            //TargetCheck();
        }
        if (Input.GetKeyDown(KeyCode.R) && isInHands)
        {
            if (isMuzzleOpen)
                source.PlayOneShot(closeMuzzle);
            else source.PlayOneShot(openMuzzle);
            animatorMuzzle.SetBool("isOpen", !animatorMuzzle.GetBool("isOpen"));
            isMuzzleOpen = animatorMuzzle.GetBool("isOpen");
        }
        if (Input.GetMouseButtonDown(0) && !isMuzzleOpen && isInHands)
        {
            Shoot();
        }
    }
    private void DrawSpreadLimit()
    {
        Debug.DrawRay(muzzle.position, Quaternion.Euler(0, spreadLimitAngle, 0) * -muzzle.up, Color.green);
        Debug.DrawRay(muzzle.position, Quaternion.Euler(0, -spreadLimitAngle, 0) * -muzzle.up, Color.green);
        Debug.DrawRay(muzzle.position, Quaternion.Euler(spreadLimitAngle, 0, 0) * -muzzle.up, Color.green);
        Debug.DrawRay(muzzle.position, Quaternion.Euler(-spreadLimitAngle, 0, 0) * -muzzle.up, Color.green);
    }
    public int CountSellsNotNull()
    {
        int count = 0;

        foreach (var item in sleeHandlers)
        {
            if (item.childCount != 0)
            {
                count++;
            }
        }
        return count;
    }
    private void BulletHit(RaycastHit hitObject)
    {
        GameObject impact = Instantiate(bulletHole, hitObject.point, Quaternion.LookRotation(hitObject.normal));
        impact.transform.parent = hitObject.transform;
        Destroy(impact, 10f);
    }
    private void Shoot()
    {
        if(ammunitionLeft > 0)
        {
            foreach(var part in particle)
            {
                part.Play();
            }
            animatorMuzzle.SetTrigger("SHOOT");
            source.PlayOneShot(shootClip);
            playerCamera.SetTrigger("SHAKE");
            TargetCheck();
            ammunitionLeft -= 1;
        }
    }
    private void Reload()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (sleeHandlers[0].childCount == 0 && inventory.CountSellsNotNull() != 0)
            {
                var bullet = inventory.bullets[inventory.CountSellsNotNull() - 1].gameObject;
                inventory.bullets[inventory.CountSellsNotNull() - 1] = null;
                if (inventory.CountSellsNotNull() != 0)
                {
                    inventory.currentSell = inventory.bullets[inventory.CountSellsNotNull() - 1].gameObject;
                }
                else inventory.currentSell = null;
                bullet.transform.parent = sleeHandlers[0];
                bullet.layer = LayerMask.NameToLayer("Hands");
                bullet.transform.localPosition = new Vector3(0f, 0f, 0f);
                bullet.transform.localRotation = new Quaternion(90f, 0f, -90f, 0f);
                source.PlayOneShot(sleeIn);

                ammunitionLeft += 1;
            }
            else if (sleeHandlers[1].childCount == 0 && inventory.CountSellsNotNull() != 0)
            {
                var bullet = inventory.bullets[inventory.CountSellsNotNull() - 1].gameObject;
                inventory.bullets[inventory.CountSellsNotNull() - 1] = null;
                if (inventory.CountSellsNotNull() != 0)
                {
                    inventory.currentSell = inventory.bullets[inventory.CountSellsNotNull() - 1].gameObject;
                }
                else inventory.currentSell = null;
                bullet.transform.parent = sleeHandlers[1];
                bullet.layer = LayerMask.NameToLayer("Hands");
                bullet.transform.localPosition = new Vector3(0f, 0f, 0f);
                bullet.transform.localRotation = new Quaternion(90f, 0f, -90f, 0f);
                source.PlayOneShot(sleeIn);

                ammunitionLeft += 1;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (sleeHandlers[0].childCount == 1)
            {
                Destroy(sleeHandlers[0].GetChild(0).gameObject);
                source.PlayOneShot(sleeIn);
                if (ammunitionLeft != 0) { ammunitionLeft -= 1; }
            }
            else if (sleeHandlers[1].childCount == 1)
            {
                Destroy(sleeHandlers[1].GetChild(0).gameObject);
                source.PlayOneShot(sleeIn);
                if (ammunitionLeft != 0) { ammunitionLeft -= 1; }
            }
        }
    }
    private void TargetCheck()
    {
        for (int i = 0; i <= sleesCount; i++)
        {
            float angleX = Random.Range(-spreadLimitAngle, spreadLimitAngle);
            float angleY = Random.Range(-spreadLimitAngle, spreadLimitAngle);
            if (Physics.Raycast(cameraCenter.position, Quaternion.Euler(angleX, angleY, 0) * -muzzle.up, out hit))
            {
                if (!hit.collider.GetComponentInParent<Monster>()) { BulletHit(hit); }

                if (hit.collider.GetComponentInParent<Monster>()) { hit.collider.GetComponentInParent<Monster>().GetDamaged(); }
            }
        }
    }
}