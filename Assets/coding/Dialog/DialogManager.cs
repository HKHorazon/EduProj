using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private static DialogManager mInstance;
    public static DialogManager Instance
    {
        get {

            if(mInstance == null)
            {
                mInstance = GameObject.FindFirstObjectByType<DialogManager>(); 
            }
            return mInstance; 
        }
    }

    public ConfirmOnly_Dialog ConfirmOnlyDialog;
    public YesNo_Dialog YesNoDialog;

    public Loading_Dialog LoadingDialog;
    public InGameMenu_Dialog inGmaeMenuDialog;



    public void ShowDialogHandle(DialogBase dialog)
    {
        dialog.gameObject.transform.SetAsLastSibling();
    }

}
