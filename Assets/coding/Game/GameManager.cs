using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    const string PLAYER_PREFAB_PATH = "Player";
    string MAP_PATH_BASIC = "GameMap";
    const string OBJPUSH_PREFAB_PATH = "ObjToPush";

    [field:SerializeField] public GameMap gameMap {  get; private set; }
    [field: SerializeField] public GameObject blackLoadingScene { get; private set; }

    private MapLoader mapLoader = null;
    private PlayerMovement player = null;

    private static GameManager mInstance = null;
    public static GameManager Instance
    {
        get
        {
            return mInstance;
        }
    }

    void Start()
    {
        mInstance = this;
        mapLoader = GetComponent<MapLoader>();
        string mapName = DataStore.Instance.CurrentBattleScene;
        if(mapName == null || mapName == "")
        {
            mapName = DataStore.FIRST_GAME_LEVEL;
        }
        StartCoroutine(Initalize(mapName));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            SceneManager.LoadScene(DataStore.MAIN_SCENE);
        }
    }

    IEnumerator Initalize(string mapFileName)
    {
        blackLoadingScene.gameObject.SetActive(true);


        if (mapLoader == null)
        {
            Debug.LogError("Cannot Find MapLoader");
            yield break;
        }

        ClearMapAndPlayer();



        //Create Map: Using Coroutine
        bool isFinish = false;

        mapLoader.Clear();
        StartCoroutine(mapLoader.CreateMap(mapFileName));
        while (!isFinish)
        {
            yield return new WaitForSeconds(0.1f);
            if (mapLoader.GMap != null)
            {
                isFinish = true;
            }
        }

        gameMap = mapLoader.GMap;

        yield return null;

        CreatePlayerAndBox();
        this.player.ControlEnable = false;

        yield return null;

        gameMap.FindObjPush();

        yield return new WaitForSeconds(0.2f);
        this.player.ControlEnable = true;

        blackLoadingScene.gameObject.SetActive(false);
    }



    void CreatePlayerAndBox()
    {
        if(gameMap == null) { return; }

        
        GameObject prefabPlayer = Resources.Load<GameObject>(PLAYER_PREFAB_PATH);
        GameObject objpush = Resources.Load<GameObject>(OBJPUSH_PREFAB_PATH);

        //TODO: Create Player using "TileMap"
        Vector3 WorldLocCorrection = new Vector3(0.5f,0.5f,0);

        foreach (var spawn in CheckItemLoca("player"))
        {
            GameObject objPlayer = Instantiate(prefabPlayer , spawn + WorldLocCorrection , Quaternion.identity);
            this.player = objPlayer?.GetComponent<PlayerMovement>();
        }

        //TODO: Create Box using "TileMap"
        int id = 0;

        foreach (var spawn in CheckItemLoca("objpush"))
        {


            Instantiate(

                objpush, spawn + WorldLocCorrection , Quaternion.identity

            ).GetComponent<Push>().ID=id;          
            id++;

            //Debug.LogError($"spawn = {spawn}, id = {id}");
        }   
    }


    public void ClearMapAndPlayer()
    {
        mapLoader.Clear();

        if(this.gameMap != null)
        {
            GameObject.Destroy(this.gameMap.gameObject);
            gameMap = null;
        }

        if(this.player != null)
        {
            GameObject.Destroy(this.player.gameObject);
            this.player = null;
        }

        //TODO: Remove Box if needed

    }

    List<Vector3> CheckItemLoca(string item)
    {
        BoundsInt bounds = gameMap.tilemap.cellBounds;

        List<Vector3> list = new List<Vector3>();
        foreach (var position in bounds.allPositionsWithin)
        {          
            Tile tile = gameMap.tilemap.GetTile<Tile>(position);
 
            if (tile!=null&&tile.name==item)
            {
                //Debug.Log(position);
                // Debug.Log(tile.name);              
                list.Add(gameMap.tilemap.CellToWorld(position));
                gameMap.tilemap.SetTile(position, null);
            }
        }
        //Debug.LogError("Find Noting");
        return list;
    }
}
