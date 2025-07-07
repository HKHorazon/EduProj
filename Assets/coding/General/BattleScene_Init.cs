using UnityEngine;

public class BattleScene_Init : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.EmptyInit();

        PanelManager.Instance.Show<BattleUI_Panel>();


    }

}
