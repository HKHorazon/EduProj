using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ResPath("Dialog/Victory_Dialog")]
public class Victory_Dialog : DialogBase
{
    private Action OnClick = null;

    public void Init(Action OnClick)
    {
        this.OnClick = OnClick;
        Animator animator = this.gameObject.GetComponent<Animator>();
        animator.SetTrigger("play");
    }

    public void OnClickButton()
    {
        this.OnClick?.Invoke();
        HideAnimation();
    }


}
