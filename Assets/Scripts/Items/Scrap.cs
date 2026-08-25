using UnityEngine;

public class Scrap : Item
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip creakClip;

    protected override void Start()
    {
        base.Start();
        source = GetComponent<AudioSource>();
    }
    public void Screak()
    {
        source.PlayOneShot(creakClip);
    }
}
