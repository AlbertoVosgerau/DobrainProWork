using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoControl : MonoBehaviour {

    public MediaPlayerCtrl media;

	
    void OnApplicationPause(bool pause)
    {
        if(!pause)
            media.Play();
    }

}
