using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Start is called before the first frame update
    Animator myAnimator;
    BoxCollider2D myBoxCollider;
    public bool isTrigger = false;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "TriggerLever")
        {
            Debug.Log("is triggered");
            if (!isTrigger)
            {
                myAnimator.SetBool("isTrigger", true);
                isTrigger = true;
            } else
            {
                myAnimator.SetBool("isTrigger", false);
                isTrigger = false;
            }
        }

    }
}
