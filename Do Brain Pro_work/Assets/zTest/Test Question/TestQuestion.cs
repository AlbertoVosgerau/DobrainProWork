using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Com.Dobrain.Dobrainproject.Content.Question.Scene;

public class TestQuestion : MonoBehaviour {

    public enum Type
    {
        Weekday, Weekend
    }

    public enum Level
    {
        A, B, C
    }

    public Type type = Type.Weekday;
    [Range(1, 40)]
    public int chapter = 1;
    [Range(1, 8)]
    public int index = 1;
    public Level level;

    [HideInInspector]
    public GameObject questionPrefab;

    IEnumerator Start () 
    {
        AsyncOperation ao = null;
        if(type == Type.Weekday)
            ao = SceneManager.LoadSceneAsync("WeekdayQuestion", LoadSceneMode.Additive);
        else if(type == Type.Weekend)
            ao = SceneManager.LoadSceneAsync("WeekendQuestion", LoadSceneMode.Additive);

        while(!ao.isDone)
            yield return null;

        if(type == Type.Weekday)
        {
            SceneWeekdayQuestion scene = GameObject.FindObjectOfType<SceneWeekdayQuestion>();

            GameObject question = Instantiate(questionPrefab);

            scene.Init(chapter, index, level.ToString(), question);
        }
        else if(type == Type.Weekend)
        {
            SceneWeekendQuestion scene = GameObject.FindObjectOfType<SceneWeekendQuestion>();

            GameObject question = Instantiate(questionPrefab);

            scene.Init(level.ToString(), question);
        }
	}

}
