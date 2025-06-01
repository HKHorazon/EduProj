using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMap : MonoBehaviour
{
    public GameObject[] Obstacles;
    public GameObject[] ObjToPush;

    public Grid grid;
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        ObjToPush = GameObject.FindGameObjectsWithTag("ObjToPush");

        Vector3 v3 = grid.CellToWorld(new Vector3Int(0, 0, 0));
        Debug.Log(v3);
         v3 = grid.CellToWorld(new Vector3Int(2, 0, 0));
        Debug.Log(v3);
         v3 = grid.CellToWorld(new Vector3Int(0, 3, 0));
        Debug.Log(v3);
    }
}
