using UnityEngine;

public class DoorSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] doorClips;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private bool canPlaying;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(!IsMoving())
            canPlaying = true;
        else PlaySound();
    }
    private bool IsMoving()
    {
        return Mathf.Abs(rb.angularVelocity.y) > 0;
    }
    private void PlaySound()
    {
        if (canPlaying)
        {
            int index = Random.Range(0, doorClips.Length);
            source.PlayOneShot(doorClips[index]);
            canPlaying = false;
        }
    }
}