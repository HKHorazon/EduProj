using UnityEngine;
using UnityEngine.SceneManagement;


[ResPath("Panel/MainUI_Panel")]
public class MainUI_Panel : PanelBase
{
    private string levelToLoad;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            int stage = PlayerPrefs.GetInt(DataStore.PREF_SAVED_LEVEL, 1);
            stage++;
            PlayerPrefs.SetInt(DataStore.PREF_SAVED_LEVEL, stage);
            PlayerPrefs.Save();
        }
    }


    public override void Show()
    {
        base.Show();
        AudioManager.Instance.PlayBGM(DataStore.BGM_MENU);
    }

    public void OnClick_NewGame()
    {
        int stage =  PlayerPrefs.GetInt(DataStore.PREF_SAVED_LEVEL,1);

        NewGame_Dialog dialog = DialogManager.Instance.Show<NewGame_Dialog>();
        dialog.Init(
            stage, 
            delegate (int stage)
            {
           
                DataStore.Instance.LoadGamePlayScene(stage);
            }
        );
    }

    public void OnClick_ResetStages()
    {
        YesNo_Dialog ynDialog = DialogManager.Instance.Show<YesNo_Dialog>();
        ynDialog.SetData(
            "",
            "Are you sure you want to reset all stages?",
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    PlayerPrefs.DeleteKey(DataStore.PREF_SAVED_LEVEL);
                    PlayerPrefs.Save();
                    ConfirmOnly_Dialog cDialog = DialogManager.Instance.Show<ConfirmOnly_Dialog>();
                    cDialog.SetData(
                        "",
                        "All stages have been reset.",
                        null
                    );
                }
            }
        );
    }

    //public void OnClick_LoadGame()
    //{
    //    levelToLoad = PlayerPrefs.GetString(DataStore.PREF_SAVED_LEVEL);

    //    if (levelToLoad == null || levelToLoad == "")
    //    {
    //        ConfirmOnly_Dialog cDialog = DialogManager.Instance.Show<ConfirmOnly_Dialog>();
    //        cDialog.SetData(
    //            "",
    //            "No Save File Found",
    //            null
    //        );
    //        return;
    //    }

    //    YesNo_Dialog ynDialog = DialogManager.Instance.Show<YesNo_Dialog>();
    //    ynDialog.SetData(
    //        "",
    //        "Are you sure you want to load?",
    //        delegate (bool isTrue)
    //        {
    //            if (isTrue)
    //            {
    //                levelToLoad = PlayerPrefs.GetString(DataStore.PREF_SAVED_LEVEL);
    //                LoadGamePlayScene(levelToLoad);
    //            }
    //        }
    //    );
    //}


    public void OnClick_Option()
    {

        //TODO
        OptionMenu_Dialog menu = DialogManager.Instance.Show<OptionMenu_Dialog>();
        menu.InitData(null); 
    }

    public void OnClick_Exit()
    {
        YesNo_Dialog dialog = DialogManager.Instance.Show<YesNo_Dialog>();
        dialog.SetData(
            "",
            "Are you sure you want to leave?",
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    Application.Quit();
                }
            }
        );
    }

}
