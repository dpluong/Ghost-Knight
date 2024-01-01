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

    [Header("Move Configuration")]
    [SerializeField] float speed = 8f;
    private bool isFacingRight = true;
    private Vector2 moveVector;

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
        moveVector = moveAction.ReadValue<Vector2>();
        myRigidbody.velocity = new Vector2 (moveVector.x * speed, myRigidbody.velocity.y);
        Flip();
    }

    private void Flip()
    {
        if ((isFacingRight && moveVector.x < 0f) || (!isFacingRight && moveVector.x > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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
        Move();
    }
}
