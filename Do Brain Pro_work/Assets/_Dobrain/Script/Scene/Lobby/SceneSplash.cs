using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSplash : MonoBehaviour {

    public string nextScene;

    public MediaPlayerCtrl mediaPlayer;

    void Awake()
    {
        mediaPlayer.OnEnd += HandleVideoEnd;
    }

    void HandleVideoEnd ()
    {
        SceneManager.LoadScene(nextScene);
    }

}
