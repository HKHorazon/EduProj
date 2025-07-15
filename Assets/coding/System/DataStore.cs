using UnityEngine;

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



    public const string MAIN_SCENE = "MainMenu";
    public const string BATTLE_SCENE = "BattleScene";
    public const string FIRST_GAME_LEVEL = "Stages/Stage01";


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

    public string CurrentMapName = "";
}
