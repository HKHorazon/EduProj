using Unity.VisualScripting;
using UnityEngine;


[ResPath("UIManager")]
public class UIManager : MonoBehaviour
{

    private static UIManager mInstance = null;
    public static UIManager Instance
    {
        get
        {
            if(mInstance == null)
            {
                ResPathAttribute attr = typeof(UIManager).GetAttribute<ResPathAttribute>();
                if(attr == null) { return null; }

                var prefab = Resources.Load(attr.ResourcePath);
                if(prefab == null) { return null; }

                var gameObj = GameObject.Instantiate(prefab);
                mInstance = gameObj.GetComponent<UIManager>();

                DontDestroyOnLoad(gameObj);
            }
            return mInstance;
        }
    }

    public void EmptyInit()
    {
    }
}
