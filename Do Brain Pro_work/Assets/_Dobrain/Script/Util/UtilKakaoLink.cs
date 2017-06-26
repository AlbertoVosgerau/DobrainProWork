using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilKakaoLink : MonoBehaviour {

    string url = "https://dobrain-pro.firebaseapp.com/kakaolink.html";

    public void SendLink()
    {
        Application.OpenURL(url);

//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"); 
//        AndroidJavaObject pm = jo.Call<AndroidJavaObject>("getPackageManager"); 
//        AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", "com.android.browser");
//        jo.Call("startActivity", intent);
    }

}
