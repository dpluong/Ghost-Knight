using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns.State;

[RequireComponent(typeof(PlayerInput))]
public class CharacterController : MonoBehaviour
{
    private Rigidbody2D myRigidbody; public Rigidbody2D MyRigidbody => myRigidbody;
    private Transform myTransform;
    private Animator myAnimator; public Animator MyAnimator => myAnimator;
    private BoxCollider2D bottomCollider;
    private StateMachine playerStateMachine; public StateMachine PlayerStateMachine => playerStateMachine;
    private PlayerInput playerInput; public PlayerInput PlayerInput => playerInput;

    [Header("Move Configuration")]
    [SerializeField] float speed = 8f;
    private bool isFacingRight = true;

    [Header("Jump Configuration")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float buttonHoldingTime = 0.3f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float cancelRate = 100f; 
    private float jumpTime;
    private bool jumping;
    private bool jumpCancelled;
    public float lastTimeGrounded;
    [SerializeField] float rememberGroundedFor = 0.2f;
    private bool onGround; public bool IsGrounded => onGround;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        bottomCollider = GetComponent<BoxCollider2D>();
        playerStateMachine = new StateMachine(this);
        playerInput = GetComponent<PlayerInput>();
    }

    void Start() 
    {
        playerStateMachine.Initialize(playerStateMachine.idleState);        
    }

    void Update()
    {
        playerStateMachine.Update();

        CountJumpingTime();
    }

    void FixedUpdate()
    {
        CheckIfGrounded();
        Move();
        Jump();
        AdjustJumpHeight();
    }

    private void CheckIfGrounded()
    {
        onGround = Physics2D.IsTouchingLayers(bottomCollider, groundLayers);
    }

    private void AdjustJumpHeight()
    {
        if (jumpCancelled && jumping && myRigidbody.velocity.y > 0)
        {
            myRigidbody.AddForce(Vector2.down * cancelRate);
            jumpCancelled = false;
        }
    }

    private void CountJumpingTime()
    {
        if (jumping)
        {
            jumpTime += Time.deltaTime;
            
            if (playerInput.jumpDisabled)
            {
                jumpCancelled = true;
                playerInput.jumpDisabled = false;
            }

            if (jumpTime > buttonHoldingTime)
            {
                jumping = false;
            }
        }
    }

    private void Jump()
    {
        if (playerInput.jumpTrigger && onGround && Time.time - lastTimeGrounded >= rememberGroundedFor)
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * myRigidbody.gravityScale));
            myRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumping = true;
            jumpCancelled = false;
            jumpTime = 0;
            playerInput.jumpDisabled = false;
            playerInput.jumpTrigger = false;
        }

        if (!onGround)
        {
            playerInput.jumpTrigger = false;
        }
    }
    
    private void Move()
    {
        myRigidbody.velocity = new Vector2 (playerInput.moveVector.x * speed, myRigidbody.velocity.y);
        Flip();
    }

    private void Flip()
    {
        if ((isFacingRight && playerInput.moveVector.x < 0f) || (!isFacingRight && playerInput.moveVector.x > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
