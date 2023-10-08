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
    public Vector2 
        mouseRotation,
        mouseRotationMultiplier,
        lookSensitivity;
    public GameObject refTransform;
    public LineRenderer lineRenderer { get; private set; }
    public Vector3 lineRendererOffset = new Vector3(0f, -0.1f, 0f);
    void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
    }
    void Update()
    {
        Application.targetFrameRate = targetFramerate;
        lineRenderer.SetPosition(0, refTransform.transform.position += lineRendererOffset);
        refTransform.transform.position = transform.position;
        refTransform.transform.eulerAngles = new(Camera.main.transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
    void LateUpdate()
    {
        playerLook();
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
    void playerLook()
    {
        mouseRotation *= mouseRotationMultiplier * lookSensitivity;
        lookRotY += mouseRotation.x * Time.fixedDeltaTime;
        lookRotX -= mouseRotation.y * Time.fixedDeltaTime;
        lookRotX = Mathf.Clamp(lookRotX, -90f, 90f);
        transform.localEulerAngles = new(0, lookRotY, 0);
        Camera.main.transform.localEulerAngles = new(lookRotX, 0, 0);
    }
}
