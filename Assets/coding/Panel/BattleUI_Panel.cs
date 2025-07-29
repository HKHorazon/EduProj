using UnityEngine;
using UnityEngine.SceneManagement;


[ResPath("Panel/BattleUI_Panel")]
public class BattleUI_Panel : PanelBase
{

    public void OnClick_Options()
    {
        InGameMenu_Dialog menu = DialogManager.Instance.Show<InGameMenu_Dialog>();
        menu.SetData(delegate ()
        {
            //Time.timeScale = 1f;
            if (GameManager.Instance.player != null)
            {
                GameManager.Instance.player.ControlEnable = true;
            }
        });
    }

}
