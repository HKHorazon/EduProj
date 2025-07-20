using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;


public class GameManager : MonoBehaviour
{

    string MAP_PATH_BASIC = "GameMap";

    public const string PLAYER_PREFAB_NAME = "Player";
    public const string OBJPUSH_PREFAB_NAME = "ObjToPush";

    public const string PLAYER_TILE_NAME = "player";
    public const string OBJPUSH_TILE_NAME = "Box";
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
        if (Input.GetKeyDown(KeyCode.V))
        {
            isVictory = true;
        }
    }

    private void StartNewGame()
    {
        string mapName =  DataStore.Instance.GetMapName();
        if (mapName == null || mapName == "")
        {
            mapName = DataStore.FIRST_GAME_LEVEL_NAME;
        }
        Debug.Log($"Start MapName = {mapName}");

        AudioManager.Instance.PlayBGM(DataStore.BGM_INGAME);
        StartCoroutine(Initalize(mapName));
    }

    public void RestartGame()
    {
        string mapName = DataStore.Instance.GetMapName();
        if (mapName == null || mapName == "")
        {
            mapName = DataStore.FIRST_GAME_LEVEL_NAME;
        }

        StartCoroutine(RestartGameCoroutine());

        Debug.Log($"RESTART MapName = {mapName}");
        //TODO
    }

    private IEnumerator RestartGameCoroutine()
    {
        string mapName = DataStore.Instance.GetMapName();
        if (mapName == null || mapName == "")
        {
            mapName = DataStore.FIRST_GAME_LEVEL_NAME;
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

        List<Push> allBoxs= CreateAllBox(gameMap);
        Debug.Log($"Create Box count={allBoxs.Count}");

        yield return null;

        gameMap.SetAllBoxes(allBoxs);
        Debug.Log($"Find Boxs count= {gameMap.AllBoxes.Count}");

        yield return null;

        CreatePlayer();
        if (this.player != null) { this.player.ControlEnable = false; }
        Debug.Log($"Create Player Over {this.player}");

        yield return null;

        gameMap.Init();


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

        foreach (var tuple in CheckItemLoca(PLAYER_TILE_NAME))
        {
            GameObject objPlayer = Instantiate(
                prefabPlayer , 
                tuple.Item2 + WorldLocCorrection ,
                Quaternion.identity
            );
            this.player = objPlayer?.GetComponent<PlayerMovement>();
        }
    }

    private List<Push> CreateAllBox(GameMap gameMap)
    {
        List<Push> allBoxes = new List<Push>();
        if (gameMap == null) { return allBoxes; }
        //TODO: Create Box using "TileMap"

        Vector3 WorldLocCorrection = new Vector3(0.5f, 0.5f, 0);
        GameObject objpush = Resources.Load<GameObject>(OBJPUSH_PREFAB_NAME);
        int id = 0;
        int count = 0;

        foreach (var tuple in CheckItemLoca(OBJPUSH_TILE_NAME))
        {

            GameObject obj = (GameObject)Instantiate(
                objpush, tuple.Item2 + WorldLocCorrection, Quaternion.identity
            );

            if (obj == null) { continue; }
            Push push = obj.GetComponent<Push>();
            if (push == null) { continue; }

            allBoxes.Add(push);
            count++;

            //Version 1: ID in sequence
            //push.ID = id;
            //id++;

            //Version 2: Load Name
            string[] strs = tuple.Item1.name.Split("_");

            push.sequenceID = count;
            push.ID = 0;
            int tempID = 0;
            if (strs.Length == 2 && int.TryParse(strs[1], out tempID)) 
            {
                push.ID = tempID;
            }
            push.SetText(' ');
            if (gameMap.characterTable.ContainsKey(push.ID))
            {
                push.SetText(gameMap.characterTable[push.ID]);
            }
            
        }
        return allBoxes;
    }

    public void ClearMapAndPlayer()
    {
        mapLoader.Clear();

        if(this.gameMap != null)
        {
            // Remove Box if needed
            foreach (var box in this.gameMap.AllBoxes)
            {
                if (box != null && box.gameObject != null)
                {
                    GameObject.Destroy(box.gameObject);
                }
            }
            this.gameMap.AllBoxes.Clear();
            GameObject.Destroy(this.gameMap.gameObject);
            gameMap = null;
        }

        if(this.player != null)
        {
            GameObject.Destroy(this.player.gameObject);
            this.player = null;
        }

     
    }

    List<Tuple<Tile,Vector3>> CheckItemLoca(string itemName)
    {

        Dictionary<string, int> temp = new Dictionary<string, int>();

        BoundsInt bounds = gameMap.tilemap.cellBounds;

        List<Tuple<Tile, Vector3>> list = new List<Tuple<Tile, Vector3>>();
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
                    Tuple<Tile, Vector3> tuple = new Tuple<Tile, Vector3>(tile, gameMap.tilemap.CellToWorld(position));
                  
                    gameMap.tilemap.SetTile(position, null);

                    list.Add(tuple);
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



    #region Game Process Logic
    public bool isVictory = false;

    public bool CheckVictory()
    {
        bool DebugON = true;

        if(this.gameMap.answerList.Count == 0) { return false; }

        StringBuilder sb = new StringBuilder();
        int victoryCount = 0;

        foreach (var answer in this.gameMap.answerList)
        {
            List<int> idList = this.gameMap.getIDFromOneAnswer(answer);
            List<List<Push>> allCombination = CollectAllCombination(idList);

            foreach (var combination in allCombination)
            {
                if (AreConsecutive(combination))
                {
                    victoryCount++;
                    break;
                }
                if(DebugON) sb.AppendLine(combination.Aggregate("", (current, push) => current + push.sequenceID + ", "));
            }
        }

        if (DebugON)
        {
            sb.AppendLine($"VICTORY : {this.gameMap.answerList.Count} == {victoryCount}");
            Debug.LogError($"{sb.ToString()}");
        }
        return this.gameMap.answerList.Count == victoryCount;
    }

    private List<List<Push>> CollectAllCombination(List<int> idList)
    {
        List<List<Push>> list = new List<List<Push>>();
        if(idList == null || idList.Count == 0) { return list; }

        List<List<Push>> tempList = new List<List<Push>>();
        foreach (int id in idList)
        {
            if (this.gameMap.boxTableByCharacters.ContainsKey(id))
            {
                List<Push> pushes = this.gameMap.boxTableByCharacters[id];
                tempList.Add(pushes);
            }
        }

        int totalCount = 1;
        foreach (List<Push> pushes in tempList)
        {
            totalCount *= pushes.Count;
        }

        for (int i = 0; i < totalCount; i++)
        {
            List<Push> combination = new List<Push>();
            int temp = i;
            for (int j = 0; j < tempList.Count; j++)
            {
                List<Push> pushes = tempList[j];
                int index = temp % pushes.Count;
                combination.Add(pushes[index]);
                temp /= pushes.Count;
            }
            if (combination.Count > 0)
            {
                list.Add(combination);
            }
        }

        return list;
    }

    private bool AreConsecutive(List<Push> pushes)
    {
        bool DebugON = true;

        if(pushes.Count == 0) return false;
        if(pushes.Count == 1) return false;


        EDirection storeDirection = EDirection.NotNeghibor;

        StringBuilder sb = new StringBuilder();

        for (int i=0;i<pushes.Count-1; i++)
        {
            if (DebugON) { sb.Append($"[{pushes[i].sequenceID}{pushes[i].text.text}]"); }

            EDirection newDir = this.gameMap.GetDirection(pushes[i], pushes[i+1]);
            if (newDir == EDirection.NotNeghibor)
            {
                if (DebugON)
                {
                    sb.Append("-FIRST");
                    Debug.Log(sb.ToString());
                }
                return false;
            }
            else if (storeDirection == EDirection.NotNeghibor && newDir != EDirection.NotNeghibor) //first time
            {
                storeDirection = newDir;
            }
            else if(storeDirection != EDirection.NotNeghibor && newDir == storeDirection)
            {
                storeDirection = newDir;
            }
            else 
            {
                if (DebugON)
                {
                    sb.Append($"-BREAK newDir={newDir}, store={storeDirection}");
                    Debug.Log(sb.ToString());
                }
                return false;
            }
        }

        if (DebugON)
        {
            sb.Append("-SUCCESS");
            Debug.Log(sb.ToString());
        }
        return true;
    }


    internal void VictoryFunctions()
    {
        int curStage = DataStore.Instance.CurrentStageId;
        bool isLastStage = curStage >= DataStore.MAX_STAGE_COUNT;
        int storeStage = PlayerPrefs.GetInt(DataStore.PREF_SAVED_LEVEL, 1);

        AudioManager.Instance.PlaySFX("Sound/Victory");

        curStage++;
        DataStore.Instance.CurrentStageId = curStage;
        if (curStage > storeStage)
        {
            PlayerPrefs.SetInt(DataStore.PREF_SAVED_LEVEL, curStage);
            PlayerPrefs.Save();
        }

        Victory_Dialog dialog = DialogManager.Instance.Show<Victory_Dialog>();
        dialog.Init(
            isLastStage,
            delegate () {

                DataStore.Instance.LoadGamePlayScene(curStage);
        });
    }

    #endregion
}
