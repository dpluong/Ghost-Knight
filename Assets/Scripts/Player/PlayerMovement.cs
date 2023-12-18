using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


/**
    Class that manipulate movement of the character, A: left, D: right, W: jump, Shift: dash, Space: attack
*/


public class PlayerMovement : MonoBehaviour
{
    [Header("Keyboard Input")]
    const UnityEngine.KeyCode JUMP = KeyCode.W;
    const UnityEngine.KeyCode LEFT = KeyCode.A;
    const UnityEngine.KeyCode RIGHT = KeyCode.D;

    [Header("Player movement stats")]
    Rigidbody2D rigidBody;
    Collider2D myBoxCollider;  
    Collider2D myCapSuleCollider;
    BoxCollider2D triggerLever;
    PlayerMovement player;
    public ParticleSystem myDust;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float startDashTime;
    //[SerializeField] private float dashCoolDown;
    private float dashTime;
    private float horizontalMove = 0f;

    [Header("Platforms")]
    GameObject movableObject;
    Rigidbody2D objectRigidBody;

    [Header("Check ground")]
    private bool isGrounded = false;
    [SerializeField] private Transform isGroundedChecker;
    [SerializeField] private float checkGroundRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rememberGroundedFor;
    private float lastTimeGrounded;
    private float lastTimeStartRun;

    [Header("Adjust floaty jump")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Flags")]
    private bool facingRight = true;
    private bool isDashing = false;
    private bool hasDashed = false;
    private bool isDead = false;
    private bool isOnAir = false;
    public static bool isInputEnabled = true;
    private bool dashInput = false;
    //private bool isPushing = false;

    // Animator
    public Animator animator;
    // Make ghost 
    public Ghost ghost;

    [Header("Attack")]
    public Transform attackPoint;
    public float attachRange = 0.5f;
    public LayerMask enemyLayer;
    public int attackDamage = 30;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    [Header("UI")]
    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;
    


    // Debug dash distance
    
    float distanceTravelled = 0;
    Vector3 lastPosition;

    void Start()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myCapSuleCollider = GetComponent<CapsuleCollider2D>();
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
        dashTime = startDashTime;
        movableObject = GameObject.FindGameObjectWithTag("MovableObject");
        if (movableObject != null)
        {
            objectRigidBody = movableObject.GetComponent<Rigidbody2D>();
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        if (this.transform.Find("TriggerLever") != null)
        {
            triggerLever = this.transform.Find("TriggerLever").GetComponent<BoxCollider2D>();
        }
    }

    void FixedUpdate() {
        if (!isDead && isInputEnabled)
        {
            if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
            {
                isOnAir = true;
            }
            else
            {
                isOnAir = false;
                hasDashed = false;
            }

            if (dashInput && !isDashing)
            {
                if (!hasDashed || !isOnAir)
                {
                    lastPosition = transform.position;
                    DashMove();
                }
            }

            if (isDashing)
            {
                dashTime -= Time.deltaTime;
            }

            if (dashTime <= 0)
            {
                dashTime = startDashTime;
                rigidBody.velocity = Vector2.zero;
                distanceTravelled += Vector3.Distance(transform.position, lastPosition);
                //Debug.Log("dash distance is " + distanceTravelled);
                distanceTravelled = 0;

                if (isOnAir)
                {
                    hasDashed = true;
                }

                isDashing = false;
                ghost.makeGhost = false;
                rigidBody.gravityScale = 4;
                dashInput = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && isInputEnabled)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (!isDashing)
            {
                Move();
            }
            JumpBoost();
            Jump();
            //JumpFixed();
            CheckIfGrounded();
            TriggerJumpAnimation();
            // Control Dashing 
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashInput = true;
            }
            //Player attack
            if (Time.time >= nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }

        }
    }

    private void Move()
    {
        rigidBody.velocity = new Vector2(horizontalMove, rigidBody.velocity.y);
        if (facingRight && horizontalMove < 0)
        {
            Flip();
        }
        else if (!facingRight && horizontalMove > 0)
        {
            Flip();
        }
        if (isGrounded)
        {
            animator.SetFloat("speed", Mathf.Abs(horizontalMove));
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(JUMP) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor))
        {
            if (!isOnAir)
            {
                CreateDust();
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);          
            }

        }
    }

    private void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }



    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        CreateDust();
    }

    private void TriggerJumpAnimation()
    {
        if (!isGrounded && rigidBody.velocity.y > 0)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("jumpUp", true);
            animator.SetBool("jumpDown", false);
        }
        else if (!isGrounded && rigidBody.velocity.y < 0)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("jumpDown", true);
            animator.SetBool("jumpUp", false);

        }
        else if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void DashMove() 
    {
        isDashing = true;
        rigidBody.gravityScale = 0;
        rigidBody.velocity = Vector2.zero;
        if (facingRight) 
        {
            rigidBody.velocity = new Vector2(dashSpeed, 0f);
        } 
        else 
        {
            rigidBody.velocity = new Vector2(dashSpeed * -1, 0f);
        }
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        ghost.makeGhost = true;
        yield return new WaitForSeconds(0.5f);
    }


    private void JumpFixed()
    {
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y > 0 && !Input.GetKey(JUMP))
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Attack() 
    {   
        animator.SetTrigger("attack");
        if (triggerLever != null)
        {
            triggerLever.enabled = true;
        }
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attachRange, enemyLayer);

        foreach (Collider2D enemy in hits)
        {
            if (enemy.isTrigger == false)
            {
                enemy.GetComponent<EnemyMovement>().TakeDamage(attackDamage);
               // enemy.GetComponentInChildren<SlimeController>().TakeDamage(attackDamage);
            }
        }
        if (triggerLever != null)
        {
            StartCoroutine(WaitForAttackAnimation());
        }
    }

    private void OnDrawGizmos() {
        if (attackPoint == null) 
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attachRange);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "MovableObject")
        {
            if (objectRigidBody.velocity.x > 0.1 || objectRigidBody.velocity.x < -0.1)
            {
                animator.SetBool("push", true);
            }
        }

        if (collision.gameObject.tag == "Enemy")
        {
            int damage = collision.gameObject.GetComponent<EnemyMovement>().attackDamage;
            TakeDamage(damage);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        if (collision.gameObject.tag == "Spikes") 
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic) {
                Die();
            } 
        }

        if (collision.gameObject.tag == "TrapArea")
        {
            Die();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "MovableObject")
        {
            objectRigidBody.velocity = new Vector2(0f, 0f);
            //Debug.Log("push disabled");
            animator.SetBool("push", false);
        }
        
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void Die()
    {
        isDead = true;
        rigidBody.bodyType = RigidbodyType2D.Static;
        myBoxCollider.enabled = false;
        myCapSuleCollider.enabled = false;
        animator.SetBool("jumpUp", false);
        animator.SetBool("jumpDown", false);
        animator.SetTrigger("dying");
        StartCoroutine(WaitForDieAnimation());

    }

    IEnumerator WaitForDieAnimation()
    {
       yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        this.gameObject.SetActive(false);
    }

    IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        triggerLever.enabled = false;
    }

    void CreateDust()
    {
        myDust.Play();
    }

    void JumpBoost()
    {
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("JumpBoost")))
        {
            CreateDust();
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce * 1.3f);
        }
    }
}
