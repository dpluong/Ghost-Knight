using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to collider of the "button", then trigger the "door" to open 
/// </summary>
public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    Collider2D triggerCollider;
    Animator triggerAnimator;
    public bool _isTrigger = false;
    public LockedDoor lockedDoor;
    
    void Start()
    {
        triggerAnimator = GetComponent<Animator>();
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    // Update alway be called by engine, so alway remove this func if we not using
    // void Update()
    // {
        

    // }

    private void OnCollisionStay2D(Collision2D collision) 
    {
        const string PLAYER_LAYER_NAME = "Player";
        if (collision.gameObject.tag == PLAYER_LAYER_NAME || collision.gameObject.tag == "MovableObject") 
        {
            if (triggerCollider.IsTouching(collision.collider))
            {
                
                triggerAnimator.SetBool("press", true);
                _isTrigger = true;
                lockedDoor._isOpen = true;
                triggerCollider.isTrigger = true;
                //StartCoroutine(WaitForNextTouch());
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "MovableObject")
        {
            _isTrigger = false;
            lockedDoor._isOpen = false;
            triggerAnimator.SetBool("press", false);
            triggerCollider.isTrigger = false;
        }
        
    }

    bool IsObjectAllowToTriggerMe(string tag)
    {
        // check lauer name, or get coponent....
        return false;
    }

    IEnumerator WaitForNextTouch()
    {
        Debug.Log("waiting");
        yield return new WaitForSeconds(5);
    }
}
