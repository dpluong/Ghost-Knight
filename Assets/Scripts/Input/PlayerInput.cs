using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public InputActionAsset actions;
    private InputAction moveAction;
    private InputAction jumpAction;

    public Vector2 moveVector;
    public bool jumpTrigger;
    public bool jumpDisabled;

    void Awake()
    {
        moveAction = actions.FindActionMap("Player").FindAction("Move");
        jumpAction = actions.FindActionMap("Player").FindAction("Jump");

        jumpAction.performed += OnJump;
        jumpAction.canceled += OnJumpCancel;
    
    }

    void Update()
    {
        ReadMoveInput();   
    }

    void OnEnable()
    {
        actions.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        actions.FindActionMap("Player").Disable();
    }

    private void ReadMoveInput()
    {
        moveVector = moveAction.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpTrigger = true;
    }

    private void OnJumpCancel(InputAction.CallbackContext context)
    {
        jumpDisabled = true;
    }

}
