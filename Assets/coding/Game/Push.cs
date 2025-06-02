using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Push : MovingObject
{
    [field:SerializeField]private bool Push1Box = false;
    public int ID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public bool Move(Vector2 direction)
    {

        if (Blocked(transform.position, direction, Push1Box))//1 push 1 box true/false
        {
            return false;
        }
        else
        {
            MovementAnimation(direction);

            return true;
        }
    }
}
