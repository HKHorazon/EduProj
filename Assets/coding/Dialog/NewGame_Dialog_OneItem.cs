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
    private Action<NewGame_Dialog_OneItem> callback;

    public void SetData(int id, string str, Action<NewGame_Dialog_OneItem> callback)
    {
        this.id = id;
        this.displayText.text = str;
        this.callback = callback;
    }

    public void SetStatus(bool isLock, bool isNew)
    {
        this.GetComponent<Button>().interactable = !isLock;
        this.objNewStage.SetActive(isNew);
    }

    public void OnClick()
    {
        callback?.Invoke(this);
    }
}
