using System;
using TMPro;
using UnityEngine;

public class YesNo_Dialog : DialogBase
{
    [SerializeField] private TextMeshProUGUI desc;
    private Action<bool> OnClick = null;

    public  void Show(string title, string desc, Action<bool> OnClick)
    {
        this.desc.text = desc;
        this.OnClick = OnClick;

        base.Show();
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
