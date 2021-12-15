using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] float speed = 0.03f;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    bool directionChanged = false;
    // Start is called before the first frame upda.te
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!directionChanged)
        {
           transform.Translate(Vector2.right * speed);
            
        }
        else
        {
            transform.Translate(Vector2.left * speed);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (directionChanged)
            {
                directionChanged = false;
            }
            else
            {
                directionChanged = true;
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }   
    }
}
