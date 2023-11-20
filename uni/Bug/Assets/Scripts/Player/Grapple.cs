using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Grapple : MonoBehaviour
{
    // this script controls everything about the hook, if controlling the player call a method in player script
    public static Grapple instance {  get; private set; }
    public bool playerRotation;
    public bool grapplePointValid { get; private set; }
    public bool playerMoving { get; private set; }
    public Collider[] grapplePointCheck { get; private set; }
    public Vector3 debugTargetPosition { get; private set; }
    public Vector3 debugTargetRotation { get; private set; }
    public float debugDistanceToTargetPosition { get; private set; }
    public GameObject grappleDestinationMarker;
    public enum ammoStateEnum
    {
        infinite,
        distanceLimited,
        usesLimited
    }
    public ammoStateEnum 
        ammoState = ammoStateEnum.infinite;
    public float 
        maxDistance = 500f, 
        distanceFromWall = 0.1f,
        moveSpeed = 1f,
        rotateSpeed = 1f, 
        distanceTravelled;
    [SerializeField] GameObject 
        grapplePoint;
    [SerializeField] LayerMask 
        layerMask = ~(1 << 2);
    //float 
    //    ammoDistanceCurrent,
    //    ammoDistanceMax,
    //    ammoUsesCurrent,
    //    ammoUsesMax;
    RaycastHit 
        hit1, 
        hit2;
    MeshRenderer 
        grapplePointRenderer;
    bool 
        movementActive,
        grappleCancelled;
    Vector3
        targetPosition;
    Quaternion
        targetRotation;
    [HideInInspector] public UnityEvent 
        grappleFired = new(),
        grappleFinished = new();

    void Awake()
    {
        instance = this;
        grapplePointRenderer = grapplePoint.GetComponent<MeshRenderer>();
    }
    void Update()
    {
        debugTargetPosition = targetPosition;
        debugTargetRotation = targetRotation.eulerAngles;
        GrapplePlayerMovement();
    }
    /// <summary>
    /// The main code loop of the grapple, runs while the grapple button is held
    /// </summary>
    public void GrappleHeld() 
    {
        // currently the grappling is disabled until the player reaches the grapple point
        if (playerMoving) { return; } 
        if (grappleCancelled) { return; }
        // grapplePoint is the holographic representation of the player while holding the grapple button
        // green = valid grapple point, red = invalid grapple point
        grapplePoint.SetActive(true);
        grapplePointRenderer.material.SetColor("_fresnelColor", grapplePointValid ? Color.green : Color.red);

        // this sends a ray from the center of the camera to a raycastable wall, there can be a distance cap
        if (Physics.Raycast(Player.instance.transform.position, Player.instance.transform.TransformDirection(Vector3.forward), out hit1, maxDistance, layerMask))
        {
            GrapplePointValidCheck();
        }
        else
        {
            // if the raycast to find a wall fails then the player hologram is set to the max distance from the player
            grapplePoint.transform.position = Player.instance.transform.position + Player.instance.transform.TransformDirection(Vector3.forward) * maxDistance;
            grapplePointValid = false;
            grapplePointRenderer.material.SetColor("_fresnelColor", Color.red);
        }
    }
    /// <summary>
    /// Checks whether the grapple does not allow the player to clip through walls
    /// </summary>
    void GrapplePointValidCheck() 
    { 
        // the sphere overlap radius must be slightly bigger than the player to make sure the grapple point surface is collided with
        targetPosition = hit1.point + hit1.normal * (Player.instance.playerRadius + distanceFromWall);
        grapplePointCheck = Physics.OverlapSphere(targetPosition, Player.instance.playerRadius + distanceFromWall + 0.01f, layerMask);

        // if the sphere collides with more than the wall the player is looking at then it is most likely invalid
        // however this could cause overlapped colliders to be invalid locations
        grapplePointValid = !(grapplePointCheck.Length > 1); 
        grapplePoint.transform.position = targetPosition;
        debugTargetPosition = targetPosition;
        if (uiDebug.instance.debugMode)
        {
            Debug.DrawLine(Player.instance.transform.position + Player.instance.lineRendererOffset, hit1.point, Color.cyan);
            Popcron.Gizmos.Sphere(targetPosition, Player.instance.playerRadius + distanceFromWall + 0.01f, Color.cyan);
            Debug.DrawRay(hit1.point, hit1.normal);
        }
    }
    /// <summary>
    /// If the grapple point is valid this will being the movement to the grapple point, runs when the grapple button is released
    /// </summary>
    public void GrappleReleased() 
    {
        // currently the grappling is disabled until the player reaches the grapple point
        if (playerMoving) { return; }
        grapplePoint.SetActive(false);
        if (grappleCancelled) { grappleCancelled = false; return; }
        if (!movementActive) { return; }
        if (grapplePointValid)
        {
			// lerp to the grapple destination and reflect rotation if enabled
			playerMoving = true;
            targetRotation = Player.instance.transform.rotation.ReflectRotation(hit1.normal);
            targetPosition = hit1.point + hit1.normal * Player.instance.playerRadius;
            grappleFired.Invoke();
        }
    }
    /// <summary>
    /// Moves the player towards the target position and rotation
    /// </summary>
    public void GrapplePlayerMovement()
    {
        if (!playerMoving) { return; }
        if (grappleDestinationMarker.activeSelf) { grappleDestinationMarker.transform.position = targetPosition; }
        distanceTravelled = Vector3.Distance(Player.instance.transform.position, targetPosition);
        Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, targetPosition, Time.deltaTime * moveSpeed);
        //if (playerRotation) { Player.instance.LookSet(Quaternion.Lerp(Player.instance.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed).eulerAngles); }
        debugDistanceToTargetPosition = Vector3.Distance(Player.instance.transform.position, targetPosition);
        if (Vector3.Distance(Player.instance.transform.position, targetPosition) == 0) { GrapplePlayerMovementFinished(); }
    }
    void GrapplePlayerMovementFinished()
    {
        playerMoving = false;
        distanceTravelled = 0;
        grappleFinished.Invoke();
    }
    public void PlayerTeleported(Vector3 position, Vector3 eulerAngles = default)
    {
        playerMoving = false;
        targetPosition = position;
        if (eulerAngles != default) { targetRotation = Quaternion.Euler(eulerAngles); }
        GrapplePointReset();
    }
    //public bool GrappleAmmoCheck() // change to check level for ammo
    //{
    //    switch (ammoState)
    //    {
    //        case ammoStateEnum.infinite:
    //            {
    //                ui.instance.grapple.Refresh(ammoStateEnum.infinite);
    //                return true;
    //            }
    //        case ammoStateEnum.distanceLimited:
    //            {
    //                ui.instance.grapple.Refresh(ammoStateEnum.distanceLimited, ammoDistanceCurrent, ammoDistanceMax);
    //                return ammoDistanceCurrent > 0;
    //            }
    //        case ammoStateEnum.usesLimited:
    //            {
    //                ui.instance.grapple.Refresh(ammoStateEnum.usesLimited, ammoUsesCurrent, ammoUsesMax);
    //                return ammoUsesCurrent > 0;
    //            }
    //        default: return false;
    //    }
    //}
    public void GrapplePointReset()
    {
        grapplePoint.transform.position = Vector3.zero;
        grapplePointValid = false;
        grapplePoint.SetActive(false);
        grapplePointCheck = null;
    }
    public void Cancel()
    {
        grappleCancelled = true;
        GrapplePointReset();
    }
    public void SetMovementActive(bool state)
    {
        movementActive = state;
    }
    public void Enable()
    {

    }
    public void Disable()
    {
        
    }
}