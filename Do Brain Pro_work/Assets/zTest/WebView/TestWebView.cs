using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWebView : MonoBehaviour {

    WebViewObject webViewObject;

    void Start()
    {
        StartWebView();
    }

	void Update () 
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                Destroy(webViewObject);
        }
    }

    public void StartWebView()
    {
        webViewObject = gameObject.AddComponent<WebViewObject>();
        webViewObject.Init((msg) => {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
        });

//        webViewObject.LoadURL("http://www.naver.com");
//        webViewObject.LoadURL("https://dobrain-pro.firebaseapp.com/kakaolink.html");
        webViewObject.LoadURL("https://developers.kakao.com/docs/js/demos/link-v2-default-feed");
        webViewObject.SetVisibility(true);
        webViewObject.SetMargins(50, 50, 50, 50);
    }


}
