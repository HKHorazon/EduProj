using System;
using TMPro;
using UnityEngine;

[ResPath("Dialog/InGameMenu_Dialog")]
public class InGameMenu_Dialog : DialogBase
{

    Action onDialogClose = null;

    public void SetData(Action onDialogClose)
    {
        this.onDialogClose = onDialogClose;
    }


    public void OnClick_Return()
    {
        onDialogClose?.Invoke();
        base.Hide();
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
            "Are you sure you want to go back MENU?",
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    Hide();
                    GameManager.Instance.GoBackMenu();
                }
                else
                {
                    DialogManager.Instance.Show<InGameMenu_Dialog>();
                }
            }
        );
    }
}
