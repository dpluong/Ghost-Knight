using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;
    Animator animator;
    public int maxHealth = 50;
    int currentHealth;
    public int attackDamage = 25;
    public EnemyHealthBar healthBar;
    float yPos;
    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        if (this.gameObject.name == "Slime")
        {
            animator = this.gameObject.GetComponentInChildren<Animator>();
        } else
        {
            animator = GetComponent<Animator>();
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = new Vector2(this.transform.position.x, yPos);
        if (IsFacingRight())
        {
            if (this.gameObject.name == "Slime")
            {
                myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            } else
            {
                myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            }

        }
        else
        {
            if (this.gameObject.name == "Slime")
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
            else
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
        }
    }

    bool IsFacingRight()
    {
        if (this.gameObject.name == "Slime")
        {
            return transform.localScale.x < 0;
        }
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (this.gameObject.name == "Slime")
            {
                transform.localScale = new Vector2((Mathf.Sign(myRigidBody.velocity.x)), 1f);
            } else
            {
                transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
        }
    }

    public void TakeDamage(int damage) 
    {
        
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        healthBar.gameObject.SetActive(true);
        if (currentHealth <= 0) 
        {
            Die();
        } else {
            animator.SetTrigger("hurt");
            StartCoroutine(WaitForHurtAnimation());
        }

        
    }

    void Die() 
    {
        
        animator.SetTrigger("isDead");
        //
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        this.enabled = false;
        StartCoroutine(WaitForDieAnimation());
        //Debug.Log("death is triggered");

    }

    IEnumerator WaitForDieAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("death is triggered");
        Destroy(this.gameObject);
    }

    IEnumerator WaitForHurtAnimation()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        healthBar.gameObject.SetActive(false);
    }

}
