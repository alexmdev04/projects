using System;
using System.Collections;
using System.Text;
using UnityEngine;
// weapon needs to wait for it to be loaded before playing sound or anims
public class Weapon : MonoBehaviour
{
    Animator animator;
    public Animation
        weaponAnimation { get; private set; }
    public WeaponData.weaponList
        weapon { get; private set; }
    public string weaponNameInternal { get; private set; }
    public string weaponNameExternal { get; private set; }
    public WeaponData.weaponTypes
        weaponType { get; private set; }
    public Player.weaponSlots
        weaponSlot { get; private set; }
    public float
        weaponRecoverySpeed = 5f;
    WeaponData.weaponFireTypes
        weaponFireType;
    [SerializeField] Vector3
        weaponOriginPosition,
        weaponOriginRotation;
    //[SerializeField] List<AnimationClip>
    //    weaponAnims; // place all animations here to be set to legacy - or set rig to legacy
    [SerializeField] AudioSource
        weaponEquipSound,
        weaponShootSound;
    [SerializeField] int
        weaponVariant = 0,
        weaponCamo = 0,
        weaponBurstAmount = 0; // only used if fireType is burst
    [Space]
    [SerializeField] bool
        weaponEquipped,
        weaponEquipFinished;
    [SerializeField] float
        weaponDamageMultiplier = 1f,
        weaponFireRateMultiplier = 1f,
        weaponEquipTimeMultiplier = 1f,
        weaponAimTimeMultiplier = 1f,
        weaponReloadTimeMultiplier = 1f;
    string
        weaponEquipAnim,
        weaponShootAnim,
        weaponReloadAnim,
        weaponOriginAnim;
    float
        weaponDamage = 0f, // base damage
        weaponFireRate = 0f, // shots per second, 0 is every frame
        weaponEquipTime = 0f, // in seconds
        weaponAimTime = 0f, // in seconds
        weaponReloadTime = 0f, // in seconds
        weaponWeight = 0f;
    public int weaponAmmoMagMax { get; private set; }
    public int weaponAmmoMagCurrent { get; private set; }
    public int weaponAmmoStock { get; private set; }
    GameObject
        weaponModel;
    float
        weaponFireRateElasped;
    bool
        weaponShootHeld;
    void Awake()
    {
        //foreach (AnimationClip anim in weaponAnims) { anim.legacy = true; }
        //animation.clip.legacy = true;

    }
    void Start()
    {
        

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //weaponAnimate(weaponOriginAnim);
            PlayerArms.instance.playerArmsAnimate(weaponOriginAnim, weapon);
        }
        if (weaponFireRateElasped > 0) { weaponFireRateElasped -= Time.deltaTime; }
        if (Input.GetKeyDown(KeyCode.R)) { weaponReload(); }
    }
    void LateUpdate()
    {
        if (weaponFireType == WeaponData.weaponFireTypes.semiAuto || 
            weaponFireType == WeaponData.weaponFireTypes.burst ||
            weaponFireType == WeaponData.weaponFireTypes.boltAction) 
            weaponShootHeld = Player.instance.input.Player.Shoot.IsPressed();
    }
    public void weaponShoot()
    {
        if (weaponEquipped && weaponEquipFinished)
        {
            switch (weaponFireType)
            {
                case WeaponData.weaponFireTypes.semiAuto:
                    {
                        if (weaponFireRateElasped > 0)
                        {
                            break;
                        }
                        else
                        {
                            if (weaponShootHeld)
                            {
                                break;
                            }
                            else
                            {
                                if (weaponAmmoCheck()) { weaponShootPassed(); }
                                weaponFireRateElasped = weaponFireRate;
                                break;
                            }
                        }
                    }
                case WeaponData.weaponFireTypes.fullAuto:
                    {
                        if (weaponFireRateElasped > 0)
                        {
                            break;
                        }
                        else
                        {
                            if (weaponAmmoCheck()) { weaponShootPassed(); }
                            weaponFireRateElasped = weaponFireRate;
                            break;
                        }
                    }
                case WeaponData.weaponFireTypes.burst:
                    {
                        Debug.LogError("Fire type weaponFireTypes.burst not implemented");
                        break;
                    }
                case WeaponData.weaponFireTypes.boltAction:
                    {
                        Debug.LogError("Fire type weaponFireTypes.boltAction not implemented");
                        break;
                    }
                case WeaponData.weaponFireTypes.melee:
                    {
                        Debug.LogError("Fire type weaponFireTypes.melee not implemented");
                        break;
                    }
                case WeaponData.weaponFireTypes.equipment:
                    {
                        Debug.LogError("Fire type weaponFireTypes.equipment not implemented");
                        break;
                    }
            }
            //weaponAnimate(weaponShootAnim);
        }
    }
    void weaponShootPassed()
    {
        weaponFireRateElasped = 0f;
        weaponSound(weaponShootSound, "weaponShootSound");
        PlayerArms.instance.playerArmsAnimate(weaponShootAnim, weapon);
        PlayerArms.instance.playerArmsPNShootRecoil();
    }

    public void weaponEquip()
    {
        PlayerArms.instance.playerArmsAnimate(weaponEquipAnim, weapon);
        StartCoroutine(weaponEquipAnimWait());
        weaponEquipped = true;
        weaponSound(weaponEquipSound, "weaponEquipSound");
    }
    IEnumerator weaponEquipAnimWait()
    {
        while (PlayerArms.instance.playerArmsAnimation.IsPlaying(weaponEquipAnim))
        {
            yield return new WaitForEndOfFrame();
            weaponEquipFinished = false;
        }
        Debug.Log("weapon drawn");
        weaponEquipFinished = true;
        yield return null;
    }
    public void weaponUnequip()
    {
        weaponEquipped = false;
        weaponEquipFinished = false;
        weaponFireRateElasped = 0f;
        
    }
    public void setWeapon
        (
        WeaponData.weaponList _weapon,
        string nameExternal,
        int variant,
        WeaponData.weaponTypes type,
        Player.weaponSlots slot,
        WeaponData.weaponFireTypes fireType,
        Vector3 originPosition,
        Vector3 originRotation,
        float damage,
        float fireRate,
        float equipTime,
        float aimTime,
        float reloadTime,
        float recoverySpeed,
        int ammoMagMax,
        int ammoStock,
        float weight = 50f,
        int burstAmount = 0
        )
    {
        weapon = _weapon;
        weaponNameInternal = weapon.ToString();
        weaponNameExternal = nameExternal;
        gameObject.name = weaponNameInternal;
        weaponVariant = variant;
        weaponEquipAnim = "Hands|" + weaponNameInternal + "_Equip";
        weaponShootAnim = "Hands|" + weaponNameInternal + "_Shoot";
        weaponReloadAnim = "Hands|" + weaponNameInternal + "_Reload";
        weaponOriginAnim = "Hands|" + weaponNameInternal + "_0";
        weaponType = type;
        weaponSlot = slot;
        weaponFireType = fireType;
        weaponOriginPosition = originPosition;
        weaponOriginRotation = originRotation;
        weaponDamage = damage;
        weaponFireRate = fireRate;
        weaponEquipTime = equipTime;
        weaponAimTime = aimTime;
        weaponReloadTime = reloadTime;
        weaponRecoverySpeed = recoverySpeed;
        weaponAmmoMagMax = ammoMagMax;
        weaponAmmoMagCurrent = weaponAmmoMagMax;
        weaponAmmoStock = ammoStock;
        weaponWeight = weight;
        weaponBurstAmount = burstAmount;
    }
    public void setWeaponModel(GameObject _weaponModel) 
    {
        weaponModel = _weaponModel;
        weaponModel.transform.SetLocalPositionAndRotation(weaponOriginPosition, Quaternion.Euler(weaponOriginRotation));
    }
    public void addWeaponAudio(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioClip.name == weaponNameInternal + "_Shoot_Sound")
        {
            weaponShootSound = audioSource;
            weaponShootSound.clip = audioClip;
            weaponShootSound.playOnAwake = false;
        }
        else if (audioClip.name == weaponNameInternal + "_Equip_Sound")
        {
            weaponEquipSound = audioSource;
            weaponEquipSound.clip = audioClip;
            weaponEquipSound.playOnAwake = false;
        }
        else
        {
            Debug.LogError("Unknown sound added to " + weaponNameInternal + ", " + audioClip.name);
        }
    }
    public void setWeaponAnimations(
        Animation animation,
        AnimationClip originAnim,
        AnimationClip equipAnim,
        AnimationClip shootAnim,
        AnimationClip reloadAnim
        )
    {
        weaponAnimation = animation;
        Debug.Log("adding clips");
        //weaponAnimation.AddClip(originAnim, weaponOriginAnim);
        //weaponAnimation.AddClip(equipAnim, weaponEquipAnim);
        //weaponAnimation.AddClip(shootAnim, weaponShootAnim);
        //weaponAnimation.AddClip(reloadAnim, weaponReloadAnim);
    }
    public StringBuilder getWeaponDebugStats()
    { // i swear it makes a performance difference
        StringBuilder sb = new StringBuilder();
        sb.Append("name: ").Append(Enum.GetName(weapon.GetType(), weapon)).Append("\n");
        sb.Append("type: ").Append(Enum.GetName(weaponType.GetType(), weaponType)).Append("\n");
        sb.Append("originPosition: (").Append(weaponOriginPosition.x).Append(", ").Append(weaponOriginPosition.y).Append(", ").Append(weaponOriginPosition.z).Append(")\n");
        sb.Append("originRotation: (").Append(weaponOriginRotation.x).Append(", ").Append(weaponOriginRotation.y).Append(", ").Append(weaponOriginRotation.z).Append(")\n");
        sb.Append("variant: ").Append(weaponVariant).Append("\n");
        sb.Append("camo: ").Append(weaponCamo).Append("\n");
        sb.Append("damage: ").Append(weaponDamage).Append(" x ").Append(weaponDamageMultiplier).Append(" = ").Append(weaponDamage * weaponDamageMultiplier).Append("\n");
        sb.Append("fireRate: ").Append(weaponFireRate).Append(" x ").Append(weaponFireRateMultiplier).Append(" = ").Append(weaponFireRate * weaponFireRateMultiplier).Append("\n");
        sb.Append("fireRateElapsed: ").Append(weaponFireRateElasped).Append("\n");
        sb.Append("equipTime: ").Append(weaponEquipTime).Append("s x ").Append(weaponEquipTimeMultiplier).Append(" = ").Append(weaponEquipTime * weaponEquipTimeMultiplier).Append("\n");
        sb.Append("aimTime: ").Append(weaponAimTime).Append("s x ").Append(weaponAimTimeMultiplier).Append(" = ").Append(weaponAimTime * weaponAimTimeMultiplier).Append("\n");
        sb.Append("reloadTime: ").Append(weaponReloadTime).Append("s x ").Append(weaponReloadTimeMultiplier).Append(" = ").Append(weaponReloadTime * weaponReloadTimeMultiplier).Append("\n");
        //sb.Append("recoverySpeed: ").Append(weaponRecoverySpeed).Append("\n");
        sb.Append("weight: ").Append(weaponWeight);
        return sb;
    }
    void weaponSound(AudioSource sound, string soundNameDebug = "sound")
    {
        if (weapon == WeaponData.weaponList.defaultWeapon)
        {
            return;
        }
        try
        {
            sound.PlayOneShot(sound.clip);
        }
        catch (NullReferenceException)
        {
            Debug.LogError(weaponNameInternal + " failed to play " + soundNameDebug + ", missing sound!");
        }
    }
    void weaponAnimate(string animation)
    {
        weaponAnimation.Stop();
        weaponAnimation.Play(animation);
    }
    public void weaponReload()
    {
        if (weaponAmmoStock <= 0 && weaponAmmoMagCurrent <= 0)
        {
            //Debug.Log(weaponNameInternal + " fully empty");
            return;
        }
        if (weaponAmmoStock <= 0)
        {
            //Debug.Log(weaponNameInternal + " stock empty");
            return;
        }
        if (weaponAmmoMagCurrent == weaponAmmoMagMax)
        {
            //Debug.Log(weaponNameInternal + " mag full");
            return;
        }
        if (weaponAmmoMagCurrent < weaponAmmoMagMax && weaponAmmoStock - (weaponAmmoMagMax - weaponAmmoMagCurrent) > 0)
        {
            weaponAmmoStock -= (weaponAmmoMagMax - weaponAmmoMagCurrent);
            weaponAmmoMagCurrent = weaponAmmoMagMax;
            //Debug.Log(weaponNameInternal + " reloaded");
            return;
        }
        if (weaponAmmoStock < weaponAmmoMagMax)
        {
            weaponAmmoMagCurrent += weaponAmmoStock;
            weaponAmmoStock = 0;
            //Debug.Log(weaponNameInternal + " reloaded (<1 full mag left)");
            return;
        }
        weaponAmmoStock -= weaponAmmoMagMax;
        weaponAmmoMagCurrent = weaponAmmoMagMax;
    }
    bool weaponAmmoCheck()
    {
        if (weaponAmmoMagCurrent > 0)
        {
            weaponAmmoMagCurrent -= 1;
            return true;
        }
        else
        {
            //Debug.Log(weaponNameInternal + " mag empty");
            weaponReload();
            return false;
        }
    }
}