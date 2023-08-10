using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public static PlayerArms 
        instance { get; private set; }
    public Animation 
        playerArmsAnimation { get; private set; }
    [SerializeField] Vector3 
        playerArmsDriftPositionScale = new(0.01f, 0.01f, 0.01f);
    [SerializeField] Vector2
        playerArmsLookSwayValue;
    [SerializeField] float
        playerArmsDriftPositionLerp = 10,
        playerArmsDriftSpeedToIdle = 20f,
        playerArmsDriftSpeedToWalking = 5f,
        playerArmsIdleSwaySpeedMultiplier = 1f,
        playerArmsIdleSwayDistanceMultiplier = 1f;        
    float
        playerArmsDriftPositionX = 0,
        playerArmsDriftPositionY = 0,
        playerArmsDriftPositionZ = 0,
        gameClock;
    void Awake()
    {
        instance = this;
        playerArmsAnimation = GetComponent<Animation>();
    }
    void Start()
    {

    }

    void Update()
    {
        gameClock += Time.deltaTime;
        playerArmsDrift(Player.instance.movementDirection);
        playerArmsLookSway();
    }
    void playerArmsDrift(Vector3 driftPosition)
    {
        float
            driftSpeed,
            driftPositionYTarget = 0,
            driftPositionZTarget = 0;

        if (driftPosition != Vector3.zero)
        {
            // set y and z
            if (driftPosition.z > 0) // only forward
            {
                driftPositionYTarget = -2;
                driftPositionZTarget = -2;
            }
            else if (driftPosition.z < 0) // only backward
            {
                driftPositionYTarget = -1;
                driftPositionZTarget = 1;
            }
            if (driftPosition.x != 0)
            {
                driftPositionYTarget = (driftPosition.z < 0) ? -1 : -2; // sideways and backward
                driftPositionZTarget = (driftPosition.z < 0) ? 1 : -2; //  sideways or forward
            }
            playerArmsDriftPositionX = Mathf.Lerp(playerArmsDriftPositionX, driftPosition.x, Time.deltaTime * playerArmsDriftPositionLerp);
            playerArmsDriftPositionY = Mathf.Lerp(playerArmsDriftPositionY, driftPositionYTarget, Time.deltaTime * playerArmsDriftPositionLerp);
            playerArmsDriftPositionZ = Mathf.Lerp(playerArmsDriftPositionZ, driftPositionZTarget, Time.deltaTime * playerArmsDriftPositionLerp);
            driftPosition = new(
                playerArmsDriftPositionX,
                playerArmsDriftPositionY,
                playerArmsDriftPositionZ);
            driftSpeed = playerArmsDriftSpeedToWalking;
        }
        else
        {
            playerArmsDriftPositionX = 0;
            playerArmsDriftPositionY = 0;
            playerArmsDriftPositionZ = 0;
            driftSpeed = playerArmsDriftSpeedToIdle;
        }
        Vector3 driftPositionScaled = Vector3.Scale(driftPosition, playerArmsDriftPositionScale);
        Vector3 playerArmsIdleSway = new(Mathf.Cos(gameClock * playerArmsIdleSwaySpeedMultiplier), Mathf.Sin(gameClock * playerArmsIdleSwaySpeedMultiplier), 0f);
        driftPositionScaled += playerArmsIdleSway * playerArmsIdleSwayDistanceMultiplier;
        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, driftPositionScaled, Time.deltaTime * driftSpeed);
    }
    public void playerArmsAnimate(string animation, WeaponData.weaponList weapon = WeaponData.weaponList.defaultWeapon)
    {
        playerArmsAnimation.Stop();
        if (weapon != WeaponData.weaponList.defaultWeapon) { playerArmsAnimation.Play("Hands|" + weapon.ToString() + "_0"); }
        playerArmsAnimation.Play(animation);
    }
    void playerArmsLookSway()
    {
        playerArmsLookSwayValue = Player.instance.input.Player.Look.ReadValue<Vector2>();
    }
}