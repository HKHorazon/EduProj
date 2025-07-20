using UnityEngine;

public class MenuScene_Init : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.EmptyInit();

        DialogManager.Instance.Hide<Loading_Dialog>();

        PanelManager.Instance.Show<MainBackground_Panel>();
        PanelManager.Instance.Show<MainUI_Panel>();

        Cursor.visible = true;
    }

}
