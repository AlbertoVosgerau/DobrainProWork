using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AndroidPlugin : MonoBehaviour {
    
    public static AndroidPlugin instance;

    AndroidJavaClass myCls;
    AndroidJavaObject myObj;

    int result = -1;

    void Awake()
    {
        instance = this;

//        myCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        myObj = myCls.GetStatic<AndroidJavaObject>("currentActivity");
//        #if UNITY_ANDROID && !UNITY_EDITOR
//
//        #endif
    }

    public string GetString(string str)
    {
        if(myObj != null)
            return "has " + myObj.Call<string>("GetString", str);
        else
            return "null " + str;
    }

    public int GetSum(int a, int b)
    {
        myObj = new AndroidJavaObject("com.dobrain.dobrainpro.TestAClass");
        result = 0;

        if(myObj != null)
            return myObj.Call<int>("GetSum", a, b);
        else 
            return result;
    }

}
