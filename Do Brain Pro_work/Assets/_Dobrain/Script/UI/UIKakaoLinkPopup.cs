using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKakaoLinkPopup : MonoBehaviour {

    public delegate void EventHandler();
    public event EventHandler OnYes;
    public event EventHandler OnNo;

    public void Create(string title, string message)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidDialog dialog = AndroidDialog.Create(title, message, "무료로 받기", "취소");
        dialog.ActionComplete += delegate(AndroidDialogResult obj)
        {
            switch(obj) {
                case AndroidDialogResult.YES:
                    if(OnYes != null)
                        OnYes();
                    break;
                case AndroidDialogResult.NO:
                    if(OnNo != null)
                        OnNo();
                    break;
            }
        };
        #elif UNITY_IOS && !UNITY_EDITOR
        IOSDialog iOSDialog = IOSDialog.Create(title, message, "무료로 받기", "취소");
        iOSDialog.OnComplete += delegate(IOSDialogResult obj)
        {
            switch(obj) {
                case IOSDialogResult.YES:
                    if(OnYes != null)
                        OnYes();
                    break;
                case IOSDialogResult.NO:
                    if(OnNo != null)
                        OnNo();
                    break;
            }
        };
        #else
        if(OnYes != null)
            OnYes();
        #endif
    }

}
