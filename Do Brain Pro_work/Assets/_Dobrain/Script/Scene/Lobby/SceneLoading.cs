using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour 
{
    public AudioSource audioSource;
    public string nextScene;
    public Transform progressViewer;
    public float progressEndY;


    float progressStartY;
    float progressHeight;

    IEnumerator Start () 
    {
        audioSource.Play();

        progressStartY = progressViewer.position.y;
        progressHeight = progressEndY - progressViewer.position.y; 

        float progress = 0f;
        while(true)
        {
            progress += 0.02f;

            DisplayProgress(progress);

            if(1.5f <= progress)
                break;
            else
                yield return null;
        }
	}

    public void DisplayProgress(float progress)
    {
        float y = progressStartY + (progressHeight * progress);
        progressViewer.position = new Vector2(0f, y);
    }

}
