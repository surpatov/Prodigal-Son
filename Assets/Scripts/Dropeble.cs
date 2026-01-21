using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropeble : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioSource source;
    [SerializeField] private float dangerVelocity;
    private bool isInWalls;
    public bool IsInWalls => isInWalls;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float power = collision.relativeVelocity.magnitude;
        if (power >= dangerVelocity)
            source.PlayOneShot(dropSound);
    }
    private void OnTriggerEnter(Collider other)
    {
        isInWalls = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isInWalls = false;
    }
}