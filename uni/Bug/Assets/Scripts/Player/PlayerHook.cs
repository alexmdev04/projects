using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public static PlayerHook instance {  get; private set; }
    public float hookMaxDistance = 500f, distanceFromPoint = 0.5f;
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
            Physics.Raycast(hit1.point, Vector3.forward, out cubeHitForward);
            Debug.DrawRay(hit1.point, Vector3.forward, Color.magenta);

            Physics.Raycast(hit1.point, Vector3.back, out cubeHitBack);
            Debug.DrawRay(hit1.point, Vector3.back, Color.magenta);

            Physics.Raycast(hit1.point, Vector3.left, out cubeHitLeft);
            Debug.DrawRay(hit1.point, Vector3.left, Color.magenta);

            Physics.Raycast(hit1.point, Vector3.right, out cubeHitRight);
            Debug.DrawRay(hit1.point, Vector3.right, Color.magenta);

            Physics.Raycast(hit1.point, Vector3.up, out cubeHitUp);
            Debug.DrawRay(hit1.point, Vector3.up, Color.magenta);

            Physics.Raycast(hit1.point, Vector3.down, out cubeHitDown);
            Debug.DrawRay(hit1.point, Vector3.down, Color.magenta);

            Vector3 nextPoint = (Player.instance.refTransform.transform.position + Player.instance.lineRendererOffset) + (Player.instance.refTransform.transform.TransformDirection(Vector3.forward) * (hit1.distance - distanceFromPoint));
            testCube2.transform.position = nextPoint;
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
