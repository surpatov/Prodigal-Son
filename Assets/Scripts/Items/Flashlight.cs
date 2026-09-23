using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class Flashlight : Item
{
    private bool isInHands;
    public void TakeInHands() { isInHands = !isInHands; }
    [SerializeField] public AudioSource source;
    [SerializeField] private AudioClip turnClip;
    [SerializeField] private Light spotLight;

    private void Update()
    {
        if (isInHands)
        {
            if (Input.GetKeyDown(KeyCode.G) || ControllerInput.YDown()) 
            {
                source.PlayOneShot(turnClip, GameSettings.SoundsVolume);
                spotLight.enabled = !spotLight.enabled;
            }
        }
    }
}