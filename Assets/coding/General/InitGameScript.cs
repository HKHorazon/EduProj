using UnityEngine;

public class InitGameScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.EmptyInit();

        PanelManager.Instance.Show<MainBackground_Panel>();
        PanelManager.Instance.Show<MainUI_Panel>();


    }

}
