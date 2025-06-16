using UnityEngine;


[RequireComponent (typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class DialogBase : MonoBehaviour
{
    private Canvas mCanvas = null;
    public Canvas Canvas
    {
        get
        {
            if(mCanvas == null)
            {
                mCanvas = this.GetComponent<Canvas>();
            }
            return mCanvas;
        }
    }

    private CanvasGroup mCanvasGroup = null;
    public CanvasGroup CanvasGroup
    {
        get
        {
            if (mCanvasGroup == null)
            {
                mCanvasGroup = this.GetComponent<CanvasGroup>();
            }
            return mCanvasGroup;
        }
    }

    public virtual void Show()
    {
        ShowAnimation();
    }

    public virtual void Hide()
    {
        HideAnimation();
    }

    protected virtual void ShowAnimation()
    {
        DialogManager.Instance.ShowDialogHandle(this);
        this.CanvasGroup.interactable = false;
        this.gameObject.SetActive(true);
        this.CanvasGroup.interactable = true;
    }

    protected virtual void HideAnimation()
    {
        this.CanvasGroup.interactable = false;
        this.gameObject.SetActive(false);
    }
}
