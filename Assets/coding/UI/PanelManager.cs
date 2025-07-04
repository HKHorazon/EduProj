using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PanelManager : MonoBehaviour
{
    private static PanelManager mInstance;
    public static PanelManager Instance
    {
        get
        {

            if (mInstance == null)
            {
                mInstance = GameObject.FindFirstObjectByType<PanelManager>();
            }
            return mInstance;
        }
    }

    private Dictionary<Type, PanelBase> panelTable = new Dictionary<Type, PanelBase>();


    public T Get<T>() where T : PanelBase
    {
        GameObject finalObject = null;
        PanelBase finalScript = null;

        //Loading from table or from Instantiate
        if (panelTable.ContainsKey(typeof(T)))
        {
            finalObject = panelTable[typeof(T)].gameObject;
            finalScript = panelTable[typeof(T)];
        }
        else
        {
            ResPathAttribute pathAttr = typeof(T).GetCustomAttribute<ResPathAttribute>();
            if (pathAttr != null)
            {
                GameObject prefab = (GameObject)Resources.Load(pathAttr.ResourcePath);

                if (prefab != null)
                {
                    finalObject = GameObject.Instantiate(prefab, this.transform);
                    finalObject.gameObject.SetActive(false);
                    finalScript = finalObject.GetComponent<PanelBase>();
                }

                if (finalObject == null || finalScript == null)
                {
                    return default(T);
                }
                panelTable[typeof(T)] = finalScript;
                finalScript.belongManager = this;
            }
        }


        return (T)finalScript;
    }

    public T Show<T>() where T : PanelBase
    {
        T finalScript = Get<T>();

        if (finalScript == null)
        {
            return null;
        }

        finalScript.Show();

        return finalScript;
    }

    public void HideAllPanels()
    {
        foreach(var kvp in panelTable)
        {
            if (kvp.Value != null && kvp.Value.isShow)
            {
                kvp.Value.Hide();
            }
        }
    }

    public void Hide(Type type, bool immediatly = false)
    {
        //TODO
        if (this.panelTable.ContainsKey(type))
        {
            this.panelTable[type].Hide();
        }
    }

    public void Hide<T>(bool immediatly = false) where T : DialogBase
    {
        Hide(typeof(T), immediatly);
    }

    public void ShowDialogHandle(DialogBase dialog)
    {
        dialog.gameObject.transform.SetAsLastSibling();
    }

}
