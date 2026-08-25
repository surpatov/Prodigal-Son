using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : Item, ISoundable, IClikable
{
    private bool canRing;
    [SerializeField] private Monster monster;
    public AudioClip Sound { get { return ringingSound; } set { ringingSound = value; } }
    [SerializeField] private AudioClip ringingSound;
    [SerializeField] private AudioClip tickingSound;
    [SerializeField] private AudioSource source;
    public float TimeToSound { get { return timeTicking; } set { timeTicking = value; } }
    [SerializeField] private float timeTicking;
    protected override void Start()
    {
        base.Start();
        monster = FindObjectOfType<Monster>();
        canRing = true;
    }
    public virtual IEnumerator MakeSound() 
    {
        Debug.Log("ALARM: START TICKING");
        canRing = false;
        source.PlayOneShot(tickingSound);
        yield return new WaitForSeconds(timeTicking);
        source.Stop();
        monster?.SoundTrigger(gameObject);
        canRing = true;
        source.PlayOneShot(Sound);
    }
    public override void Use()
    {
        if(canRing)
            StartCoroutine(MakeSound());
    }
}