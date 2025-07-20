using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogManager : MonoBehaviour
{
    private static DialogManager mInstance;
    public static DialogManager Instance
    {
        get {

            if(mInstance == null)
            {
                mInstance = GameObject.FindFirstObjectByType<DialogManager>(); 
            }
            return mInstance; 
        }
    }

    [SerializeField, ReadOnly] private DialogBase currentDialog = null;

    private Dictionary<Type, DialogBase> dialogTable = new Dictionary<Type, DialogBase>();


    public T Get<T>() where T : DialogBase
    {
        GameObject finalObject = null;
        DialogBase finalScript = null;

        //Loading from table or from Instantiate
        if (dialogTable.ContainsKey(typeof(T)))
        {
            finalObject = dialogTable[typeof(T)].gameObject;
            finalScript = dialogTable[typeof(T)];
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
                    finalScript = finalObject.GetComponent<DialogBase>();
                }

                if (finalObject == null || finalScript == null)
                {
                    return default(T);
                }
                dialogTable[typeof(T)] = finalScript;
                finalScript.belongManager = this;
            }
        }

        return (T)finalScript;
    }

    public T Show<T>(bool immediatly = false) where T : DialogBase
    {
        T finalScript = Get<T>();

        if(finalScript == null)
        {
            return null;
        }

        HideCurrent(true);

        this.currentDialog = finalScript;
        finalScript.Show();

        return finalScript;
    }

    public void Hide(Type type, bool immediatly = false)
    {
        if (currentDialog != null &&
            currentDialog.GetType() == type)
        {
            HideCurrent(immediatly);
        }
    }

    public void Hide<T>(bool immediatly=false) where T : DialogBase
    {
        Hide(typeof(T), immediatly);
    }

    public void HideCurrent(bool immediatly=false)
    {
        if(currentDialog != null)
        {
            if (immediatly)
            {
                currentDialog.HideImmediatly();
            }
            else
            {
                currentDialog.Hide();
            }
            currentDialog = null;
        }
    }

    public void ShowDialogHandle(DialogBase dialog)
    {
        dialog.gameObject.transform.SetAsLastSibling();
    }

}
