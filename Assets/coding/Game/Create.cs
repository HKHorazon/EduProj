using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Create : MonoBehaviour
{
    string fileName1 = "Player";
    string fileName = "GameMap";  
    public GameObject GMap { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
                     
    }
    
    public IEnumerator create()
    {
        GameObject player = Resources.Load<GameObject>(fileName1);
        
        GameObject gameItem = Resources.Load<GameObject>(fileName);

        GMap = Instantiate(gameItem);

        yield return null;
     

        //Instantiate(player,GMap.GetComponent<GameMap>().Py_Spawn_local,Quaternion.identity);
       
     
    }

    // Update is called once per frame

}
