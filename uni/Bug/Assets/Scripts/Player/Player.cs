using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    // this script controls everything about the player e.g. position, state, look, interact.

    // instancing
    public static Player instance { get; private set; }
    public LineRenderer lineRenderer { get; private set; }
    public int 
        targetFramerate;
    public Vector2 
        mouseRotation,
        mouseRotationMultiplier,
        lookSensitivity;
    public GameObject 
        worksheetObj;
    public Vector3 
        lineRendererOffset = new Vector3(0f, -0.1f, 0f),
        playerDimensions = Vector3.one;
    public float playerRadius;
    Vector2 playerEulerAngles;
    [SerializeField] Light torch;

    void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lineRenderer = GetComponent<LineRenderer>();
        Debug.developerConsoleEnabled = true;
        playerDimensions = Vector3.Scale(playerDimensions, transform.localScale);
        playerRadius = playerDimensions.x / 2f;
    }
    void Start()
    {

    }
    void Update()
    {
        Application.targetFrameRate = targetFramerate;
        lineRenderer.textureScale = new Vector2(lineRenderer.positionCount, 1);
        if (Input.GetKeyDown(KeyCode.Tilde)) { Debug.developerConsoleVisible = !Debug.developerConsoleVisible; }
        if (Input.GetKeyDown(KeyCode.F8)) { worksheetObj.SetActive(!worksheetObj.activeSelf); }
    }
    void LateUpdate()
    {
        Look();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Equals))
        {
            targetFramerate += 1;
        }
        else if (Input.GetKey(KeyCode.Minus))
        {
            targetFramerate -= 1;
        }
    }
    /// <summary>
    /// Controls the camera view of the player - where they are looking
    /// </summary>
    void Look()
    {
        mouseRotation = (mouseRotation * mouseRotationMultiplier) * lookSensitivity;
        playerEulerAngles.x += mouseRotation.x * lookSensitivity.x;
        playerEulerAngles.y += mouseRotation.y * lookSensitivity.y;
        playerEulerAngles.y = Mathf.Clamp(playerEulerAngles.y, -90f, 90f);
        transform.localRotation = Quaternion.AngleAxis(playerEulerAngles.x, Vector3.up) 
                                * Quaternion.AngleAxis(playerEulerAngles.y, Vector3.left);
    }
    /// <summary>
    /// <para>Manually sets the rotation of the player</para>
    /// <para>Used instead of Player.instance.transform.eulerAngles = Vector3</para>
    /// </summary>
    /// <param name="eulerAngles"></param>
    public void LookSet(Vector3 eulerAngles)
    {
        playerEulerAngles.x = eulerAngles.x;
        playerEulerAngles.y = eulerAngles.y;
    }
    ///// <summary>
    ///// <para>Manually sets the rotation of the player</para>
    ///// <para>Used instead of Player.instance.transform.rotation = Quaternion</para>
    ///// </summary>
    ///// <param name="quaternion"></param>
    //public void LookSet(Quaternion quaternion)
    //{
    //    Vector3 eulerAngles = quaternion.eulerAngles;
    //    lookRotX = eulerAngles.x;
    //    lookRotY = eulerAngles.y;
    //}
    public void BeginTutorial()
    {

    }
    public void TeleportInstant(Vector3 worldSpacePosition, Vector3 worldSpaceEulerAngles = default)
    {
        Grapple.instance.PlayerTeleported(worldSpacePosition, worldSpaceEulerAngles);
        transform.position = worldSpacePosition;
        playerEulerAngles = worldSpaceEulerAngles;
        //if (worldSpaceEulerAngles != default) 
        //{ 
        //    LookSet(worldSpaceEulerAngles);
        //}
    }
    public void debugToggleTorch()
    {
        torch.gameObject.SetActive(!torch.gameObject.activeSelf);
    }
    public StringBuilder debugGetStats()
    {
        return new StringBuilder(uiDebug.str_playerTitle)
            .Append(uiDebug.str_targetFramerate).Append(targetFramerate.ToString())
            .Append(uiDebug.str_mouseRotation).Append(mouseRotation.ToStringBuilder()).Append(uiDebug.str_multiply).Append(mouseRotationMultiplier.ToStringBuilder())
            .Append(uiDebug.str_lookSensitivity).Append(lookSensitivity.ToStringBuilder())
            .Append(uiDebug.str_playerDimensions).Append(playerRadius);
    }
}