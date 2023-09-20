using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class PlayerArmsDrift : MonoBehaviour
{
    [SerializeField] Vector3
        positionScale = new(0.01f, 0.01f, 0.01f),
        positionTargetForward = new(0f, -2f, -2f),
        positionTargetBackward = new(0f, -1f, 1f),
        positionTargetLeftOnly = new(0f, -2f, -2f),
        positionTargetRightOnly = new(0f, -1f, 1f),
        rotationTargetLeft = new(0f, 0f, 7f),
        rotationTargetRight = new(0f, 0f, -7f);
    [SerializeField] float
        positionTargetLerpSpeed = 10f,
        rotationTargetLerpSpeed = 10f,
        positionToIdleSpeed = 20f,
        rotationToIdleSpeed = 20f,
        positionToWalkingSpeed = 5f,
        rotationToWalkingSpeed = 5f,
        idleSwaySpeedMultiplier = 1f,
        idleSwayDistanceMultiplier = 1f,
        walkingAnimSpeed,
        walkingAnimXMultiplier,
        walkingAnimYMultiplier,
        walkingAnimRotMultiplier,
        walkingAnimDelay,
        walkingAnimCameraBobMultiplier = 0.5f,
        lookSwayXMultiplier = 0.01f,
        lookSwayYMultiplier = 0.01f;
    Vector3
        position,
        rotation,
        walkingAnimVector;
    [HideInInspector] public StringBuilder
        debugDriftPositionInput,
        debugDriftPositionTarget,
        debugDriftPositionOutput,
        debugDriftRotationInput,
        debugDriftRotationTarget,
        debugDriftRotationOutput;
    [HideInInspector] public string
        debugDriftPositionSpeed,
        debugDriftRotationSpeed;
    float
        walkingAnimX = 0f,
        walkingAnimY = 0f,
        walkingAnimDelayValue;
    bool
        walkingAnimLoop,
        walkingAnimEnable;
    void Start()
    {
        StartCoroutine(walkingAnim());
    }
    public void driftkb(Vector3 positionTemp)
    {
        debugDriftPositionInput = new StringBuilder()
            .Append("(").Append(positionTemp.x)
            .Append(", ").Append(positionTemp.y)
            .Append(", ").Append(positionTemp.z).Append(")");
        float
            positionToStateSpeed,
            rotationToStateSpeed;
        Vector3
            rotationTemp,
            positionTarget,
            rotationTarget;

        // +x = right
        // +z = forward

        if (positionTemp != Vector3.zero)
        {
            if (positionTemp.x != 0)
            {
                rotationTarget = (positionTemp.x < 0) ? rotationTargetLeft : rotationTargetRight; // moving sideways

                if (positionTemp.z != 0) 
                { 
                    positionTarget = (positionTemp.z > 0) ? positionTargetForward : positionTargetBackward; // moving forward/backward and sideways
                } 
                else 
                { 
                    positionTarget = (positionTemp.x < 0) ? positionTargetLeftOnly : positionTargetRightOnly; // only moving sideways
                } 
            }
            else // only moving forward or backward
            {
                positionTarget = (positionTemp.z > 0) ? positionTargetForward : positionTargetBackward;
                rotationTarget = Vector3.zero;
            } 
            

            position = Vector3.Lerp(position, positionTarget, Time.deltaTime * positionTargetLerpSpeed);
            rotation = Vector3.Lerp(rotation, rotationTarget, Time.deltaTime * rotationTargetLerpSpeed);
            positionTemp = position;
            rotationTemp = rotation;
            positionToStateSpeed = positionToWalkingSpeed;
            rotationToStateSpeed = rotationToWalkingSpeed;
            walkingAnimEnable = true;
        }
        else
        {
            rotationTemp = Vector3.zero;
            positionTarget = Vector3.zero;
            rotationTarget = Vector3.zero;
            position = Vector3.zero;
            rotation = Vector3.zero;
            positionToStateSpeed = positionToIdleSpeed;
            rotationToStateSpeed = rotationToIdleSpeed;
            walkingAnimX = 0f;
            walkingAnimY = 0f;
            walkingAnimVector = Vector3.zero;
            walkingAnimEnable = false;
            walkingAnimDelayValue = 0f;
        }

        Vector3 playerArmsIdleSway = new(Mathf.Cos(PlayerArms.instance.gameClock * idleSwaySpeedMultiplier), Mathf.Sin(PlayerArms.instance.gameClock * idleSwaySpeedMultiplier), 0f);
        Vector3 positionScaled = Vector3.Scale(positionTemp, positionScale);
        positionScaled += (playerArmsIdleSway * idleSwayDistanceMultiplier) + walkingAnimVector;
        Vector3 driftPositionOutput = Vector3.Lerp(transform.localPosition, positionScaled, Time.deltaTime * positionToStateSpeed);
        transform.localPosition = driftPositionOutput;

        //sin(-0.5pi x^2))
        rotationTemp += new Vector3(walkingAnimVector.x * walkingAnimRotMultiplier, 0f, 0f);
        rotationTemp += new Vector3(Mathf.Clamp(Player.instance.mouseRotation.y, -1000, 1000) * lookSwayXMultiplier, Mathf.Clamp(-Player.instance.mouseRotation.x, -1000, 1000) * lookSwayYMultiplier, 0f);
        Quaternion driftRotationOutput = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationTemp), Time.deltaTime * rotationToStateSpeed);
        transform.localRotation = Quaternion.Euler(driftRotationOutput.eulerAngles);// + PlayerArms.instance.pnVector);
        Camera.main.transform.localPosition = new Vector3(0f, Player.instance.playerCameraHeight, 0f) + (driftPositionOutput * walkingAnimCameraBobMultiplier);

        #region debug data gathering
        debugDriftPositionTarget = new StringBuilder()
            .Append("(").Append(Math.Round(positionTarget.x, 4))
            .Append(", ").Append(Math.Round(positionTarget.y, 4))
            .Append(", ").Append(Math.Round(positionTarget.z, 4)).Append(")");

        debugDriftPositionOutput = new StringBuilder()
            .Append("(").Append(Math.Round(driftPositionOutput.x, 4))
            .Append(", ").Append(Math.Round(driftPositionOutput.y, 4))
            .Append(", ").Append(Math.Round(driftPositionOutput.z, 4)).Append(")");

        debugDriftRotationTarget = new StringBuilder()
            .Append("(").Append(Math.Round(rotationTarget.x, 4))
            .Append(", ").Append(Math.Round(rotationTarget.y, 4))
            .Append(", ").Append(Math.Round(rotationTarget.z, 4)).Append(")");

        debugDriftRotationOutput = new StringBuilder()
            .Append("(").Append(Math.Round(driftRotationOutput.eulerAngles.x, 4))
            .Append(", ").Append(Math.Round(driftRotationOutput.eulerAngles.y, 4))
            .Append(", ").Append(Math.Round(driftRotationOutput.eulerAngles.z, 4)).Append(")");

        debugDriftPositionSpeed = positionToStateSpeed.ToString();
        debugDriftRotationSpeed = rotationToStateSpeed.ToString();
        #endregion
    }
    IEnumerator walkingAnim()
    {
        while (true) 
        {
            if (walkingAnimEnable && walkingAnimDelayValue >= walkingAnimDelay)
            {
                float walkingAnimValue = 0;
                if (!walkingAnimLoop)
                {
                    if (walkingAnimX >= 1f) { walkingAnimLoop = true; }
                    else { walkingAnimValue = Time.deltaTime * walkingAnimSpeed; }
                }
                else
                {
                    if (walkingAnimX <= -1f) { walkingAnimLoop = false; }
                    else { walkingAnimValue = -Time.deltaTime * walkingAnimSpeed; }
                }
                walkingAnimX += walkingAnimValue;
                walkingAnimY = Mathf.Sin(-0.5f * Mathf.PI * Mathf.Pow(walkingAnimX, 2));
                walkingAnimVector = new(walkingAnimX * walkingAnimXMultiplier, walkingAnimY * walkingAnimYMultiplier, 0f);
            }
            else
            {
                walkingAnimDelayValue += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}