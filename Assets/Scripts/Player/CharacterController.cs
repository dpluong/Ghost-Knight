using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DesignPatterns.State;
using System;

[RequireComponent(typeof(PlayerInput))]
public class CharacterController : MonoBehaviour
{
    public InputActionAsset actions;
    public Rigidbody2D MyRigidbody => myRigidbody;
    public StateMachine PlayerStateMachine => playerStateMachine;
    public bool IsGrounded => onGround;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private BoxCollider2D bottomCollider;
    private InputAction moveAction;
    private InputAction jumpAction;
    private StateMachine playerStateMachine;

    [Header("Move Configuration")]
    [SerializeField] float speed = 8f;
    private bool isFacingRight = true;
    public Vector2 moveVector;

    [Header("Jump Configuration")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float buttonHoldingTime = 0.3f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float cancelRate = 100f;
    private float jumpTime;
    public bool jumping;
    private bool jumpCancelled;
    private bool onGround;


    

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bottomCollider = GetComponent<BoxCollider2D>();

        moveAction = actions.FindActionMap("Player").FindAction("Move");
        jumpAction = actions.FindActionMap("Player").FindAction("Jump");
        jumpAction.performed += OnJump;
        jumpAction.canceled += OnJumpCancel;

        playerStateMachine = new StateMachine(this);
    }

    void Start() 
    {
        playerStateMachine.Initialize(playerStateMachine.idleState);   
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
        PollInputs();
        playerStateMachine.Update();
        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > buttonHoldingTime)
            {
                jumping = false;
            }
        }

    }

    void FixedUpdate()
    {
        onGround = Physics2D.IsTouchingLayers(bottomCollider, groundLayers);
        Move();
        if (jumpCancelled && jumping && myRigidbody.velocity.y > 0)
        {
            myRigidbody.AddForce(Vector2.down * cancelRate);
        }
    }

    private void PollInputs()
    {
        moveVector = moveAction.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (onGround)
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * myRigidbody.gravityScale));
            myRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumping = true;
            jumpCancelled = false;
            jumpTime = 0;
        }
    }

    private void OnJumpCancel(InputAction.CallbackContext context)
    {
        jumpCancelled = true;
    }

    private void Move()
    {
        if (moveVector.x != 0)
        {
            myRigidbody.velocity = new Vector2(moveVector.x * speed, myRigidbody.velocity.y);
            Flip();
        }
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
}
