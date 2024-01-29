using UnityEngine;

public class InputHandler : MonoBehaviour
{ 
    // this script "binds" input maps to actions in a centralised location / creates publically available values that can be accessed but not edited

    public static InputHandler instance { get; private set; }
    public PlayerInput input { get; private set; }
    public bool active { get; private set; }

    void Awake()
    {
        instance = this;
        input = new();
        input.Player.Enable();
    }
    void Update()
    {
        if (!active) 
        {
            Player.instance.mouseDelta = Vector2.zero;
            Player.instance.movementDirection = Vector3.zero;
            return;
        }

        // mouse vector
        Player.instance.mouseDelta = input.Player.Look.ReadValue<Vector2>();

        // movement vector
        Player.instance.movementDirection = input.Player.Move.ReadValue<Vector3>();

	}
    public void SetActive(bool state)
    {
        active = state;
    }
}