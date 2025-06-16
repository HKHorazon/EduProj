using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class AnswerIDList
{
    [SerializeField]
    public List<int> correctIds;
}

public class GameMap : MonoBehaviour
{
    
    public GameObject[] ObjToPush;
    //public string [] wall;
    public Grid grid;
    public Tilemap tilemap;
    public Vector3 Wposition = new Vector3(0,0,0);

    public List<AnswerIDList> answerIdList;

    // Start is called before the first frame update
    void Start()
    {         
        
    }

    public bool block(Vector3 n_direction)
    {
        Vector3Int cellposition = tilemap.WorldToCell(n_direction);
           
        Tile tileM = tilemap.GetTile<Tile>(cellposition);

        
        if (tileM==null) {   return true;    }
        
  
        if (tileM.name.StartsWith(GameManager.WALL_TILE_NAME))
        {
            //Debug.Log(tileM.name);
            return false;
        }
        
        return true;
    }
    public void FindObjPush()
    {
        ObjToPush = GameObject.FindGameObjectsWithTag(GameManager.OBJPUSH_TILE_NAME);
    }
}
