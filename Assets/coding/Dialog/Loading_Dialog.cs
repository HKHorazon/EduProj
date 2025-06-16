using System;
using TMPro;
using UnityEngine;

public class Loading_Dialog : DialogBase
{
    public override void Show()
    {
        Debug.Log("~~SHOW Loading~~");
        base.Show();
    }

    protected override void ShowAnimation()
    {
        DialogManager.Instance.ShowDialogHandle(this);
        this.CanvasGroup.interactable = false;
        this.gameObject.SetActive(true);
        this.CanvasGroup.interactable = true;
    }

    public override void Hide()
    {
        Debug.Log("~~HIDE Loading~~");
        base.Hide();
    }

}
