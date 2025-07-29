using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ResPath("Dialog/NewGame_Dialog_OneItem")]
public class NewGame_Dialog_OneItem : MonoBehaviour
{
    public int id = 0;

    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Image imageBG;
    [SerializeField] private GameObject objNewStage;
    [SerializeField] private GameObject objIsFinish;
    private Action<NewGame_Dialog_OneItem> callback;

    public void SetData(int id, string str, Action<NewGame_Dialog_OneItem> callback)
    {
        this.id = id;
        this.displayText.text = DataStore.Instance.GetChineseText(this.id);
        this.callback = callback;
    }

    public void SetStatus(bool isLock, bool isNew, bool isFinish)
    {
        this.GetComponent<CanvasGroup>().alpha = isLock ? 0.5f : 1f;
        this.GetComponent<Button>().interactable = !isLock;

        if (this.objNewStage != null) { this.objNewStage.SetActive(isNew); }
        if (this.objIsFinish != null) { this.objIsFinish?.SetActive(isFinish); }
    }

    public void OnClick()
    {
        callback?.Invoke(this);
    }
}
