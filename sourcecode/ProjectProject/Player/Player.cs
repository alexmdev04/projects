using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player 
        instance { get; private set; }
    public GameObject 
        playerArmsPrefab;
    public PlayerInput 
        input;
    public Vector3 
        movementDirection;
    public List<Weapon> 
        weaponsHeld = new List<Weapon>();
    public Weapon
        weaponPrimary,
        weaponSecondary,
        weaponMelee,
        weaponEquipment1,
        weaponEquipment2,
        weaponEquipment3;
    public enum weaponSlots
    {
        weaponPrimary,
        weaponSecondary,
        weaponMelee,
        weaponEquipment1,
        weaponEquipment2,
        weaponEquipment3
    }
    public weaponSlots 
        weaponSlotEquipped;
    [SerializeField] bool createDefaultWeapons;
    [SerializeField] float
        movementSpeed = 4f,
        mouseSensitivity = 1f,
        movementAcceleration = 0.1f,
        movementDecceleration = 0.05f;
    float
        mouseXRotation,
        mouseYRotation;
    Rigidbody 
        playerRigidbody;
    Vector3
        smoothInputVelocity,
        smoothInput;


    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 165;
        QualitySettings.vSyncCount = 1;
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        input = new();
        input.Player.Shoot.performed += ctx => playerShoot();
        input.Player.WeaponSlot1.performed += ctx => playerEquipWeapon(weaponSlots.weaponPrimary);
        input.Player.WeaponSlot2.performed += ctx => playerEquipWeapon(weaponSlots.weaponSecondary);
        input.Player.MeleeWeaponSlot.performed += ctx => playerEquipWeapon(weaponSlots.weaponMelee);
        input.Player.EquipmentSlot1.performed += ctx => playerEquipWeapon(weaponSlots.weaponEquipment1);
        input.Player.WeaponSlotUp.performed += ctx => playerEquipWeaponScroll(true, false); 
        input.Player.WeaponSlotDown.performed += ctx => playerEquipWeaponScroll(false, false);
        input.Player.Enable();

    }
    void Start()
    {
        if (createDefaultWeapons)
        {
            weaponPrimary = WeaponData.instance.createWeapon(WeaponData.weaponList.defaultWeapon, 1);
            weaponSecondary = WeaponData.instance.createWeapon(WeaponData.weaponList.defaultWeapon, 2);
            weaponMelee = WeaponData.instance.createWeapon(WeaponData.weaponList.defaultWeapon, 3);
            weaponEquipment1 = WeaponData.instance.createWeapon(WeaponData.weaponList.defaultWeapon, 4);
        }
        weaponPrimary.weaponEquip();
    }
    void Update()
    {
        playerLook();
        movementDirection = input.Player.Move.ReadValue<Vector3>();
        if (movementDirection != Vector3.zero) { playerMove(movementAcceleration); }
        else { playerMove(movementDecceleration); }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            giveWeapon(WeaponData.weaponList.desertEagle, weaponSlotReplace: true);
        }
    }
    void playerLook()
    {
        mouseYRotation += input.Player.Look.ReadValue<Vector2>().x * Time.deltaTime * mouseSensitivity;
        mouseXRotation -= input.Player.Look.ReadValue<Vector2>().y * Time.deltaTime * mouseSensitivity;
        mouseXRotation = Mathf.Clamp(mouseXRotation, -90f, 90f);
        transform.localEulerAngles = new(0, mouseYRotation, 0);
        Camera.main.transform.localEulerAngles = new(mouseXRotation, 0, 0);
    }
    void playerMove(float acceleration)
    {
        smoothInput = Vector3.SmoothDamp(smoothInput, movementDirection, ref smoothInputVelocity, acceleration);
        playerRigidbody.MovePosition(playerRigidbody.position + (movementSpeed * Time.deltaTime * transform.TransformDirection(smoothInput)));
    }
    void playerShoot()
    {
        switch (weaponSlotEquipped)
        {
            case weaponSlots.weaponPrimary:
                {
                    weaponPrimary.weaponShoot();
                    break;
                }
            case weaponSlots.weaponSecondary:
                {
                    weaponSecondary.weaponShoot();
                    break;
                }
            case weaponSlots.weaponMelee:
                {
                    weaponMelee.weaponShoot();
                    break;
                }
            case weaponSlots.weaponEquipment1:
                {
                    weaponEquipment1.weaponShoot();
                    break;
                }
            case weaponSlots.weaponEquipment2:
                {
                    weaponEquipment2.weaponShoot();
                    break;
                }
            case weaponSlots.weaponEquipment3:
                {
                    weaponEquipment3.weaponShoot();
                    break;
                }
        }
    }
    public void giveWeapon(WeaponData.weaponList weapon, int variantID = 0, bool weaponSlotReplace = false)
    {
        Weapon newWeapon = WeaponData.instance.createWeapon(weapon, variantID);
        switch (newWeapon.weaponType)
        {
            case WeaponData.weaponTypes.gun:
                {
                    switch (newWeapon.weaponSlot)
                    {
                        case weaponSlots.weaponPrimary:
                            {
                                if (weaponPrimary != null && weaponSlotReplace)
                                {
                                    weaponPrimary = newWeapon;
                                    Debug.LogWarning("Replacing primary weapon with '" + newWeapon.weapon.ToString() + "'");
                                    break;
                                }
                                else if (weaponPrimary != null && !weaponSlotReplace)
                                {
                                    Debug.LogError("Player already has a primary weapon, set weaponSlotReplace to true to replace instead");
                                    break;
                                }
                                else
                                {
                                    weaponPrimary = newWeapon;
                                    Debug.Log("New primary weapon '" + newWeapon.weapon.ToString() + "'");
                                    break;
                                }
                            }
                        case weaponSlots.weaponSecondary:
                            {
                                if (weaponSecondary != null && weaponSlotReplace)
                                {
                                    weaponSecondary = newWeapon;
                                    Debug.LogWarning("Replacing secondary weapon with '" + newWeapon.weapon.ToString() + "'");
                                    break;
                                }
                                else if (weaponSecondary != null && !weaponSlotReplace)
                                {
                                    Debug.LogError("Player already has a secondary weapon, set weaponSlotReplace to true to replace instead");
                                    break;
                                }
                                else
                                {
                                    weaponSecondary = newWeapon;
                                    Debug.Log("New secondary weapon '" + newWeapon.weapon.ToString() + "'");
                                    break;
                                }
                            }
                    }
                    break; 
                }
            case WeaponData.weaponTypes.melee:
                {
                    {
                        if (weaponMelee != null && weaponSlotReplace)
                        {
                            weaponMelee = newWeapon;
                            Debug.LogWarning("Replacing melee weapon with '" + newWeapon.weapon.ToString() + "'");
                            break;
                        }
                        else if (weaponMelee != null && !weaponSlotReplace)
                        {
                            Debug.LogError("Player already has a melee weapon, set weaponSlotReplace to true to replace instead");
                            break;
                        }
                        else
                        {
                            weaponMelee = newWeapon;
                            Debug.Log("New melee weapon '" + newWeapon.weapon.ToString() + "'");
                            break;
                        }
                    }
                }
            case WeaponData.weaponTypes.equipment:
                {
                    if (weaponEquipment1 == null)
                    {
                        weaponEquipment1 = newWeapon;
                        Debug.Log("New equipment '" + newWeapon.weapon.ToString() + "'");
                        break;
                    }
                    else if (weaponEquipment2 == null)
                    {
                        weaponEquipment2 = newWeapon;
                        Debug.Log("New equipment '" + newWeapon.weapon.ToString() + "'");
                        break;
                    }
                    else if (weaponEquipment3 == null)
                    {
                        weaponEquipment3 = newWeapon;
                        Debug.Log("New equipment '" + newWeapon.weapon.ToString() + "'");
                        break;
                    }
                    else
                    {
                        if (weaponSlotReplace)
                        {
                            Debug.LogWarning("Equipment slots full, replacing weaponEquipment1");
                            weaponEquipment1 = newWeapon;
                            break;
                        }
                        else
                        {
                            Debug.LogError("Equipment slots full, set weaponSlotReplace to true to replace instead");
                            break;
                        }
                    }
                }
        }
        playerEquipWeapon(newWeapon.weaponSlot);
    }
    void playerEquipWeapon(weaponSlots weaponSlot)
    {
        Weapon weaponBeingEquipped = weaponMelee;
        switch (weaponSlot)
        {
            case weaponSlots.weaponPrimary:
                {
                    weaponBeingEquipped = weaponPrimary;
                    weaponSlotEquipped = weaponSlot;
                    break;
                }
            case weaponSlots.weaponSecondary:
                { 
                    weaponBeingEquipped = weaponSecondary;
                    weaponSlotEquipped = weaponSlot;
                    break;
                }
            case weaponSlots.weaponMelee:
                {
                    weaponBeingEquipped = weaponMelee;
                    weaponSlotEquipped = weaponSlot;
                    break;
                }
            case weaponSlots.weaponEquipment1:
                {
                    weaponBeingEquipped = weaponEquipment1;
                    weaponSlotEquipped = weaponSlot;
                    break;
                }
            case weaponSlots.weaponEquipment2:
                {
                    weaponBeingEquipped = weaponEquipment2;
                    weaponSlotEquipped = weaponSlot;
                    break;
                }
            case weaponSlots.weaponEquipment3:
                {
                    weaponBeingEquipped = weaponEquipment3;
                    weaponSlotEquipped = weaponSlot;
                    break;
                }
        }
        foreach (Weapon _weapon in weaponsHeld)
        {
            _weapon.weaponUnequip();
            _weapon.gameObject.SetActive(false);
            if (_weapon == weaponBeingEquipped)
            {
                _weapon.gameObject.SetActive(true);
                _weapon.weaponEquip();
            }
        }
    }
    void playerEquipWeaponScroll(bool up, bool wrapAround)
    {
        int slotChange;
        if (up)
        {
            if (!wrapAround && weaponSlotEquipped == weaponSlots.weaponMelee)
            {
                return;
            }
            slotChange = 1;
        }
        else
        {
            if (!wrapAround && weaponSlotEquipped == weaponSlots.weaponPrimary)
            {
                return;
            }
            slotChange = -1;
        }
        playerEquipWeapon(weaponSlotEquipped + slotChange);
    }
}