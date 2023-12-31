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
    private BoxCollider2D bottomCollider;

    private InputAction moveAction;

    [Header("Jump Configuration")]
    [SerializeField] private float jumpForce = 35f;
    [SerializeField] private float gravityScale = 10f;
    [SerializeField] private float fallingGravityScale = 40f;
    [SerializeField] private LayerMask groundLayers;
    private bool onGround = true;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bottomCollider = GetComponent<BoxCollider2D>();

        moveAction = actions.FindActionMap("Player").FindAction("Move");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (onGround)
            {
                myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
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
        onGround = Physics2D.IsTouchingLayers(bottomCollider, groundLayers);
    }
}
