using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public static PlayerHook instance {  get; private set; }
    public float 
        playerHookMaxDistance = 500f, 
        playerHookDistanceFromWall = 0.1f,
        playerHookMoveSpeed = 1f,
        playerHookRotateSpeed = 1f;
    public bool playerHookRotate;
    public bool playerHookValidLocation { get; private set; }
    public bool playerHookMoving { get; private set; }
    public Collider[] playerHookPointCheck { get; private set; }
    [SerializeField] GameObject playerHookPoint, testCube1;
    [SerializeField] LayerMask layerMask = ~(1 << 2);

    float playerRadius;
    Vector3 playerDimensions = Vector3.one;
    RaycastHit playerHookHit1, playerHookHit2;
    MeshRenderer playerHookPointRenderer;

    public Vector3 debugTargetPosition { get; private set; }
    public Vector3 debugTargetRotation { get; private set; }
    public float debugDistanceToTargetPosition { get; private set; }


    void Awake()
    {
        instance = this;
        playerHookPointRenderer = playerHookPoint.GetComponent<MeshRenderer>();
        playerRadius = playerDimensions.x / 2f;
    }
    public void PlayerHookHeld()
    {
        if (playerHookMoving) { return; }

        playerHookPoint.SetActive(true);
        playerHookPointRenderer.material.SetColor("_fresnelColor", playerHookValidLocation ? Color.green : Color.red);

        if (Physics.Raycast(Player.instance.transform.position, Player.instance.transform.TransformDirection(Vector3.forward), out playerHookHit1, playerHookMaxDistance, layerMask))
        {
            Vector3 targetPosition = playerHookHit1.point + playerHookHit1.normal * (playerRadius + playerHookDistanceFromWall);
            playerHookPointCheck = Physics.OverlapSphere(targetPosition, playerRadius + playerHookDistanceFromWall + 0.01f, layerMask);
			playerHookValidLocation = !(playerHookPointCheck.Length > 1);
	        playerHookPoint.transform.position = targetPosition;
            debugTargetPosition = targetPosition;
            if (uiDebug.instance.debugMode)
			{
				Debug.DrawLine(Player.instance.transform.position + Player.instance.lineRendererOffset, playerHookHit1.point, Color.cyan);
				Popcron.Gizmos.Sphere(targetPosition, playerRadius + playerHookDistanceFromWall + 0.01f, Color.cyan);
                Debug.DrawRay(playerHookHit1.point, playerHookHit1.normal);
			}
        }
        else
        {
            playerHookPoint.transform.position = Player.instance.transform.position + Player.instance.transform.TransformDirection(Vector3.forward) * playerHookMaxDistance;
            playerHookValidLocation = false;
            playerHookPointRenderer.material.SetColor("_fresnelColor", Color.red); ;
        }
    }
    public void PlayerHookReleased()
    {
        if (playerHookMoving) { return; }
        playerHookPoint.SetActive(false);
        if (playerHookValidLocation && Physics.Raycast(Player.instance.transform.position, Player.instance.transform.TransformDirection(Vector3.forward), out playerHookHit2))
        {
			// lerp to pos
			playerHookMoving = true;
            Quaternion targetRotation = Player.instance.transform.rotation.ReflectRotation(playerHookHit1.normal);
            Vector3 targetPosition = playerHookHit2.point + playerHookHit1.normal * playerRadius;
            debugTargetPosition = targetPosition;
            debugTargetRotation = targetRotation.eulerAngles;
            StartCoroutine(PlayerHookMove(targetPosition, targetRotation));
        }
    }
    public IEnumerator PlayerHookMove(Vector3 targetPosition, Quaternion targetRotation)
    {
        testCube1.transform.position = targetPosition;
        do
        {
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, targetPosition, Time.deltaTime * playerHookMoveSpeed);
            if (playerHookRotate) { Player.instance.PlayerLookSet(Quaternion.Lerp(Player.instance.transform.rotation, targetRotation, Time.deltaTime * playerHookRotateSpeed).eulerAngles); }
            debugDistanceToTargetPosition = Vector3.Distance(Player.instance.transform.position, targetPosition);
            yield return new WaitForEndOfFrame();
        }
        while (Vector3.Distance(Player.instance.transform.position, targetPosition) > 0f);
        Player.instance.transform.position = targetPosition;
        if (playerHookRotate) { Player.instance.PlayerLookSet(targetRotation.eulerAngles); }
        playerHookMoving = false;
    }
}