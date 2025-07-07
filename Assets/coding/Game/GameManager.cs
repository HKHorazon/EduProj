using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{

    string MAP_PATH_BASIC = "GameMap";

    public const string PLAYER_PREFAB_NAME = "Player";
    public const string OBJPUSH_PREFAB_NAME = "ObjToPush";

    public const string PLAYER_TILE_NAME = "player";
    public const string OBJPUSH_TILE_NAME = "objpush";
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
        StartNewGame();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            ShowInGameDialog();
        }
    }

    private void StartNewGame()
    {
        string mapName = DataStore.Instance.CurrentMapName;
        if (mapName == null || mapName == "")
        {
            mapName = DataStore.FIRST_GAME_LEVEL;
        }
        Debug.Log($"Start MapName = {mapName}");
        StartCoroutine(Initalize(mapName));
    }

    public void RestartGame()
    {
        string mapName = DataStore.Instance.CurrentMapName;
        if (mapName == null || mapName == "")
        {
            mapName = DataStore.FIRST_GAME_LEVEL;
        }

        StartCoroutine(RestartGameCoroutine());

        Debug.Log($"RESTART MapName = {mapName}");
        //TODO
    }

    private IEnumerator RestartGameCoroutine()
    {
        string mapName = DataStore.Instance.CurrentMapName;
        if (mapName == null || mapName == "")
        {
            mapName = DataStore.FIRST_GAME_LEVEL;
        }

        DialogManager.Instance.Show<Loading_Dialog>();
        this.player.ControlEnable = false;

        yield return new WaitForSeconds(0.3f);
        ClearMapAndPlayer();
        yield return new WaitForSeconds(0.3f);
        yield return Initalize(mapName);
        yield return new WaitForSeconds(0.3f);

        DialogManager.Instance.Hide<Loading_Dialog>();
        this.player.ControlEnable = true;
    }

    public void ShowInGameDialog()
    {
        this.player.ControlEnable = false;

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

    #region Map Init
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
        Debug.Log($"MapLoader Load Over = {mapLoader.GMap}");

        gameMap = mapLoader.GMap;

        yield return null;

        List < Push> allBoxs= CreateAllBox();
        Debug.Log($"Create Box count={allBoxs.Count}");

        yield return null;

        gameMap.SetAllBoxes(allBoxs);
        Debug.Log($"Find Boxs count= {gameMap.AllBoxes.Count}");

        yield return null;

        CreatePlayer();
        if (this.player != null) { this.player.ControlEnable = false; }
        Debug.Log($"Create Player Over {this.player}");

        yield return null;



        yield return new WaitForSeconds(1f);
        this.player.ControlEnable = true;

        DialogManager.Instance.Hide<Loading_Dialog>();
        //blackLoadingScene.gameObject.SetActive(false);
    }

    private void CreatePlayer()
    {
        if(gameMap == null) { return ; }

        Vector3 WorldLocCorrection = new Vector3(0.5f, 0.5f, 0);
        GameObject prefabPlayer = Resources.Load<GameObject>(PLAYER_PREFAB_NAME);

        foreach (var spawn in CheckItemLoca(PLAYER_TILE_NAME))
        {
            GameObject objPlayer = Instantiate(prefabPlayer , spawn + WorldLocCorrection , Quaternion.identity);
            this.player = objPlayer?.GetComponent<PlayerMovement>();
        }
    }

    private List<Push> CreateAllBox()
    {
        List<Push> allBoxes = new List<Push>();
        if (gameMap == null) { return allBoxes; }
        //TODO: Create Box using "TileMap"

        Vector3 WorldLocCorrection = new Vector3(0.5f, 0.5f, 0);
        GameObject objpush = Resources.Load<GameObject>(OBJPUSH_PREFAB_NAME);
        int id = 0;
        int count = 0;

        foreach (var spawn in CheckItemLoca(OBJPUSH_TILE_NAME))
        {
            GameObject obj = (GameObject)Instantiate(
                objpush, spawn + WorldLocCorrection, Quaternion.identity
            );

            if (obj == null) { continue; }
            Push push = obj.GetComponent<Push>();
            if (push == null) { continue; }

            allBoxes.Add(push);
            count++;

            //Version 1: ID in sequence
            push.ID = id;
            id++;

            //Version 2: Load Name

        }
        return allBoxes;
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

        Dictionary<string, int> temp = new Dictionary<string, int>();

        BoundsInt bounds = gameMap.tilemap.cellBounds;

        List<Vector3> list = new List<Vector3>();
        foreach (var position in bounds.allPositionsWithin)
        {          
            Tile tile = gameMap.tilemap.GetTile<Tile>(position);

            if (tile != null)
            {
                if (!temp.ContainsKey(tile.name)) { temp[tile.name] = 0; }
                temp[tile.name] += 1;

                if (tile.name.StartsWith(itemName))
                {
                    //Debug.Log(position);
                    // Debug.Log(tile.name);              
                    list.Add(gameMap.tilemap.CellToWorld(position));
                    gameMap.tilemap.SetTile(position, null);


                }
            }
    
        }

        foreach(var kvp in temp) 
        {
            Debug.Log($"Tile Name = {kvp.Key} Count = {kvp.Value}");
        }

        Debug.Log($"CheckItemLoca with itemName = {itemName} list={list.Count}");

        //Debug.LogError("Find Noting");
        return list;
    }
    
    #endregion



    #region Game Logic

    public void CheckVictory()
    {

    }

    #endregion
}
