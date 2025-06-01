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

    protected void ShowAnimation()
    {
        this.CanvasGroup.interactable = false;
        this.gameObject.SetActive(true);
        this.CanvasGroup.interactable = true;
    }

    protected void HideAnimation()
    {
        this.CanvasGroup.interactable = false;
        this.gameObject.SetActive(false);
    }
}
