using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class uiWeaponSlot : MonoBehaviour
{
    public Player.weaponSlots weaponSlot;
    public Image uiWeaponSlotIcon;
    public TextMeshProUGUI uiWeaponSlotBindingName, uiWeaponSlotWeaponName;
    Weapon weapon;
    InputAction uiWeaponSlotBinding;
    bool uiWeaponSlotIconLoaded;

    void FixedUpdate()
    {
        switch (weaponSlot)
        {
            case Player.weaponSlots.weaponPrimary:
                {
                    weapon = Player.instance.weaponPrimary;
                    uiWeaponSlotBinding = Player.instance.input.Player.Primary_Weapon;
                    break;
                }
            case Player.weaponSlots.weaponSecondary:
                {
                    weapon = Player.instance.weaponSecondary;
                    uiWeaponSlotBinding = Player.instance.input.Player.Secondary_Weapon;
                    break;
                }
            case Player.weaponSlots.weaponMelee:
                {
                    weapon = Player.instance.weaponMelee;
                    uiWeaponSlotBinding = Player.instance.input.Player.Melee_Weapon;
                    break;
                }
            case Player.weaponSlots.weaponEquipment1:
                {
                    weapon = Player.instance.weaponEquipment1;
                    uiWeaponSlotBinding = Player.instance.input.Player.Equipment1;
                    break;
                }
            case Player.weaponSlots.weaponEquipment2:
                {
                    weapon = Player.instance.weaponEquipment2;
                    uiWeaponSlotBinding = Player.instance.input.Player.Equipment1;
                    break;
                }
            case Player.weaponSlots.weaponEquipment3:
                {
                    weapon = Player.instance.weaponEquipment3;
                    uiWeaponSlotBinding = Player.instance.input.Player.Equipment1;
                    break;
                }
        }
        uiWeaponSlotRefresh();
    }
    void uiWeaponSlotRefresh()
    {
        if (weapon.weapon == WeaponData.weaponList.defaultWeapon && uiInventory.instance.ifDefaultHideSlot)
        {
            uiWeaponSlotIcon.gameObject.SetActive(false);
            uiWeaponSlotWeaponName.gameObject.SetActive(false);
        }
        else
        {
            uiWeaponSlotIcon.gameObject.SetActive(uiWeaponSlotIconLoaded || !uiInventory.instance.ifDefaultHideSlot);
            uiWeaponSlotWeaponName.gameObject.SetActive(uiWeaponSlotIconLoaded || !uiInventory.instance.ifDefaultHideSlot);
            uiWeaponSlotBindingName.text = uiWeaponSlotBinding.GetBindingDisplayString();
            if (uiWeaponSlotWeaponName.text != weapon.weaponNameExternal)
            {
                uiWeaponSlotWeaponName.text = weapon.weaponNameExternal;
                if (weapon.weapon != WeaponData.weaponList.defaultWeapon)
                {
                    AsyncOperationHandle<Sprite> weaponSprite = Addressables.LoadAssetAsync<Sprite>("Assets/Textures/Weapon Icons/" + weapon.weaponNameInternal + "_Icon.png");
                    //Debug.Log("async started");
                    uiWeaponSlotIconLoaded = false;
                    weaponSprite.Completed += delegate { uiWeaponSlotLoadIconCompleted(ref weaponSprite); };
                }
            }
        }
    }
    void uiWeaponSlotLoadIconCompleted(ref AsyncOperationHandle<Sprite> weaponSprite)
    {
        if (weaponSprite.Status == AsyncOperationStatus.Succeeded)
        {
            uiWeaponSlotIcon.sprite = weaponSprite.Result;
            uiWeaponSlotIconLoaded = true;
            Debug.Log("set icon for " + uiWeaponSlotIcon.transform.parent.name);
        }
    }
}
