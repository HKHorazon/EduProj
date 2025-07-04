using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ResPath("Dialog/ConfirmOnly_Dialog")]
public class ConfirmOnly_Dialog : DialogBase
{
    [SerializeField] private TextMeshProUGUI desc;
    private Action OnClick = null;

    public void SetData(string title, string desc, Action OnClick)
    {
        this.desc.text = desc;
        this.OnClick = OnClick;
    }

    public void OnClickButton()
    {
        this.OnClick?.Invoke();
        HideAnimation();
    }


}
