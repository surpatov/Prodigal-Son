using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;

    public void Activate()
    {
        GetComponent<Collider>().enabled = false;
        objectToActivate.SetActive(true);
    }
}
