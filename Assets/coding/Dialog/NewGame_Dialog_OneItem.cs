using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ResPath("Dialog/NewGame_Dialog_OneItem")]
public class NewGame_Dialog_OneItem : MonoBehaviour
{
    public int id = 0;

    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Image imageBG;

    public void SetData(int id, string str)
    {
        this.id = id;
        this.displayText.text = str;
    }
}
