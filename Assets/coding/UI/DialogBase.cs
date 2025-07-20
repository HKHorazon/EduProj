using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent (typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
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

    [SerializeField, ReadOnly] public DialogManager belongManager = null;

    #region SHOW

    public virtual void Show(bool immediatly=false)
    {
        if (immediatly)
        {
            ShowImmediatly();
        }
        else
        {
            ShowAnimation();
        }
    }

    protected virtual void ShowImmediatly()
    {
        DialogManager.Instance.ShowDialogHandle(this);
        this.gameObject.SetActive(true);
        this.CanvasGroup.interactable = true;
        this.transform.localScale = Vector3.one;
    }

    protected virtual void ShowAnimation()
    {
        DialogManager.Instance.ShowDialogHandle(this);
        this.CanvasGroup.interactable = false;


        this.transform.localScale = Vector3.zero;
        this.gameObject.SetActive(true);

        this.transform.DOScale(1.0f, 0.2f).onComplete = delegate (){
            this.CanvasGroup.interactable = true;
        };
        this.CanvasGroup.interactable = true;
    }

    #endregion


    #region HIDE

    public void SelfHide(bool immediatly)
    {
        belongManager.Hide(this.GetType(), immediatly);
    }

    internal virtual void Hide()
    {
        HideAnimation();
    }

    internal void HideImmediatly()
    {
        this.gameObject.SetActive(false);
    }

    protected virtual void HideAnimation()
    {
        this.CanvasGroup.interactable = false;
        this.gameObject.SetActive(false);
    }

    #endregion


}
