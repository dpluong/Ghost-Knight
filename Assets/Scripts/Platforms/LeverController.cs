using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform leftStick;
    Transform rightStick;
    Lever lever;
    private bool isOpen = true;
    void Start()
    {
        leftStick = this.transform.Find("LeftPivot");
        rightStick = this.transform.Find("RightPivot");
        lever = this.transform.Find("Lever").GetComponent<Lever>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lever.isTrigger && isOpen)
        {
            Debug.Log("Open sticks");
            Open();
            isOpen = false;
        } else if (!lever.isTrigger && !isOpen)
        {
            Close();
            isOpen = true;
        }
    }

    void Open()
    {
        leftStick.Rotate(0, 0, -90);
        rightStick.Rotate(0, 0, 90);
    }

    void Close()
    {
        leftStick.Rotate(0, 0, 90);
        rightStick.Rotate(0, 0, -90);
    }
}
