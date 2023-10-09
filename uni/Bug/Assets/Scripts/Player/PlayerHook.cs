using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public static PlayerHook instance {  get; private set; }
    public float hookMaxDistance = 500f, distanceFromWall = 0.1f;

    float playerRadius;
    int layerMask = ~(1 << 2);
    Vector3 playerDimensions = Vector3.one;
    RaycastHit hit1, hit2;
    bool validLocation;

    [SerializeField] GameObject hit1Pos, targetPos, finalPos;

    void Awake()
    {
        instance = this;
        playerRadius = playerDimensions.x / 2f;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void playerHookHeld()
    {
        targetPos.SetActive(!validLocation);
        finalPos.SetActive(validLocation);

        if (Physics.Raycast(Player.instance.refTransform.transform.position, Player.instance.refTransform.transform.TransformDirection(Vector3.forward), out hit1, hookMaxDistance))
        {
            hit1Pos.transform.position = hit1.point;

            Vector3 targetPosition = hit1.point + hit1.normal * (playerRadius + distanceFromWall); // clipping prevention
            targetPos.transform.position = targetPosition;

            Collider[] sphere = Physics.OverlapSphere(targetPosition, playerRadius + distanceFromWall, layerMask);
            if (sphere.Length > 1)
            {
                //string obstructingObjNames = "";
                //foreach (Collider obstructingObj in sphere) { obstructingObjNames += obstructingObj.gameObject.name + ", "; }
                //Debug.Log("obstruction = " + obstructingObjNames);
                validLocation = false;
            }
            else
            {
                validLocation = true;
                finalPos.transform.position = targetPosition;
            }
        }
    }

    public void playerHookReleased()
    {
        if (validLocation && Physics.Raycast(Player.instance.refTransform.transform.position, Player.instance.refTransform.transform.TransformDirection(Vector3.forward), out hit2))
        {
            transform.position = hit2.point + hit1.normal * playerRadius;
            Player.instance.playerLookHookComplete(Vector3.Reflect(transform.forward, hit2.normal));
        }
    }
    public void playerHookFinished()
    {

    }
}
