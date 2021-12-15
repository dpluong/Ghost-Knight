using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D myRigidBody;
    private Animator animator;
    private BoxCollider2D spikeCollider;

    private bool isFalling;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spikeCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFalling) {
            if (spikeCollider.IsTouchingLayers(LayerMask.GetMask("Foreground"))) {
                myRigidBody.bodyType = RigidbodyType2D.Static;
                this.gameObject.layer = LayerMask.NameToLayer("Foreground");
                GameObject child = gameObject.transform.Find("FallingArea").gameObject;
                child.SetActive(false);
            }
        }

        
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            myRigidBody.gravityScale = 1;
            animator.SetBool("fall", true);
        }
        if (collision.gameObject.tag == "Ground")
        {
            isFalling = true;
        }
    }

    
}
