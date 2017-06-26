using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlugin : MonoBehaviour {
    
    public Text resultViewer;

	void Start () 
    {
//        resultViewer.text = AndroidPlugin.instance.GetString("hello world");
//        resultViewer.text = AndroidPlugin.instance.GetSum(1, 1).ToString();
	}

    public void Display()
    {
        resultViewer.text = AndroidPlugin.instance.GetSum(1, 1).ToString();
    }

}
