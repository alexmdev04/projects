using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public static PlayerHook instance {  get; private set; }
    [SerializeField] float hookMaxDistance = 5f, distanceFromPoint = 0.5f;
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
        //Debug.DrawRay(Player.instance.refTransform.transform.position, Player.instance.refTransform.transform.TransformDirection(Vector3.forward), Color.green);
        RaycastHit hit1, hit2;
        if (Physics.Raycast(Player.instance.refTransform.transform.position, Player.instance.refTransform.transform.TransformDirection(Vector3.forward), out hit1, hookMaxDistance))
        {
            Debug.DrawLine(Player.instance.refTransform.transform.position += Player.instance.lineRendererOffset, hit1.point, Color.green);
            //Debug.Log(hit1.collider.name);
            //Player.instance.lineRenderer.SetPosition(1, hit1.point);
            Debug.DrawRay(Player.instance.refTransform.transform.position + (Player.instance.refTransform.transform.TransformDirection(Vector3.forward) * (hit1.distance - distanceFromPoint)), Player.instance.refTransform.transform.TransformDirection(Vector3.forward), Color.red);//out hit2, hookMaxDistance);
            // place circle on wall, flush, not inside wall, place edge of circle on the wall idk
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
