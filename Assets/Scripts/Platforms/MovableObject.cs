using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Trigger trigger;
    private Rigidbody2D myRigidBody;
    [SerializeField] private Transform checker;
    public Vector2 checkSize;
    [SerializeField] private LayerMask checkLayer;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        
    }

    private void OnCollisionStay2D(Collision2D collision) 
    {

        
        if (collision.gameObject.tag == "Trigger") 
        {
            if ((CheckPlayer() && Input.GetKey(KeyCode.D)) || (CheckPlayer() && Input.GetKey(KeyCode.A)))
            {

                if (trigger._isTrigger)
                {
                    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
                }
                else
                {
                    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 1f);
                }
            }
        } 
    }    

    private bool CheckPlayer()
    {
        Collider2D collider = Physics2D.OverlapBox(checker.position, checkSize, 0f, checkLayer);
        if (collider != null)
        {
            return true;
        } 
        return false;
    }
}
