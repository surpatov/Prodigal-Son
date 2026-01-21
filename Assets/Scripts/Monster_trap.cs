using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_trap : MonoBehaviour, ISoundable
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip crackSound;
    public AudioClip Sound { get { return crackSound; } set { crackSound = value; } }
    public float TimeToSound { get; set; }
    [SerializeField] private Monster monster;
    void Start()
    {
        monster = FindObjectOfType<Monster>();
    }
    public void MakeSound()
    {
        source.PlayOneShot(crackSound);
        monster.SoundTrigger(gameObject);
    }
}