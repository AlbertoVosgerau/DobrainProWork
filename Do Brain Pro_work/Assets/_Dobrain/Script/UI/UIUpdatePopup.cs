using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdatePopup : MonoBehaviour {

    public delegate void EventHandler();
    public event EventHandler OnClose;

    public void Create(string title, string message, string url)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidDialog dialog = AndroidDialog.Create(title, message, "업데이트", "취소");
        dialog.ActionComplete += delegate(AndroidDialogResult obj)
        {
                switch(obj) {
                    case AndroidDialogResult.YES:
                        AndroidNativeUtility.OpenAppRatingPage(url);
                        break;
                }

                if(OnClose != null)
                    OnClose();
        };
        #elif UNITY_IOS && !UNITY_EDITOR
        IOSDialog iOSDialog = IOSDialog.Create(title, message, "업데이트", "취소");
        iOSDialog.OnComplete += delegate(IOSDialogResult obj)
        {
            switch(obj) {
                case IOSDialogResult.YES:
                IOSNativeUtility.RedirectToAppStoreRatingPage("1232104060");
                break;
            }

            if(OnClose != null)
                OnClose();
        };
        #endif
    }

}
