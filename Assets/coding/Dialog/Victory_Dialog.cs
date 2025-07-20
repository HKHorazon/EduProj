using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ResPath("Dialog/Victory_Dialog")]
public class Victory_Dialog : DialogBase
{
    private Action OnClick = null;


    [SerializeField] private List<Victory_Dialog_OneAnswer> answerList;

    [SerializeField] private TextMeshProUGUI victoryText = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button restartButton = null;
    [SerializeField] private Button returnToMenuButton = null;

    public void Init(bool isLastStage, Action OnClick)
    {
        this.OnClick = OnClick;
        Animator animator = this.gameObject.GetComponent<Animator>();
        animator.SetTrigger("play");

        nextButton?.gameObject.SetActive(!isLastStage);
        returnToMenuButton?.gameObject.SetActive(isLastStage);

        //Get Map Data for description..

        for (int i = 0; i < answerList.Count; i++)
        {
            Victory_Dialog_OneAnswer item = answerList[i];
            item.gameObject.SetActive(i < GameManager.Instance.gameMap.answerList.Count);
            if (i < GameManager.Instance.gameMap.answerList.Count)
            {
                OneAnswer answer = GameManager.Instance.gameMap.answerList[i];
                item.answerText.SetText(answer.answerText);
                item.answerDesc.SetText(answer.answerDesc);
            }
        }


        string text = isLastStage ? "全部關卡皆已過關" : "過關!!";
        victoryText.SetText(text);
    }

    public void OnClick_Next()
    {
        SelfHide(true);
        this.OnClick?.Invoke();
    }

    public void OnClick_Restart()
    {
        DataStore.Instance.LoadGamePlayScene(DataStore.Instance.CurrentStageId - 1);
        SelfHide(true);
    }

    public void OnClick_ReturnToMenu()
    {
        SelfHide(true);
        DataStore.Instance.LoadMenuSceneFromPlay();

    }

}
