using UnityEngine;
using UnityEngine.SceneManagement;

public class DataStore : MonoBehaviour
{
    public static DataStore mInstance = null;
    public static DataStore Instance    
    {
        get 
        {
            if(mInstance == null)
            {
                GameObject obj = new GameObject("DataStore");
                DontDestroyOnLoad(obj);
                mInstance = obj.AddComponent<DataStore>();
            }
            return mInstance;
        } 
    }



    public const string PREF_SAVED_LEVEL = "SavedLevel";
    public const string PREF_DISPLAY_MODE_TAG = "DISPLAY_MODE";
    public const string PREF_RESOLUTION_TAG = "RESOLUTION";
    public const string PREF_SOUND_TAG = "SOUND";
    public const string PREF_BGM_TAG = "BGM";




    public const string MAIN_SCENE = "MainMenu";
    public const string BATTLE_SCENE = "BattleScene";
   


    public const string SFX_BUTTON_CLICK = "Sound/UI_Click";
    public const string SFX_VICTORY = "Sound/Victory";

    public string SFX_PLAYER_MOVE
    {
        get
        {
            int r = Random.Range(1, 5);
            return $"Sound/FOOTSTEP_0{r}";
        }
    }
    //public const string PLAYER_MOVING_SFX_PATH = "Sound/tung";

    public const string BGM_MENU = "BGM/BGM_Menu";
    public const string BGM_INGAME = "BGM/BGM_Ingame";


    #region stage related

    public const int MAX_STAGE_COUNT = 10;
    public const int FIRST_GAME_LEVEL = 1;
    public const string FIRST_GAME_LEVEL_NAME = "Stages/Stage01";
    public int CurrentStageId = 1;
    public string GetMapName()
    {
        return $"Stages/Stage{CurrentStageId.ToString("00")}";
    }
    public void LoadGamePlayScene(int stageID)
    {
        PanelManager.Instance.HideAllPanels();
        DialogManager.Instance.Show<Loading_Dialog>();
        DataStore.Instance.CurrentStageId = stageID;
        SceneManager.LoadScene(DataStore.BATTLE_SCENE);
    }
    
    public void LoadMenuSceneFromPlay()
    {
        PanelManager.Instance.HideAllPanels();
        DialogManager.Instance.Show<Loading_Dialog>();
        SceneManager.LoadScene(DataStore.MAIN_SCENE);
    }

    #endregion
}
