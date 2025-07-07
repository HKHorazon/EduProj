using UnityEngine;
using UnityEngine.SceneManagement;


[ResPath("Panel/MainUI_Panel")]
public class MainUI_Panel : PanelBase
{
    private string levelToLoad;


    public void OnClick_NewGame()
    {
        //LoadGamePlayScene(DataStore.FIRST_GAME_LEVEL);

        NewGame_Dialog dialog = DialogManager.Instance.Show<NewGame_Dialog>();
        dialog.Init(delegate (int stage)
        {
            string stageName = $"Stage{stage.ToString("00")}";
            Debug.Log($"stageName = {stageName}");
            LoadGamePlayScene(stageName);
        });
    }

    public void OnClick_LoadGame()
    {
        levelToLoad = PlayerPrefs.GetString("SavedLevel");

        if (levelToLoad == null || levelToLoad == "")
        {
            ConfirmOnly_Dialog cDialog = DialogManager.Instance.Show<ConfirmOnly_Dialog>();
            cDialog.SetData(
                "",
                "No Save File Found",
                null
            );
            return;
        }

        YesNo_Dialog ynDialog = DialogManager.Instance.Show<YesNo_Dialog>();
        ynDialog.SetData(
            "",
            "Are you sure you want to load?",
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    levelToLoad = PlayerPrefs.GetString("SavedLevel");
                    LoadGamePlayScene(levelToLoad);
                }
            }
        );
    }

    protected void LoadGamePlayScene(string stageName)
    {
        PanelManager.Instance.HideAllPanels();
        DialogManager.Instance.Show<Loading_Dialog>();
        DataStore.Instance.CurrentMapName = stageName;
        SceneManager.LoadScene(DataStore.BATTLE_SCENE);
    }

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
