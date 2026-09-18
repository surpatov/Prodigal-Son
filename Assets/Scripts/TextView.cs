using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextView : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private TextMeshProUGUI textItem;
    public TextMeshProUGUI GetTextItem => textItem;
    [SerializeField] private TextMeshProUGUI textDrop;
    RaycastHit hit;
    int layerMaskOnlyPlayer;
    int layerMaskWithoutPlayer;
    void Start()
    {
        layerMaskOnlyPlayer = 1 << 6;
        layerMaskWithoutPlayer = ~layerMaskOnlyPlayer;

        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position + GetComponent<PlayerInteract>().offset, transform.forward, out hit, 2f, layerMaskWithoutPlayer))
        {
            if (hit.collider.gameObject.GetComponent<Item>()) { textItem.text = hit.collider.gameObject.GetComponent<Item>().GetName; }
            else if (hit.collider.tag == "Pickapable") { textItem.text = "drag"; }
            else if(hit.collider.tag == "Door_Front" || hit.collider.tag == "Door_Back") { textItem.text = "push"; } 
            else if(hit.collider.tag == "Cupboard_door" || hit.collider.tag == "Comode")
            {
                if (hit.collider.GetComponent<Animator>().GetBool("isOpen")) { textItem.text = "close"; }
                else { textItem.text = "open"; }
            }
            else if (hit.collider.GetComponent<Note>()) { textItem.text = "read"; }
            else if (hit.collider.GetComponent<HidingPlace>()) { textItem.text = "hide"; }
            else if(hit.collider.GetComponent<TelephoneIntro>()) { textItem.text = "mute call"; }
            else { textItem.text = ""; }
        }
        else { textItem.text = ""; }

        if (inventory.GetCurrentItem) { textDrop.text = "drop " + inventory.GetCurrentItem.GetComponent<Item>().GetName; }
        else { textDrop.text = ""; }
    }
}