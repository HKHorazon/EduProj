using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{

    string MAP_PATH_BASIC = "GameMap";

    public const string PLAYER_PREFAB_NAME = "Player";

    public const string PLAYER_TILE_NAME = "player";
    public const string OBJPUSH_TILE_NAME = "ObjToPush";
    public const string WALL_TILE_NAME = "block";

    [field:SerializeField, ReadOnly] public GameMap gameMap {  get; private set; }
    //[field: SerializeField] public GameObject blackLoadingScene { get; private set; }

    [SerializeField, ReadOnly] private MapLoader mapLoader = null;
    [SerializeField, ReadOnly] private PlayerMovement player = null;

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
        Debug.Log($"Start loading MapName = {mapName}");
        StartCoroutine(Initalize(mapName));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            ShowInGameDialog();
        }
    }

    public void ShowInGameDialog()
    {
        this.player.ControlEnable = false;
        //Time.timeScale = 0f;

        InGameMenu_Dialog dialog = DialogManager.Instance.Show<InGameMenu_Dialog>();
        dialog.SetData(delegate ()
        {
            //Time.timeScale = 1f;
            this.player.ControlEnable = true;
        });
    }

    public void GoBackMenu()
    {
        DialogManager.Instance.Show<Loading_Dialog>();
        SceneManager.LoadScene(DataStore.MAIN_SCENE);
    }

    IEnumerator Initalize(string mapFileName)
    {
        DialogManager.Instance.Show<Loading_Dialog>();
        //blackLoadingScene.gameObject.SetActive(true);


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

        yield return new WaitForSeconds(1f);
        this.player.ControlEnable = true;

        DialogManager.Instance.Hide<Loading_Dialog>();
        //blackLoadingScene.gameObject.SetActive(false);
    }



    void CreatePlayerAndBox()
    {
        if(gameMap == null) { return; }

        
        GameObject prefabPlayer = Resources.Load<GameObject>(PLAYER_PREFAB_NAME);
        GameObject objpush = Resources.Load<GameObject>(OBJPUSH_TILE_NAME);

        //TODO: Create Player using "TileMap"
        Vector3 WorldLocCorrection = new Vector3(0.5f,0.5f,0);

        foreach (var spawn in CheckItemLoca(PLAYER_TILE_NAME))
        {
            GameObject objPlayer = Instantiate(prefabPlayer , spawn + WorldLocCorrection , Quaternion.identity);
            this.player = objPlayer?.GetComponent<PlayerMovement>();
        }

        //TODO: Create Box using "TileMap"
        int id = 0;

        foreach (var spawn in CheckItemLoca(OBJPUSH_TILE_NAME))
        {
            GameObject obj = (GameObject)Instantiate(

                objpush, spawn + WorldLocCorrection, Quaternion.identity

            );

            if(obj == null) { continue; }
            Push push = obj.GetComponent<Push>();
            if(push == null) { continue; }

            //Version 1: ID in sequence
            push.ID = id;
            id++;

            //Version 2: Load Name

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

    List<Vector3> CheckItemLoca(string itemName)
    {
        BoundsInt bounds = gameMap.tilemap.cellBounds;

        List<Vector3> list = new List<Vector3>();
        foreach (var position in bounds.allPositionsWithin)
        {          
            Tile tile = gameMap.tilemap.GetTile<Tile>(position);
 
            if (tile!=null && tile.name.StartsWith(itemName))
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
