using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextView : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private TextMeshProUGUI textItem;
    public TextMeshProUGUI GetTextItem => textItem;
    [SerializeField] private TextMeshProUGUI textNote;
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
            if (hit.collider.gameObject.GetComponent<Item>()) { textItem.text = hit.collider.gameObject.GetComponent<Item>().GetName + " [LMB]"; }
            else if (hit.collider.tag == "Pickapable") { textItem.text = "drag [LMB]"; }
            else if(hit.collider.tag == "Door_Front" || hit.collider.tag == "Door_Back") { textItem.text = "push\n[LMB]/[RMB]"; } 
            else if(hit.collider.tag == "Cupboard_door" || hit.collider.tag == "Comode")
            {
                if (hit.collider.GetComponent<Animator>().GetBool("isOpen")) { textItem.text = "close [LMB]"; }
                else { textItem.text = "open [LMB]"; }
            }
            else if (hit.collider.GetComponent<Note>()) { textNote.text = hit.collider.GetComponent<Note>().GetMessageText; }
            else if (hit.collider.GetComponent<HidingPlace>()) { textItem.text = "hide [LMB]"; }
            else { textItem.text = ""; textNote.text = ""; }
        }
        else { textItem.text = ""; textNote.text = ""; }

        if (inventory.GetCurrentItem) { textDrop.text = "drop " + inventory.GetCurrentItem.GetComponent<Item>().GetName + " [F]"; }
        else { textDrop.text = ""; }
    }
}