using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

[ResPath("Dialog/NewGame_Dialog")]
public class NewGame_Dialog : DialogBase
{
    const int TOTAL_STAGE_COUNT = 10;

    [SerializeField] private Transform gridTransform = null;
    [SerializeField, ReadOnly] private List<NewGame_Dialog_OneItem> itemList = null;

    public void Init()
    {
        InitItems();
    }

    private void InitItems()
    {
        itemList = itemList ?? new List<NewGame_Dialog_OneItem>();
        if (itemList.Count!=0) { return; }


        var attr = typeof(NewGame_Dialog_OneItem).GetCustomAttribute<ResPathAttribute>();
        if(attr == null) { return; }
        GameObject prefab = Resources.Load<GameObject>(attr.ResourcePath);
        if(prefab == null) { return; }

        itemList = new List<NewGame_Dialog_OneItem>();
        for(int i = 0; i < TOTAL_STAGE_COUNT; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, this.gridTransform);
            if (obj == null) { return;}

            NewGame_Dialog_OneItem item = obj.GetComponent<NewGame_Dialog_OneItem>();
            item.SetData(i + 1, (i + 1).ToString());

            itemList.Add(item);
        }
    }

    private void FillItemStatus()
    {

    }

    public void OnClick_Return()
    {
        SelfHide(false);
    }
}
