using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public static PlayerHook instance {  get; private set; }
    public float hookMaxDistance = 500f, distanceFromWall = 0.1f;
    Vector3 playerDimensions = Vector3.one;
    [SerializeField] GameObject testCube1, testCube2;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void playerHookHeld()
    {
        //Debug.DrawRay(Player.instance.refTransform.transform.position + Player.instance.lineRendererOffset, Player.instance.refTransform.transform.TransformDirection(Vector3.forward), Color.green);
        RaycastHit 
            hit1, 
            hit2,
            cubeHitForward,
            cubeHitBack,
            cubeHitLeft,
            cubeHitRight,
            cubeHitUp,
            cubeHitDown;
        if (Physics.Raycast(Player.instance.refTransform.transform.position, Player.instance.refTransform.transform.TransformDirection(Vector3.forward), out hit1, hookMaxDistance))
        {
            //Debug.DrawLine(Player.instance.refTransform.transform.position + Player.instance.lineRendererOffset, hit1.point, Color.green);
            //Debug.Log(hit1.collider.name);
            //Player.instance.lineRenderer.SetPosition(1, hit1.point);
            testCube1.transform.position = hit1.point;
            Vector3 targetPosition = hit1.point;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit1.normal);
            Vector3 offset = hit1.normal * playerDimensions.z / 2f;
            targetPosition += offset;
            Debug.DrawRay(targetPosition, hit1.normal);
            Popcron.Gizmos.Sphere(targetPosition, playerDimensions.x / 2f);
            Collider[] sphere = Physics.OverlapSphere(targetPosition, (playerDimensions.x / 2f) + distanceFromWall, ~(1 << 2));
            if (sphere.Length > 1)
            {
                string obstructingObjNames = "";
                foreach (Collider obstructingObj in sphere) { obstructingObjNames += obstructingObj.gameObject.name + ", "; }
                Debug.Log("obstruction = " + obstructingObjNames);
            }
            else
            {
                testCube2.transform.SetPositionAndRotation(targetPosition, targetRotation);
            }

        }
    }
    public void playerHookReleased()
    {
        Player.instance.lineRenderer.SetPosition(1, Player.instance.refTransform.transform.position += Player.instance.lineRendererOffset);
    }
    public void playerHookFinished()
    {

    }
}
