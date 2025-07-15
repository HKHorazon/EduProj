using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonScript : MonoBehaviour
{
    private Button button;
    [SerializeField] private string ClickSound = "Sound/UI_Click";

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on " + gameObject.name);
        }
        button.onClick.AddListener(PlayClickSound);
    }


    private void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX(ClickSound);
    }
}
