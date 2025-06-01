using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class MovingObject : MonoBehaviour
{
    const float TOTAL_TIME = 0.3F;

    protected bool isMoving = false;
  
    protected void MovementAnimation(Vector2 direction)
    {
       
        if (isMoving) { return; }

        
        StartCoroutine(

            RealMovementAnimation(this.transform.position, direction)
            
        );
        
        
       
        List<int> list = new List<int>();

        var a = list.GetEnumerator();
        foreach (int item in list)
        {

        }

        //transform.Translate(direction);
    }

    private IEnumerator RealMovementAnimation(Vector3 fromPos,Vector2 direction)
    {
        float time = 0f;
        isMoving = true;

        while (time < TOTAL_TIME)
        {
            this.transform.position = new Vector3(
                fromPos.x + direction.x * (time/ TOTAL_TIME),
                fromPos.y + direction.y * (time / TOTAL_TIME),
                fromPos.z
            );
            time += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        this.transform.position = new Vector3(
            fromPos.x + direction.x,
            fromPos.y + direction.y,
            fromPos.z
        );
        isMoving = false;
    }
    
}
