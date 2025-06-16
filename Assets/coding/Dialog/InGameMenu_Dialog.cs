using System;
using TMPro;
using UnityEngine;

public class InGameMenu_Dialog : DialogBase
{

    Action onDialogClose = null;

    public void Show(Action onDialogClose)
    {
        this.onDialogClose = onDialogClose;
        base.Show();
    }

    public override void Hide()
    {
        onDialogClose?.Invoke();
        base.Hide();
    }

    public void OnClick_Return()
    {
        Hide();
    }

    public void OnClick_Options()
    {
    }

    public void OnClick_GoBack()
    {
        DialogManager.Instance.YesNoDialog.Show(
            "",
            "Are you sure you want to go back MENU?",
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    Hide();
                    GameManager.Instance.GoBackMenu();
                }
            }
        );
    }
}
