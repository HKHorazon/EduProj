using System;
using TMPro;
using UnityEngine;

[ResPath("Dialog/Loading_Dialog")]
public class Loading_Dialog : DialogBase
{
    public override void Show(bool immediatly = false)
    {
        Debug.Log("~~SHOW Loading~~");
        base.Show(true);
    }

    protected void HideLoading()
    {
        Debug.Log("~~HIDE Loading~~");
        base.SelfHide(true);
    }

}
