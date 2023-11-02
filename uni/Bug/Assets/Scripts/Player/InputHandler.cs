using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{ 
    // this script "binds" input maps to actions in a centralised location

    public static InputHandler instance {  get; private set; }
    public PlayerInput input;
    void Awake()
    {
        instance = this;
        input = new();
        //input.Player.Jump.performed += ctx => playerJump();
        input.Player.Enable();
    }
    void Update()
    {
        // grapple button held
        if (input.Player.Grapple.IsPressed()) { Grapple.instance.GrappleHeld(); } 

        // grapple button released
        if (input.Player.Grapple.WasReleasedThisFrame()) { Grapple.instance.GrappleReleased(); } 

        // mouse vector
        Player.instance.mouseRotation = input.Player.Look.ReadValue<Vector2>(); 

        if (Input.GetKeyDown(KeyCode.R)) { Grapple.instance.GrappleAmmoCheck(); }
	}
}