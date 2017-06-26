using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestQuestion))]
public class TestQuestionEditor : Editor {

    static TestQuestion instance;

    void OnEnable()
    {
        if(instance == null)
            instance = target as TestQuestion;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        instance.questionPrefab = null;
        string type = instance.type.ToString();
        string chapter = instance.chapter.ToString();
        if(instance.type == TestQuestion.Type.Weekday)
        {
            string index = (instance.index - 1).ToString();
            instance.questionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Dobrain/Data/Question/" + type + "/Chapters/ch" + chapter + "/Prefab/ch" + chapter + "_q" + index + ".prefab");
        }
        else if(instance.type == TestQuestion.Type.Weekend)
        {
            instance.questionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Dobrain/Data/Question/" + type + "/ch" + chapter + ".prefab");
        }

        if(instance.questionPrefab == null)
        {
            GUILayout.Space(10);
            GUI.color = Color.red;
            GUILayout.BeginHorizontal();
            GUILayout.Label("(!) 문제가 없습니다");
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
        }
    }

}
