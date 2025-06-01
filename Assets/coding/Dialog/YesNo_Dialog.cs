using System;
using TMPro;
using UnityEngine;

public class YesNo_Dialog : DialogBase
{
    [SerializeField] private TextMeshProUGUI desc;
    private Action<bool> OnClick = null;

    public void Show(string title, string desc, Action<bool> OnClick)
    {
        this.desc.text = desc;
        this.OnClick = OnClick;

        ShowAnimation();
    }

    public void OnClickYes()
    {
        this.OnClick?.Invoke(true);
        HideAnimation();
    }

    public void OnClickNo()
    {
        this.OnClick?.Invoke(false);
        HideAnimation();
    }
}
