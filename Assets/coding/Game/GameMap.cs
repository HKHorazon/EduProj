using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMap : MonoBehaviour
{
    
    public GameObject[] ObjToPush;
    public string [] wall;
    public Grid grid;
    public Tilemap tilemap;
    public Vector3 Wposition = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {         
        
    }
    public bool block(Vector3 n_direction)
    {
        Vector3Int cellposition = tilemap.WorldToCell(n_direction);
           
        Tile tileM = tilemap.GetTile<Tile>(cellposition);

        
        if (tileM==null) {   return true;    }
        
        foreach (var wallname in wall)
        {
            if (tileM.name == wallname)
            {
                //Debug.Log(tileM.name);
                return false;
            }
        }  
        return true;
    }
    public void FindObjPush()
    {
        ObjToPush = GameObject.FindGameObjectsWithTag("ObjToPush");
    }
}
