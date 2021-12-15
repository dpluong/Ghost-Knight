using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 90;
    private BoxCollider2D myCollider;
    private bool reverseRotate = false;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        
        //pos = myRigidbody.position;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //pos = myRigidbody.position;
        Rotate();
    }

    void Rotate()
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            //myRigidbody.position = pos;
            //Debug.Log("reverse rotation");
            if (reverseRotate)
            {
                reverseRotate = false;
            } else 
            {
                
                reverseRotate = true;
            }
            
            
        } 

        if (!reverseRotate)
        {
            transform.Rotate(new Vector3(0, 0, degreesPerSecond) * Time.deltaTime);
        } else 
        {
            transform.Rotate(new Vector3(0, 0, degreesPerSecond * -1) * Time.deltaTime);
        }
            
    }

    

}
