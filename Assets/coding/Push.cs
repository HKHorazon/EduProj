using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Push : MovingObject
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Move(Vector2 direction)
    {
        if (ObjToBlocked(transform.position, direction))//1
        {
            return false;
        }
        else
        {
            MovementAnimation(direction);
            return true;
        }
    }
    public bool ObjToBlocked(Vector3 position, Vector2 direction)
    {
        Vector2 newpos = new Vector2(position.x, position.y) + direction;//2 get self direction

        //check direction can be push or not
        foreach (var obj in GameManager.Instance.gameMap.Obstacles)
        {
           //check obstacles position
            if (obj.transform.position.x == newpos.x && obj.transform.position.y == newpos.y)
            {
                //true =con not moving,End
                return true;
            }
        }
         
        //3 check the box that can push(single box)
        foreach (var objToPush in GameManager.Instance.gameMap.ObjToPush)
        {

            if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
            {
                //push muti box on/off(true)
                //return true;
            }
        }
        //3 check the box that can push(muti box)
        if (MutiBoxPush(direction))
        {
            return false;
        }

        return true;
        
    }
    public bool MutiBoxPush(Vector2 direction)
    {
        //get self direction
        Vector2 newpos1 = new Vector2(transform.position.x, transform.position.y) + direction;
        
        foreach (var objToPush in GameManager.Instance.gameMap.ObjToPush)
        {

            Vector2 newpos = new Vector2(objToPush.transform.position.x, objToPush.transform.position.y) + direction;
            //compare that current cube new position and checking each object can push on scene
            //if current cube has same new position with world object can push on scene,scene obj will be push by player direction

            if (objToPush.transform.position.x == newpos1.x && objToPush.transform.position.y == newpos1.y)
            {
                //create pushcomponent of objtopush 
                Push objPush = objToPush.GetComponent<Push>();

                //Push Check for the box gona be push
                if (objPush && objPush.Move(direction))
                {

                }
                else//if be blocked will break out and not process
                {
                    return false;
                }

            }

        }return true;//process animation          
            
                 
    }

}
