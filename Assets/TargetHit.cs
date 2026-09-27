using UnityEngine;

public class TargetHit : MonoBehaviour
{
    [Header("Сила отскока")]
    [SerializeField] private float force = 2f;
    [SerializeField] private float torqueForce = 15f;

    [Header("Разброс направления")]
    [SerializeField] private float randomSpread = 0.4f; // насколько рандомно в стороны
    [SerializeField] private float upwardBias = 0.5f;    // насколько вверх отлетает

    [SerializeField] private AudioClip[] bulletHitClips;

    [SerializeField] private ParticleSystem[] particles;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // пока не попали — висит на месте
    }

    public void OnHit(Vector3 hitPoint, Vector3 shootDirection)
    {
        AudioSource.PlayClipAtPoint(bulletHitClips[Random.Range(0, bulletHitClips.Length - 1)], transform.position, GameSettings.SoundsVolume);
        foreach(var particle in particles) particle.Play();

        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 randomDir = Random.insideUnitSphere * randomSpread;
        Vector3 dir = (shootDirection.normalized + Vector3.up * upwardBias + randomDir).normalized;

        rb.AddForceAtPosition(dir * force, hitPoint, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * torqueForce;

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }
}