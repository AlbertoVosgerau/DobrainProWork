using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJust : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        DateTime date = new DateTime();
        DateTime nullDate = new DateTime();
        Debug.Log(date == nullDate);
	}

}
