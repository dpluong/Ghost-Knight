using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterController : MonoBehaviour
{
    public InputActionAsset actions;


    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private InputAction moveAction;
    

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        moveAction = actions.FindActionMap("Player").FindAction("Move");

        //actions.FindActionMap("Player").FindAction("Jump").performed += OnJump;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Jump!");
    }

    private void Move()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Debug.Log(moveVector);
    }

    void OnEnable()
    {
        actions.FindActionMap("Player").Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap("Player").Disable();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }
}
