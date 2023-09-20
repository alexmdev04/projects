using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player 
        instance { get; private set; }
    public int
        targetFramerate = 165;
    public float
        weaponRecoverySpeed,
        playerCameraHeight = 0.825f;
    public GameObject 
        playerArmsPrefab;
    public PlayerInput 
        input;
    public Vector3 
        movementDirection;
    public Vector2 
        mouseRotation,
        mouseRotationMultiplier = Vector2.one;
    // valorant = 3.75x
    public List<Weapon> 
        weaponsHeld = new List<Weapon>();
    public Weapon
        weaponPrimary,
        weaponSecondary,
        weaponMelee,
        weaponEquipment1,
        weaponEquipment2,
        weaponEquipment3,
        weaponEquipped;
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
    public bool 
        defaultWeaponsEquippable;
    [SerializeField] bool createDefaultWeapons,  weaponScrollWrapAround;
    [SerializeField] float
        movementSpeed = 4f,
        lookSensitivity = 1f,
        movementAcceleration = 0.1f,
        movementDecceleration = 0.05f,
        playerJumpForce = 5f,
        playerHeightCM = 180f;
    [SerializeField] GameObject playerCapsule;
    Rigidbody playerRigidbody;
    float
        lookRotX,
        lookRotY;
    Vector3
        smoothInputVelocity,
        smoothInput;

    void Awake()
    {
        instance = this;
        //Application.targetFrameRate = targetFramerate;
        QualitySettings.vSyncCount = 1;
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        input = new();
        input.Player.Primary_Weapon.performed += ctx => playerEquipWeaponBySlot(weaponSlots.weaponPrimary);
        input.Player.Secondary_Weapon.performed += ctx => playerEquipWeaponBySlot(weaponSlots.weaponSecondary);
        input.Player.Melee_Weapon.performed += ctx => playerEquipWeaponBySlot(weaponSlots.weaponMelee);
        input.Player.Equipment1.performed += ctx => playerEquipWeaponBySlot(weaponSlots.weaponEquipment1);
        input.Player.Weapon_Slot_Up.performed += ctx => playerEquipSlotScroll(true); 
        input.Player.Weapon_Slot_Down.performed += ctx => playerEquipSlotScroll(false);
        input.Player.Jump.performed += ctx => playerJump();
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
        playerEquipWeapon(weaponPrimary, weaponSlots.weaponPrimary, true);
    }
    void Update()
    {
        playerCapsule.transform.localScale = new Vector3(playerCapsule.transform.localScale.x, playerHeightCM / 200, playerCapsule.transform.localScale.z);
        Application.targetFrameRate = targetFramerate;
        checkEquippedWeapon();
        movementDirection = input.Player.Move.ReadValue<Vector3>();
        if (movementDirection != Vector3.zero) { playerMove(movementAcceleration); }
        else { playerMove(movementDecceleration); }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            giveWeapon(WeaponData.weaponList.akm, weaponSlotReplace: true);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            giveWeapon(WeaponData.weaponList.desertEagle, weaponSlotReplace: true);
        }
        if (input.Player.Shoot.IsPressed())
        {
            playerShoot();
        }
        if (Input.GetKeyDown(KeyCode.Space)) { playerJump(); }
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            targetFramerate += 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            targetFramerate -= 1;
        }
    }
    void LateUpdate()
    {
        playerLook();
    }
    void playerLook()
    {
        mouseRotation = input.Player.Look.ReadValue<Vector2>();
        mouseRotation = Vector2.Scale(mouseRotation, mouseRotationMultiplier * lookSensitivity);
        lookRotY += mouseRotation.x * Time.fixedDeltaTime;
        lookRotX -= mouseRotation.y * Time.fixedDeltaTime;
        lookRotX = Mathf.Clamp(lookRotX, -90f, 90f);
        transform.localEulerAngles = new(0, lookRotY, 0);
        // scale mouse rotation for different games cm/360
        Camera.main.transform.localEulerAngles = new(lookRotX, 0, 0);
    }
    void playerMove(float acceleration)
    {
        smoothInput = Vector3.SmoothDamp(smoothInput, movementDirection, ref smoothInputVelocity, acceleration);
        playerRigidbody.MovePosition(playerRigidbody.position + (movementSpeed * Time.deltaTime * transform.TransformDirection(smoothInput)));
    }
    void playerJump()
    {
        playerRigidbody.AddForce(playerJumpForce * Vector3.up, ForceMode.VelocityChange);
    }
    void playerShoot()
    {
        weaponEquipped.weaponShoot(); 
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
                                    playerDestroyWeapon(weaponPrimary);
                                    weaponPrimary = newWeapon;
                                    Debug.LogWarning("Replacing primary weapon with '" + newWeapon.weapon.ToString() + "'");
                                    break;
                                }
                                else if (weaponPrimary != null && !weaponSlotReplace)
                                {
                                    playerDestroyWeapon(newWeapon);
                                    Debug.LogError("Player already has a primary weapon, set weaponSlotReplace to true to replace instead");
                                    break;
                                }
                                else
                                {
                                    playerDestroyWeapon(weaponPrimary);
                                    weaponPrimary = newWeapon;
                                    Debug.Log("New primary weapon '" + newWeapon.weapon.ToString() + "'");
                                    break;
                                }
                            }
                        case weaponSlots.weaponSecondary:
                            {
                                if (weaponSecondary != null && weaponSlotReplace)
                                {
                                    playerDestroyWeapon(weaponSecondary);
                                    weaponSecondary = newWeapon;
                                    Debug.LogWarning("Replacing secondary weapon with '" + newWeapon.weapon.ToString() + "'");
                                    break;
                                }
                                else if (weaponSecondary != null && !weaponSlotReplace)
                                {
                                    playerDestroyWeapon(newWeapon);
                                    Debug.LogError("Player already has a secondary weapon, set weaponSlotReplace to true to replace instead");
                                    break;
                                }
                                else
                                {
                                    playerDestroyWeapon(weaponSecondary);
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
                            playerDestroyWeapon(weaponMelee);
                            weaponMelee = newWeapon;
                            Debug.LogWarning("Replacing melee weapon with '" + newWeapon.weapon.ToString() + "'");
                            break;
                        }
                        else if (weaponMelee != null && !weaponSlotReplace)
                        {
                            playerDestroyWeapon(newWeapon);
                            Debug.LogError("Player already has a melee weapon, set weaponSlotReplace to true to replace instead");
                            break;
                        }
                        else
                        {
                            playerDestroyWeapon(weaponMelee);
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
                        playerDestroyWeapon(weaponEquipment1);
                        weaponEquipment1 = newWeapon;
                        Debug.Log("New equipment '" + newWeapon.weapon.ToString() + "'");
                        break;
                    }
                    else if (weaponEquipment2 == null)
                    {
                        playerDestroyWeapon(weaponEquipment2);
                        weaponEquipment2 = newWeapon;
                        Debug.Log("New equipment '" + newWeapon.weapon.ToString() + "'");
                        break;
                    }
                    else if (weaponEquipment3 == null)
                    {
                        playerDestroyWeapon(weaponEquipment3); 
                        weaponEquipment3 = newWeapon;
                        Debug.Log("New equipment '" + newWeapon.weapon.ToString() + "'");
                        break;
                    }
                    else
                    {
                        if (weaponSlotReplace)
                        {
                            playerDestroyWeapon(weaponEquipment1);
                            weaponEquipment1 = newWeapon;
                            Debug.LogWarning("Equipment slots full, replacing weaponEquipment1");
                            break;
                        }
                        else
                        {
                            playerDestroyWeapon(newWeapon);
                            Debug.LogError("Equipment slots full, set weaponSlotReplace to true to replace instead");
                            break;
                        }
                    }
                }
        }
        if (weaponSlotReplace) { playerEquipWeaponBySlot(newWeapon.weaponSlot); }
    }
    void playerEquipWeaponByWeapon(Weapon weapon)
    {
        playerEquipWeapon(weapon, weapon.weaponSlot);
        //if (weapon.weapon == WeaponData.weaponList.defaultWeapon && defaultWeaponsEquippable)
        //{
        //    return;
        //}
        //else
        //{
        //    weaponRecoverySpeed = weapon.weaponRecoverySpeed;
        //    weaponSlotEquipped = weapon.weaponSlot;
        //    foreach (Weapon _weapon in weaponsHeld)
        //    {
        //        _weapon.weaponUnequip();
        //        _weapon.gameObject.SetActive(false);
        //        if (_weapon == weapon)
        //        {
        //            _weapon.gameObject.SetActive(true);
        //            _weapon.weaponEquip();
        //        }
        //    }
        //}
    }
    void playerEquipWeaponBySlot(weaponSlots weaponSlot)
    {
        Weapon weaponBeingEquipped = weaponMelee;
        switch (weaponSlot)
        {
            case weaponSlots.weaponPrimary:
                {
                    weaponBeingEquipped = weaponPrimary;
                    break;
                }
            case weaponSlots.weaponSecondary:
                { 
                    weaponBeingEquipped = weaponSecondary;
                    break;
                }
            case weaponSlots.weaponMelee:
                {
                    weaponBeingEquipped = weaponMelee;
                    break;
                }
            case weaponSlots.weaponEquipment1:
                {
                    weaponBeingEquipped = weaponEquipment1;
                    break;
                }
            case weaponSlots.weaponEquipment2:
                {
                    weaponBeingEquipped = weaponEquipment2;
                    break;
                }
            case weaponSlots.weaponEquipment3:
                {
                    weaponBeingEquipped = weaponEquipment3;
                    break;
                }
        }
        playerEquipWeapon(weaponBeingEquipped, weaponSlot);
        //if (weaponBeingEquipped.weapon == WeaponData.weaponList.defaultWeapon && defaultWeaponsEquippable)
        //{
        //    return;
        //}
        //else
        //{
        //    weaponRecoverySpeed = weaponBeingEquipped.weaponRecoverySpeed;
        //    weaponSlotEquipped = weaponSlot;
        //    foreach (Weapon _weapon in weaponsHeld)
        //    {
        //        _weapon.weaponUnequip();
        //        _weapon.gameObject.SetActive(false);
        //        if (_weapon == weaponBeingEquipped)
        //        {
        //            _weapon.gameObject.SetActive(true);
        //            _weapon.weaponEquip();
        //        }
        //    }
        //}
    }
    void playerEquipWeapon(Weapon weaponBeingEquipped, weaponSlots weaponSlot, bool forceEquip = false)
    {
        if (weaponBeingEquipped.weapon != WeaponData.weaponList.defaultWeapon || defaultWeaponsEquippable || forceEquip)
        {
            weaponRecoverySpeed = weaponBeingEquipped.weaponRecoverySpeed;
            weaponSlotEquipped = weaponSlot;
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
    }
    void playerEquipSlotScroll(bool up)
    {
        switch (weaponSlotEquipped)
        {
            case weaponSlots.weaponPrimary:
                break;
            case weaponSlots.weaponSecondary:
                break;
            case weaponSlots.weaponMelee:
                break;
            case weaponSlots.weaponEquipment1:
                break;
            case weaponSlots.weaponEquipment2:
                break;
            case weaponSlots.weaponEquipment3:
                break;
            default:
                break;
        }

        if (up)
        {

        }

        int slotChange;
        if (up)
        {
            if (weaponSlotEquipped == weaponSlots.weaponMelee) 
            {
                if (weaponScrollWrapAround)
                {
                    playerEquipWeaponBySlot(weaponSlots.weaponPrimary);
                    return;
                }
                else
                {
                    return;
                }
            }
            slotChange = 1;
        }
        else
        {
            if (weaponSlotEquipped == weaponSlots.weaponPrimary)
            {
                if (weaponScrollWrapAround)
                {
                    playerEquipWeaponBySlot(weaponSlots.weaponMelee);
                    return;
                }
                else
                {
                    return;
                }
            }
            slotChange = -1;
        }
        if (weaponSecondary.weapon == WeaponData.weaponList.defaultWeapon && !defaultWeaponsEquippable) { slotChange *= 2; }
        playerEquipWeaponBySlot(weaponSlotEquipped + slotChange);
    }
    void playerDestroyWeapon(Weapon weapon)
    {
        weaponsHeld.Remove(weapon);
        Destroy(weapon.gameObject);
    }
    void checkEquippedWeapon()
    {
        switch (weaponSlotEquipped)
        {
            case weaponSlots.weaponPrimary:
                {
                    weaponEquipped = weaponPrimary;
                    break;
                }
            case weaponSlots.weaponSecondary:
                {
                    weaponEquipped = weaponSecondary;
                    break;
                }
            case weaponSlots.weaponMelee:
                {
                    weaponEquipped = weaponMelee;
                    break;
                }
            case weaponSlots.weaponEquipment1:
                {
                    weaponEquipped = weaponEquipment1;
                    break;
                }
            case weaponSlots.weaponEquipment2:
                {
                    weaponEquipped = weaponEquipment2;
                    break;
                }
            case weaponSlots.weaponEquipment3:
                {
                    weaponEquipped = weaponEquipment3;
                    break;
                }
        }
    }
}