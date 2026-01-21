using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundable
{
    public AudioClip Sound { get; set; }
    public float TimeToSound { get; set; }
    public virtual IEnumerator MakeSound() { yield return new WaitForSeconds(TimeToSound); }
}
