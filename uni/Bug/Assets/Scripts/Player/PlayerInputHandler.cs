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
    void Update()
    {
        if (input.Player.Hook.IsPressed()) { PlayerHook.instance.PlayerHookHeld(); }
        if (input.Player.Hook.WasReleasedThisFrame()) { PlayerHook.instance.PlayerHookReleased(); }
        Player.instance.mouseRotation = input.Player.Look.ReadValue<Vector2>();
        PlayerHook.instance.playerHookMaxDistance += Input.mouseScrollDelta.y;
        Player.instance.movementDirection = input.Player.Move.ReadValue<Vector3>();
	}
}
