using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class PanelBase : MonoBehaviour
{
    [SerializeField, ReadOnly] public PanelManager belongManager = null;
    [field:SerializeField, ReadOnly] public bool isShow { get; private set; } = false;

    public virtual void Show()
    {
        isShow = true;
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        isShow = false;
        this.gameObject.SetActive(false);
    }
}
