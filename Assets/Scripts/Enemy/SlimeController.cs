using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    //[SerializeField] float moveSpeed = 1f;
   // Rigidbody2D myRigidBody;
    Animator animator;
    public int maxHealth = 50;
    int currentHealth;
    public int attackDamage = 25;
    public EnemyHealthBar healthBar;
    // Use this for initialization
    void Start()
    {
       // 
       animator = GetComponent<Animator>();
      currentHealth = maxHealth;
     healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
   

   

   

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        healthBar.gameObject.SetActive(true);
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("hurt");
            StartCoroutine(WaitForHurtAnimation());
        }


    }

    void Die()
    {
        animator.SetTrigger("isDead");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        this.enabled = false;
        StartCoroutine(WaitForDieAnimation());
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
