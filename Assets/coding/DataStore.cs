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

    public string CurrentMapName = "";
}
