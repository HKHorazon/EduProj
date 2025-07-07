using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MovingObject
{
 
    private Vector2 [] movementSave;
    private bool ReadyToMove;
    int step;
    public bool ControlEnable { get; set; } = false;


    // Start is called before the first frame update
    void Start()
    {
      
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!ControlEnable) { return; }

        Vector2 moveinput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        moveinput.Normalize();

        if (moveinput.sqrMagnitude > 0.5)
        {
            if (ReadyToMove)
            {
                ReadyToMove = false;

                Move(moveinput);
            }
        }
        else
        {
            ReadyToMove = true;
        }
    }

    public bool Move(Vector2 direction)
    {

        SoundManager.SoundInstance.PlayWalkSoundEffect();
       
        //GameManager.Instance.gameMap.FindObjPush();

        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;   
        }
        else
        {
            direction.y = 0;
        }

        direction.Normalize();

        if(Blocked(transform.position,direction,false))
        {
            return false;
        }
        else
        {
            MovementAnimation(direction);
            GameManager.Instance.CheckVictory();
            return true;
        }
    }

}
