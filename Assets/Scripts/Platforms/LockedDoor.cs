using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _isOpen = false;
    private Animator lockedDoor;
    void Start()
    {
        lockedDoor = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOpen) 
        {
            lockedDoor.SetBool("doorTrigger", true);
        } else 
        {
            lockedDoor.SetBool("doorTrigger", false);
        }
    }
}
