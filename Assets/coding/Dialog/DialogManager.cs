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

}
