using System.Collections;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    // this script controls everything about the player e.g. position, state, look, interact.
    public static Player instance { get; private set; }
    Rigidbody rb;
    public Vector2 
        mouseDelta,
        mouseDeltaMultiplier = Vector2.one,
        lookSensitivity;
    public Vector3 
        playerEulerAngles,
        movementDirection;
    public bool 
        lookActive = true,
        moveActive = true;
    [SerializeField] float 
        movementSpeed = 5f,
        cameraHeight = 0.825f,
        movementAcceleration = 0.1f,
        movementDecceleration = 0.05f,
        jumpForce = 5f,
        playerHeightCM = 180f;
    [SerializeField] GameObject
        playerCapsule;
    public Transform
        playerTransformReadOnly;
    Vector3 
        smoothInputVelocity,
        smoothInput;
    /* leftover torch from project bug
    [SerializeField] Light torch;
    [SerializeField] float torchIntensityMax = 300f;
    [SerializeField] float torchIntensityMin = 0.2f;
    [SerializeField] float torchIntensityDistance = 15f;
    bool torchActive;
    RaycastHit torchHit;
    */
    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 165;
        QualitySettings.vSyncCount = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F)) { ToggleTorch(); }
        playerCapsule.transform.localScale = new Vector3(playerCapsule.transform.localScale.x, playerHeightCM / 200, playerCapsule.transform.localScale.z);
        Camera.main.transform.localPosition = new(Camera.main.transform.localPosition.x, cameraHeight, Camera.main.transform.localPosition.z);
        if (moveActive) { Move(); }

        playerTransformReadOnly.position = rb.position;
    }
    void LateUpdate()
    {
        if (lookActive) { Look(); }
    }
    void FixedUpdate()
    {

    }
    /// <summary>
    /// Controls the camera view of the player - where they are looking
    /// </summary>
    void Look()
    {
        mouseDelta = (mouseDelta * mouseDeltaMultiplier) * lookSensitivity;
        playerEulerAngles.x += mouseDelta.x * lookSensitivity.x;
        playerEulerAngles.y += mouseDelta.y * lookSensitivity.y;
        playerEulerAngles.y = Mathf.Clamp(playerEulerAngles.y, -90f, 90f);

        Quaternion newRotation = Quaternion.AngleAxis(playerEulerAngles.x, Vector3.up)
                                * Quaternion.AngleAxis(playerEulerAngles.y, Vector3.left);

        playerTransformReadOnly.localRotation = newRotation;
        transform.localEulerAngles = new Vector3(0f, newRotation.eulerAngles.y, 0f);
        Camera.main.transform.localEulerAngles = new Vector3(newRotation.eulerAngles.x, 0f, 0f);
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
    void Move()
    {
        float acceleration;
        if (movementDirection != Vector3.zero) { acceleration = movementAcceleration; }
        else { acceleration = movementDecceleration; }
        //float acceleration = movementDirection != Vector3.zero ? movementAcceleration : movementDecceleration;
        smoothInput = Vector3.SmoothDamp(smoothInput, movementDirection, ref smoothInputVelocity, acceleration);
        rb.MovePosition(rb.position + (movementSpeed * Time.deltaTime * transform.TransformDirection(smoothInput)));
    }
    void Jump()
    {
        rb.AddForce(jumpForce * Vector3.up, ForceMode.VelocityChange);
    }
    /// <summary>
    /// Instantly teleports the player to the given position and rotation
    /// </summary>
    /// <param name="worldSpacePosition"></param>
    /// <param name="worldSpaceEulerAngles"></param> 
    public void TeleportInstant(Vector3 worldSpacePosition, Vector3 worldSpaceEulerAngles)
    {
        transform.position = worldSpacePosition;
        playerEulerAngles.x = worldSpaceEulerAngles.y;
        float yAngle = 0;
        if (worldSpaceEulerAngles.x == 0) { yAngle = 0; }
        else if (worldSpaceEulerAngles.x > 0 && worldSpaceEulerAngles.x <= 90) { yAngle = 0 - worldSpaceEulerAngles.x; }
        else if (worldSpaceEulerAngles.x < 360 && worldSpaceEulerAngles.x >= 270) { yAngle = 360 - worldSpaceEulerAngles.x; }
        playerEulerAngles.y = yAngle;
        //Debug.Log("teleported to; pos: " + worldSpacePosition + ", inrot: " + worldSpaceEulerAngles + ", outrot: " + playerEulerAngles);
    }
    //public void TorchSetActive(bool state)
    //{
    //    torchActive = state;
    //}
    //public void ToggleTorch()
    //{
    //    if (!torchActive) { return; }
    //    torch.gameObject.SetActive(!torch.gameObject.activeSelf);
    //}
    public StringBuilder debugGetStats()
    {
        return new StringBuilder(uiDebug.str_playerTitle)
            .Append(uiDebug.str_targetFPS).Append(Application.targetFrameRate).Append(uiDebug.str_vSync).Append(QualitySettings.vSyncCount)
            //.Append(uiDebug.str_mouseRotation).Append(mouseDelta.ToStringBuilder()).Append(uiDebug.str_multiply).Append(mouseDeltaMultiplier.ToStringBuilder())
            .Append(uiDebug.str_lookSensitivity).Append(lookSensitivity.ToStringBuilder())
            .Append("movementDirection = ").Append(movementDirection);
    }
    /// <summary>
    /// Scales the intensity of the torch depending on how close a wall is to the player
    /// </summary>
    //void TorchIntensityScaler()
    //{
    //    if (torch.gameObject.activeSelf)
    //    {
    //        torch.intensity =
    //            Physics.Raycast(transform.position, transform.forward, out torchHit, torchIntensityDistance) ?
    //            Mathf.Lerp(torchIntensityMin, torchIntensityMax, Vector3.Distance(transform.position, torchHit.point) / torchIntensityDistance)
    //            : torchIntensityMax;
    //    }
    //}
    IEnumerator walkingAnim()
    {
        bool 
            walkingAnimEnable = true,
            walkingAnimLoop = false;
        float
            walkingAnimDelay = 0.175f,
            walkingAnimDelayValue = 0f,
            walkingAnimX = 0f, //
            walkingAnimXMultiplier = 0.01f,
            walkingAnimY = 0f, //
            walkingAnimYMultiplier = 0.025f,
            walkingAnimSpeed = 5f;
        Vector3 walkingAnimVector;

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
                walkingAnimY = walkingAnimEquation(walkingAnimX);
                walkingAnimVector = new(walkingAnimX * walkingAnimXMultiplier, walkingAnimY * walkingAnimYMultiplier, 0f);
            }
            else
            {
                walkingAnimDelayValue += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    float walkingAnimEquation(float walkingAnimX)
    {
        return Mathf.Sin(-0.5f * Mathf.PI * Mathf.Pow(walkingAnimX, 2));
    }
}