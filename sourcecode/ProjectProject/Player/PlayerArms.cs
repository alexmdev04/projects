using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public static PlayerArms 
        instance { get; private set; }
    public PlayerArmsDrift playerArmsDrift { get; private set; }
    public Animation 
        playerArmsAnimation { get; private set; }
    public  Vector2 playerArmsLookSwayValue { get; private set; }
    public float gameClock;
    public Vector3 pnVector { get; private set; }
    [SerializeField] Vector3 pnScale = Vector3.one;
    void Awake()
    {
        instance = this;
        playerArmsAnimation = GetComponent<Animation>();
        playerArmsAnimation.wrapMode = WrapMode.Clamp;
        playerArmsDrift = GetComponent<PlayerArmsDrift>();
    }
    void Start()
    {

    }
    void Update()
    {
        gameClock += Time.deltaTime;
        playerArmsDrift.driftkb(Player.instance.movementDirection);
        pnVector = Vector3.Lerp(pnVector, Vector3.zero, Time.deltaTime * Player.instance.weaponRecoverySpeed);
    }
    public void playerArmsAnimate(string animation, WeaponData.weaponList weapon = WeaponData.weaponList.defaultWeapon)
    {
        playerArmsAnimation.Stop();
        if (weapon != WeaponData.weaponList.defaultWeapon) { playerArmsAnimation.Play("Hands|" + weapon.ToString() + "_0"); }
        playerArmsAnimation.Play(animation);
    }
    public void playerArmsPNShootRecoil()
    {
        float random1 = UnityEngine.Random.Range(0f, 100f);
        float random2 = UnityEngine.Random.Range(0f, 100f);
        float pn1 = Mathf.PerlinNoise(random1, random2);
        float pn2 = Mathf.PerlinNoise(random2, random1);
        float pnPosNeg1 = UnityEngine.Random.Range(0f, 1f);
        float pnPosNeg2 = UnityEngine.Random.Range(0f, 1f);
        float pnPosNegOutput1 = (pnPosNeg1 > 0.5f) ? -1 : 1;
        float pnPosNegOutput2 = (pnPosNeg2 > 0.5f) ? -1 : 1;
        pnVector = Vector3.Scale(new Vector3(pn1 * pnPosNegOutput1, pn2 * pnPosNegOutput2, 0), pnScale);
        transform.localEulerAngles += pnVector;
        //Debug.Log(pnVector);
    }
}