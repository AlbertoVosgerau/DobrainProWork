using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Com.Dobrain.Dobrainproject.Content.Question;
using Com.Dobrain.Dobrainproject.Manager;
using Dobrain.contents;

namespace Com.Dobrain.Dobrainproject.Content.Question.Scene
{
    public class SceneWeekendQuestion : MonoBehaviour {

        public delegate void EventHandler(WeekendManager weekendManager);
        public event EventHandler OnComplete;

        public Transform panel;

        
        public void Init(string level, GameObject question)
        {
            question.transform.SetParent(panel, false);
            question.transform.localPosition = Vector2.zero;
            question.transform.SetAsFirstSibling();

            WeekendManager weekendManager = GameObject.FindObjectOfType<WeekendManager>();
            weekendManager.OnComplete += Question_OnComplete;

            weekendManager.StartCoroutine(weekendManager.Initialize(level));
        }

        void Question_OnComplete (WeekendManager weekendManager)
        {
            if(OnComplete != null)
                OnComplete(weekendManager);
        }


    }
}
