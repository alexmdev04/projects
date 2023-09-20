using System.Collections.Generic;
using UnityEngine;

public class uiInventory : MonoBehaviour
{
    public static uiInventory instance { get; private set; }
    public bool ifDefaultHideSlot = true;
    [SerializeField] float animOutDistance = -40, animInDistance = -20f, animSpeed = 1f;
    [SerializeField] Color animOutColour, animInColour;
    [SerializeField] List<uiWeaponSlot> uiWeaponSlots;

    void Awake() { instance = this; }
    void Update() 
    { 
        uiInventorySlotSelectAnimation();
    }
    void uiInventorySlotSelectAnimation()
    {
        foreach (uiWeaponSlot slot in uiWeaponSlots)
        {
            if (slot.weaponSlot == Player.instance.weaponSlotEquipped)
            { // out anim
                slot.transform.localPosition = Vector3.Lerp(slot.transform.localPosition, new Vector3(animOutDistance, slot.transform.localPosition.y, slot.transform.localPosition.z), Time.deltaTime * animSpeed);
                Color uiWeaponSlotColor = Color.Lerp(slot.uiWeaponSlotIcon.color, animOutColour, Time.deltaTime * animSpeed);
                slot.uiWeaponSlotIcon.color = uiWeaponSlotColor;
                slot.uiWeaponSlotWeaponName.color = uiWeaponSlotColor;

            }
            else
            { // in anim
                slot.transform.localPosition = Vector3.Lerp(slot.transform.localPosition, new Vector3(animInDistance, slot.transform.localPosition.y, slot.transform.localPosition.z), Time.deltaTime * animSpeed);
                Color uiWeaponSlotColor = Color.Lerp(slot.uiWeaponSlotIcon.color, animInColour, Time.deltaTime * animSpeed);
                slot.uiWeaponSlotIcon.color = uiWeaponSlotColor;
                slot.uiWeaponSlotWeaponName.color = uiWeaponSlotColor;
            }
        }
    }
}