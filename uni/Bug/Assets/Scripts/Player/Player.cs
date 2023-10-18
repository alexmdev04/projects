using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    public int targetFramerate;
    float 
        lookRotX,
        lookRotY;
    public Vector3 testVector;
    public Vector2 
        mouseRotation,
        mouseRotationMultiplier,
        lookSensitivity;
    public GameObject refTransform, worksheetObj;
    public Transform transformCalculator;
    public LineRenderer lineRenderer { get; private set; }
    public Vector3 
        lineRendererOffset = new Vector3(0f, -0.1f, 0f),
        movementDirection;
    public float movementSpeed = 5f;
    void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lineRenderer = GetComponent<LineRenderer>();
        Debug.developerConsoleEnabled = true;
    }
    void Start()
    {
    }
    void Update()
    {
        Application.targetFrameRate = targetFramerate;
        lineRenderer.textureScale = new Vector2(lineRenderer.positionCount, 1);
        //lineRenderer.SetPosition(0, refTransform.transform.position += lineRendererOffset);
        //refTransform.transform.position = transform.position;
        //refTransform.transform.eulerAngles = new(Camera.main.transform.eulerAngles.x, transform.eulerAngles.y, 0);
        if (Input.GetKeyDown(KeyCode.Tilde)) { Debug.developerConsoleVisible = !Debug.developerConsoleVisible; }
        if (Input.GetKeyDown(KeyCode.L)) { worksheetObj.SetActive(!worksheetObj.activeSelf); }
        PlayerMove();
    }
    void LateUpdate()
    {
        PlayerLook();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            targetFramerate += 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            targetFramerate -= 1;
        }
    }
    void PlayerLook()
    {
        if (PlayerHook.instance.playerHookMoving) { return; }
        mouseRotation *= mouseRotationMultiplier * lookSensitivity;
        lookRotY += mouseRotation.x * Time.fixedDeltaTime;
        lookRotX -= mouseRotation.y * Time.fixedDeltaTime;
        lookRotX = Mathf.Clamp(lookRotX, -90f, 90f);
        transform.eulerAngles = new(lookRotX, lookRotY, 0);
        //Camera.main.transform.localEulerAngles = new(lookRotX, 0, 0);
    }
    public void PlayerLookSet(Vector3 eulerAngles) 
    {
        lookRotX = eulerAngles.x;
        lookRotY = eulerAngles.y;
    }
	void PlayerMove()
    {
		transform.position += movementSpeed * Time.deltaTime * transform.TransformDirection(movementDirection);
	}
}