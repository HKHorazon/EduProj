using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


[ResPath("Panel/BattleUI_Panel")]
public class BattleUI_Panel : PanelBase
{
    public TextMeshProUGUI TextWordCount;

    public void SetWordCount(int count)
    {
        Debug.LogError($"SET WORD Count = {count}");
        if (TextWordCount != null)
        {
            TextWordCount.SetText(string.Format(ConstString.BATTLE_WORD_COUNT, count));
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (DialogManager.Instance.GetCurrent() != null)
            {
                OnClick_Options();
            }
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            if (DialogManager.Instance.GetCurrent() != null)
            {
                YesNo_Dialog yesNo = DialogManager.Instance.Show<YesNo_Dialog>();
                yesNo.SetData(
                    "",
                    ConstString.LEAVE_GAME_PROMT,
                    delegate (bool isTrue)
                    {
                        if (isTrue)
                        {
                            Hide();
                            GameManager.Instance.RestartGame();
                        }
                        else
                        {
                            Hide();
                        }
                    }
                );
            }
        }
    }

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
