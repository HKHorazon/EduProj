using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmOnly_Dialog : DialogBase
{
    [SerializeField] private TextMeshProUGUI desc;
    private Action OnClick = null;

    public void Show(string title, string desc, Action OnClick)
    {
        this.desc.text = desc;
        this.OnClick = OnClick;

        base.Show();
    }

    public void OnClickButton()
    {
        this.OnClick?.Invoke();
        HideAnimation();
    }


}
