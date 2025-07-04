using System;
using TMPro;
using UnityEngine;

[ResPath("Dialog/Loading_Dialog")]
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

    protected void HideLoading()
    {
        Debug.Log("~~HIDE Loading~~");
        base.SelfHide(false);
    }

}
