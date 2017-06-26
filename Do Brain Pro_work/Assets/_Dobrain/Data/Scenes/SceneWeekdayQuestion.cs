using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Com.Dobrain.Dobrainproject.Content.Question;
using Com.Dobrain.Dobrainproject.Manager;

namespace Com.Dobrain.Dobrainproject.Content.Question.Scene
{
    public class SceneWeekdayQuestion : MonoBehaviour {

        public delegate void EventHandler();
        public event EventHandler OnComplete;

        public Transform panel;

        
        public void Init(int chapter, int index, string level, GameObject question)
        {
            question.transform.SetParent(panel, false);
            question.transform.localPosition = Vector2.zero;

            QuestionManager questionManager = GameObject.FindObjectOfType<QuestionManager>();
            questionManager.OnComplete += Question_OnComplete;

            questionManager.StartCoroutine(questionManager.Initialize(chapter,index,level));
        }

        void Question_OnComplete (QuestionManager questionManager)
        {
            if(OnComplete != null)
                OnComplete();
        }


    }
}
