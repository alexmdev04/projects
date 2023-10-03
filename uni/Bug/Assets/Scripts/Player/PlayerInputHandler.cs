using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInput input;

    void Awake()
    {
        input = new();
        //input.Player.Jump.performed += ctx => playerJump();
        input.Player.Enable();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (input.Player.Hook.IsPressed()) { PlayerHook.instance.playerHookHeld(); }
        if (input.Player.Hook.WasReleasedThisFrame()) { PlayerHook.instance.playerHookReleased(); }
        Player.instance.mouseRotation = input.Player.Look.ReadValue<Vector2>();
    }
}
