using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class uiInventory : MonoBehaviour
{
    [SerializeField] bool ifDefaultHideSlot = true;

    [Space]
    [Header("weaponMelee")]
    [SerializeField] GameObject uiWeaponMelee;
    [SerializeField] Image uiWeaponMeleeIcon;
    [SerializeField] TextMeshProUGUI uiWeaponMeleeBindingName;
    [SerializeField] TextMeshProUGUI uiWeaponMeleeName;

    [Space]
    [Header("weaponSecondary")]
    [SerializeField] GameObject uiWeaponSecondary;
    [SerializeField] Image uiWeaponSecondaryIcon;
    [SerializeField] TextMeshProUGUI uiWeaponSecondaryBindingName;
    [SerializeField] TextMeshProUGUI uiWeaponSecondaryName;

    [Space]
    [Header("weaponPrimary")]
    [SerializeField] GameObject uiWeaponPrimary;
    [SerializeField] Image uiWeaponPrimaryIcon;
    [SerializeField] TextMeshProUGUI uiWeaponPrimaryBindingName;
    [SerializeField] TextMeshProUGUI uiWeaponPrimaryName;

    void Update()
    {
        uiInventoryRefresh(
            weapon: Player.instance.weaponMelee,
            uiWeapon: uiWeaponMelee, 
            uiWeaponName: uiWeaponMeleeName,
            uiWeaponBindingName: uiWeaponMeleeBindingName,
            uiWeaponIcon: uiWeaponMeleeIcon, 
            uiWeaponBinding: Player.instance.input.Player.weaponPrimary);

        uiInventoryRefresh(
            weapon: Player.instance.weaponSecondary,
            uiWeapon: uiWeaponSecondary,
            uiWeaponName: uiWeaponSecondaryName,
            uiWeaponBindingName: uiWeaponSecondaryBindingName,
            uiWeaponIcon: uiWeaponSecondaryIcon,
            uiWeaponBinding: Player.instance.input.Player.weaponSecondary);

        uiInventoryRefresh(
            weapon: Player.instance.weaponPrimary,
            uiWeapon: uiWeaponPrimary,
            uiWeaponName: uiWeaponPrimaryName,
            uiWeaponBindingName: uiWeaponPrimaryBindingName,
            uiWeaponIcon: uiWeaponPrimaryIcon, 
            uiWeaponBinding: Player.instance.input.Player.weaponPrimary);
    }
    void uiInventoryRefresh(
        Weapon weapon,
        GameObject uiWeapon,
        TextMeshProUGUI uiWeaponName,
        TextMeshProUGUI uiWeaponBindingName,
        Image uiWeaponIcon,
        InputAction uiWeaponBinding)
    {
        if (weapon.weapon == WeaponData.weaponList.defaultWeapon && ifDefaultHideSlot)
        {
            uiWeapon.gameObject.SetActive(false);
        }
        else
        {
            uiWeapon.gameObject.SetActive(true);
            uiInventoryCheckWeapon(uiWeaponName, weapon, uiWeaponIcon);
            uiWeaponBindingName.text = uiWeaponBinding.GetBindingDisplayString();
        }
    }
    void uiInventoryCheckWeapon(TextMeshProUGUI currentUIName, Weapon weaponToCheck, Image iconToSet)
    {
        if (currentUIName.text != weaponToCheck.weaponNameExternal)
        {
            currentUIName.text = weaponToCheck.weaponNameExternal;
            uiInventorySetIcon(iconToSet, weaponToCheck);
        }
    }
    void uiInventorySetIcon(Image weaponIcon, Weapon weapon)
    {
        if (weapon.weapon != WeaponData.weaponList.defaultWeapon)
        {
            AsyncOperationHandle<Sprite> weaponSprite = Addressables.LoadAssetAsync<Sprite>("Assets/Textures/Weapon Icons/" + weapon.weaponNameInternal + "_Icon.png");
            weaponSprite.Completed += delegate { uiInventoryLoadIconCompleted(weaponSprite, weaponIcon); };
        }
    }
    void uiInventoryLoadIconCompleted(AsyncOperationHandle<Sprite> weaponSprite, Image weaponIcon)
    {
        if (weaponSprite.Status == AsyncOperationStatus.Succeeded)
        {
            weaponIcon.sprite = weaponSprite.Result;
            Debug.Log("set icon for " + weaponIcon.transform.parent.name);
        }
    }
}