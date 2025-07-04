using System;
using TMPro;
using UnityEngine;

[ResPath("Dialog/YesNo_Dialog")]
public class YesNo_Dialog : DialogBase
{
    [SerializeField] private TextMeshProUGUI desc;
    private Action<bool> OnClick = null;

    public  void SetData(string title, string desc, Action<bool> OnClick)
    {
        this.desc.text = desc;
        this.OnClick = OnClick;

    }

    public void OnClickYes()
    {
        this.OnClick?.Invoke(true);
        Hide();
    }

    public void OnClickNo()
    {
        this.OnClick?.Invoke(false);
        Hide();
    }
}
