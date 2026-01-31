using UnityEngine;

public class Scrap : Item
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip creakClip;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void Screak()
    {
        source.PlayOneShot(creakClip);
    }
}
