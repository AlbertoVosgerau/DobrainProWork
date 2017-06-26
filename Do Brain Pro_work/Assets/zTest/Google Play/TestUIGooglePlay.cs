using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIGooglePlay : MonoBehaviour {

    public Text logView;

    public void SetLog(string log)
    {
        logView.text += log + "\n\n";
    }
	
}
