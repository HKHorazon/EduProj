using System;
using TMPro;
using UnityEngine;

[ResPath("Dialog/InGameMenu_Dialog")]
public class InGameMenu_Dialog : DialogBase
{

    Action onDialogClose = null;

    public TextMeshProUGUI stageText;

    public void SetData(Action onDialogClose)
    {
        string stageID = DataStore.Instance.GetChineseText(DataStore.Instance.CurrentStageId);
        this.stageText.SetText($"當前關卡：第 {stageID} 關");
        this.onDialogClose = onDialogClose;
    }


    public void OnClick_Return()
    {
        onDialogClose?.Invoke();
        base.Hide();
    }

    public void OnClick_RestartGame()
    {
        YesNo_Dialog yesNo = DialogManager.Instance.Show<YesNo_Dialog>();
        yesNo.SetData(
            "", 
            ConstString.LEAVE_GAME_PROMT,
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    Hide();
                    GameManager.Instance.RestartGame();
                }
                else
                {
                    DialogManager.Instance.Show<InGameMenu_Dialog>();
                }
            }
        );
    }

    public void OnClick_Options()
    {
        OptionMenu_Dialog menu = DialogManager.Instance.Show<OptionMenu_Dialog>();
        menu.InitData(delegate ()
        {
            DialogManager.Instance.Show<InGameMenu_Dialog>();
        });
    }

    public void OnClick_GoBack()
    {
        YesNo_Dialog dialog = DialogManager.Instance.Show<YesNo_Dialog>();


        dialog.SetData(
            "",
            ConstString.RETURN_MENU_PROMT,
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    SelfHide(true);
                    DataStore.Instance.LoadMenuSceneFromPlay();
                }
                else
                {
                    DialogManager.Instance.Show<InGameMenu_Dialog>();
                }
            }
        );
    }
}
