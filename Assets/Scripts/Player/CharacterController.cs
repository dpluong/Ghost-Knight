using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DesignPatterns.State;

[RequireComponent(typeof(PlayerInput))]
public class CharacterController : MonoBehaviour
{
    //public InputActionAsset actions;
    
    
    
    
    
    //public float JumpTime => jumpTime;
   // public bool Jumping => jumping;
    //public bool JumpCancelled => jumpCancelled;

    private Rigidbody2D myRigidbody; public Rigidbody2D MyRigidbody => myRigidbody;
    private Transform myTransform; public Transform MyTransform => myTransform;
    private Animator myAnimator;
    private BoxCollider2D bottomCollider;
   // private InputAction moveAction;
    private StateMachine playerStateMachine; public StateMachine PlayerStateMachine => playerStateMachine;
    private PlayerInput playerInput; public PlayerInput PlayerInput => playerInput;

    [Header("Move Configuration")]
    [SerializeField] float speed = 8f; public float Speed => speed;
    //private bool isFacingRight = true;
    //private Vector2 moveVector;

    [Header("Jump Configuration")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float buttonHoldingTime = 0.3f; public float ButtonHoldingTime => buttonHoldingTime;
    [SerializeField] float jumpHeight = 5f; public float JumpHeight => jumpHeight;
    [SerializeField] float cancelRate = 100f; public float CancelRate => cancelRate;
    //private float jumpTime;
    //private bool jumping;
    //private bool jumpCancelled;
    private bool onGround; public bool IsGrounded => onGround;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        bottomCollider = GetComponent<BoxCollider2D>();
        playerStateMachine = new StateMachine(this);
        playerInput = GetComponent<PlayerInput>();

        //moveAction = actions.FindActionMap("Player").FindAction("Move");
    }

    void Start() 
    {
        playerStateMachine.Initialize(playerStateMachine.idleState);        
    }

    void Update()
    {
        playerStateMachine.Update();
        /*
        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > buttonHoldingTime)
            {
                jumping = false;
            }
        }*/
    }

    void FixedUpdate()
    {
        onGround = Physics2D.IsTouchingLayers(bottomCollider, groundLayers);
        //Move();
        playerStateMachine.FixedUpdate();
        
        
        /*if (jumpCancelled && jumping && myRigidbody.velocity.y > 0)
        {
            myRigidbody.AddForce(Vector2.down * cancelRate);
        }*/
    }


/*
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
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

        if (context.canceled)
        {
            jumpCancelled = true;
        }
    }*/

    /*
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
    }*/  
}
