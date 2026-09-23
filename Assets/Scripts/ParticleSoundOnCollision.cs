using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSoundOnCollision : MonoBehaviour
{
    public AudioClip[] collisionSounds;
    private ParticleSystem partSystem;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Awake()
    {
        partSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        collisionEvents.Clear();
        int numCollisionEvents = partSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            if (collisionSounds.Length > 0)
            {
                AudioSource.PlayClipAtPoint(
                    collisionSounds[Random.Range(0, collisionSounds.Length)],
                    collisionEvents[i].intersection
                );
            }
        }
    }
}