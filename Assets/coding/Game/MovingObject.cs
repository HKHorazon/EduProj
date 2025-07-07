using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class MovingObject : MonoBehaviour
{
    const float TOTAL_TIME = 0.05F;

    protected bool isMoving = false;
  
    protected void MovementAnimation(Vector2 direction)
    {   
        if (isMoving) { return; }  
        
        StartCoroutine(
            RealMovementAnimation(this.transform.position, direction)           
        );       
    }

    private IEnumerator RealMovementAnimation(Vector3 fromPos,Vector2 direction)
    {
        float time = 0f;
        isMoving = true;

        while (time < TOTAL_TIME)
        {
            this.transform.position = new Vector3(
                fromPos.x + direction.x * (time / TOTAL_TIME),
                fromPos.y + direction.y * (time / TOTAL_TIME),
                fromPos.z
            );
            time += Time.deltaTime;
            //yield return new WaitForSeconds(0.05f);
            yield return null;
            
           
        }
        this.transform.position = new Vector3(
            fromPos.x + direction.x,
            fromPos.y + direction.y,
            fromPos.z
        );
       
        isMoving = false;
    }

    public bool Blocked(Vector2 position, Vector2 direction, bool push1box)
    {
        // Vector3Int newpos = new Vector3Int(Mathf.FloorToInt(position.x + direction.x), Mathf.FloorToInt(position.y + direction.y));
        Vector3 newpos = new Vector3(position.x+direction.x,position.y+direction.y,0);

        //Debug.Log("World loc"+newpos);

        //Debug.Log("Cell loc"+GameManager.Instance.gameMap.tilemap.WorldToCell(newpos));

        if (GameManager.Instance.gameMap.block(newpos))
        {
            //Debug.Log("null");
        }
        else
        {
           // Debug.Log("block");
            return true;
        }

        if (push1box)
        {     
            foreach (var objToPush in GameManager.Instance.gameMap.AllBoxes)
            {

                if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
                {
                    //push muti box on/off(true)
                    return true;
                }
            }
        }
        
        foreach (var objToPush in GameManager.Instance.gameMap.AllBoxes)
        {
            
            if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
            {
                Push objPush = objToPush.GetComponent<Push>();

                if (objPush && objPush.Move(direction))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

}
