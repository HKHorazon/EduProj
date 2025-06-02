using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    public int x = 0;
    public int y = 0;

    const float CELL_SIZE = 1f;

    
    public void Reposition()
    {
        this.gameObject.transform.position = new Vector3(
            x* CELL_SIZE,
            y* CELL_SIZE
        );
    }
}
