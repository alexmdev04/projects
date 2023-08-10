using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponData : MonoBehaviour
{
    public static WeaponData 
        instance { get; private set; }
    Weapon newWeapon;
    GameObject 
        newWeaponObj,
        newWeaponModel;
    Transform newWeaponParent;
    string newWeaponFileName;
    AnimationClip
        originAnim,
        equipAnim,
        shootAnim,
        reloadAnim;
    List<AudioClip> newWeaponSounds;
    AudioClip
        shootSound,
        equipSound,
        reloadSound;
    [SerializeField] GameObject weaponDefault;
    void Awake() { instance = this; }
    public enum weaponList
    {
        defaultWeapon,
        fists,
        knife,
        desertEagle
    }
    public enum weaponTypes
    {
        gun,
        melee,
        equipment // smokes etc?
    }
    public enum weaponFireTypes
    {
        semiAuto,
        fullAuto,
        burst,
        boltAction,
        melee,
        equipment
    }
    public enum weaponPenetrationLevel
    {
        none,
        low,
        medium,
        high,
        max
    }
    public Weapon createWeapon(weaponList weapon, int variant = 0)
    {
        newWeaponFileName = weapon.ToString();
        Debug.Log("newWeapon, " + newWeaponFileName + " variant=" + variant);

        weaponTypes type = weaponTypes.melee;
        Player.weaponSlots slot = Player.weaponSlots.weaponMelee;
        weaponFireTypes fireType = weaponFireTypes.melee;
        Vector3 
            originPosition = Vector3.zero,
            originRotation = Vector3.zero;
        float 
            damage = 0f,
            fireRate = 0f,
            equipTime = 0f,
            aimTime = 0f,
            reloadTime = 0f;


        switch (weapon)
        {
            case weaponList.defaultWeapon:
                {
                    type = weaponTypes.gun;
                    originPosition = new Vector3(-0.1426f, 0.6044f, -0.1378f);
                    originRotation = new Vector3(-90, -0f, -180f);
                    damage = 100;
                    fireRate = 1f;
                    equipTime = 1f;
                    aimTime = 0.25f;
                    reloadTime = 1f;
                    switch (variant)
                    {
                        case 1:
                            {
                                slot = Player.weaponSlots.weaponPrimary;
                                fireType = weaponFireTypes.fullAuto;
                                break;
                            }
                        case 2:
                            {
                                slot = Player.weaponSlots.weaponSecondary;
                                fireType = weaponFireTypes.semiAuto;
                                break;
                            }
                        case 3:
                            {
                                type = weaponTypes.melee;
                                slot = Player.weaponSlots.weaponMelee;
                                fireType = weaponFireTypes.melee;
                                break;
                            }
                        case 4:
                            {
                                type = weaponTypes.equipment;
                                slot = Player.weaponSlots.weaponEquipment1;
                                fireType = weaponFireTypes.equipment;
                                break;
                            }
                        default:
                            {
                                slot = Player.weaponSlots.weaponPrimary;
                                fireType = weaponFireTypes.fullAuto;
                                break;
                            }
                    }
                    break;
                }
            case weaponList.desertEagle:
                {
                    type = weaponTypes.gun;
                    slot = Player.weaponSlots.weaponSecondary;
                    fireType = weaponFireTypes.semiAuto;
                    originPosition = new Vector3(0.1425832f, 0.10636367f, 0.6310548f);
                    originRotation = new Vector3(0f, -180f, 0f);
                    damage = 65;
                    fireRate = 0.5f;
                    equipTime = 1f;
                    aimTime = 0.3f;
                    reloadTime = 1f;
                    break;
                }
            case weaponList.fists:
                {

                    break;
                }
            case weaponList.knife:
                {
                    type = weaponTypes.melee;
                    slot = Player.weaponSlots.weaponMelee;
                    fireType = weaponFireTypes.melee;
                    originPosition = new Vector3();
                    originRotation = new Vector3();
                    damage = 100;
                    fireRate = 0.5f;
                    equipTime = 1f;
                    aimTime = 0f;
                    reloadTime = 0f;
                    break;
                }
            default:
                {
                    Debug.LogError("Weapon " + newWeaponFileName + " not found!");
                    return null;
                }
        }
        // find transform for creating new model
        newWeaponParent = PlayerArms.instance.gameObject.transform.Find("Hands").Find(newWeaponFileName + "_Weapon");
        if (newWeaponParent == null)
        {
            Debug.LogError("No transform found! (playerArms/Hands/" + newWeaponFileName + "_Weapon)");
            newWeaponParent = PlayerArms.instance.gameObject.transform.Find("Hands");
        }
        
        // creating default model in case model loading fails, also makes sure layer is set to viewmodel
        newWeaponObj = null;
        newWeaponObj = Instantiate(weaponDefault, newWeaponParent);
        //newWeaponObj.layer = 3;
        //newWeaponObj.transform.localScale = Vector3.one;
        //newWeaponObj.transform.localPosition = Vector3.zero;

        // creates weapon component and sets its data
        newWeapon = null;
        newWeapon = newWeaponObj.AddComponent<Weapon>();
        newWeapon.setWeapon(weapon, variant, type, slot, fireType, originPosition, originRotation, damage, fireRate, equipTime, aimTime, reloadTime);
        Player.instance.weaponsHeld.Add(newWeapon);

        // adding weapon model
        newWeaponModel = null;
        loadWeaponModel("Assets/Models/weaponModels/" + newWeaponFileName + ".fbx", newWeaponObj.transform);

        // adding weapon animations
        originAnim = null;
        equipAnim = null;
        shootAnim = null;
        reloadAnim = null;
        loadWeaponAnimationClips("Assets/Models/playerArmsModels/SeperateArms.fbx");

        // adding weapon sounds
        equipSound = null;
        shootSound = null;
        reloadSound = null;
        loadWeaponSounds();

        return newWeapon;
    }
    void loadWeaponModel(string path = "Assets/Models/weaponModels/desertEagle.fbx", Transform parentObj = null)
    {
        try
        {
            AsyncOperationHandle<GameObject> weaponModel = Addressables.LoadAssetAsync<GameObject>(path);
            weaponModel.Completed += delegate { loadWeaponModelCompleted(weaponModel, parentObj); };
        }
        catch (InvalidKeyException)
        {
            Debug.LogError("No weapon model found for " + path + ", creating weaponDefault");
            newWeapon.setWeaponModel(newWeaponModel);
            newWeaponModel.layer = 3;
            newWeaponModel.name = newWeaponFileName;
        }
    }
    void loadWeaponModelCompleted(AsyncOperationHandle<GameObject> weaponModel, Transform parentObj)
    {
        if (weaponModel.Status == AsyncOperationStatus.Succeeded)
        {
            newWeaponModel = Instantiate(weaponModel.Result, parentObj);
            Destroy(newWeaponObj.transform.Find("weaponDefault").gameObject);
            newWeapon.setWeaponModel(newWeaponModel);
            newWeaponModel.layer = 3;
            newWeaponModel.GetComponentInChildren<MeshRenderer>().gameObject.layer = 3;
            newWeaponModel.name = newWeaponFileName;
        }
        else
        {
            //if (weaponModel.OperationException == (System.Exception)InvalidKeyException() )

            Debug.LogError("Failed to load model! " + weaponModel.DebugName);
        }
    }
    void loadWeaponAnimationClips(string path = "Assets/Models/playerArmsModels/PlayerArms.fbx")
    {
        try
        {
            AsyncOperationHandle<IList<AnimationClip>> animationClips = Addressables.LoadAssetAsync<IList<AnimationClip>>(path);
            animationClips.Completed += delegate { loadWeaponAnimationClipsCompleted(animationClips); };
        }
        catch (InvalidKeyException) 
        {
            Debug.LogError("No animations found in active playerArms model!");
        }
    }
    void loadWeaponAnimationClipsCompleted(AsyncOperationHandle<IList<AnimationClip>> animationClips)
    {
        if (animationClips.Status == AsyncOperationStatus.Succeeded)
        {
            //foreach (AnimationClip animationClip in animationClips.Result)
            //{
            //    Debug.Log(animationClip.name);
            //}
            newWeapon.setWeaponAnimations(
                newWeapon.AddComponent<Animation>(),
                originAnim,
                equipAnim,
                shootAnim,
                reloadAnim);
        }
        else
        {
            Debug.LogError("Failed to load animationClips list! " + animationClips.DebugName);
        }
    }
    void loadWeaponSounds()
    {
        try
        {
            AsyncOperationHandle<AudioClip> shootSound = Addressables.LoadAssetAsync<AudioClip>("Assets/Sounds/" + newWeaponFileName + "_Shoot_Sound.wav");
            shootSound.Completed += delegate { loadWeaponSoundCompleted(shootSound); };
        }
        catch (InvalidKeyException)
        {
            Debug.LogError(newWeaponFileName + "_Shoot_Sound not found!");
        }
        try
        {
            AsyncOperationHandle<AudioClip> equipSound = Addressables.LoadAssetAsync<AudioClip>("Assets/Sounds/" + newWeaponFileName + "_Equip_Sound.wav");
            equipSound.Completed += delegate { loadWeaponSoundCompleted(equipSound); };
        }
        catch (InvalidKeyException)
        {
            Debug.LogError(newWeaponFileName + "_Equip_Sound not found!");
        }
    }
    void loadWeaponSoundCompleted(AsyncOperationHandle<AudioClip> sound)
    {
        if (sound.Status == AsyncOperationStatus.Succeeded)
        {
            newWeapon.addWeaponAudio(newWeapon.AddComponent<AudioSource>(), sound.Result);
        }
        else
        {
            Debug.LogError("Failed to load sound! " + sound.DebugName);
        }
    }
}