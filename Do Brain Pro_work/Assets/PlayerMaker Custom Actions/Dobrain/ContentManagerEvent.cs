using System;
using UnityEngine;

using Com.Dobrain.Dobrainproject.Manager;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class ContentManagerEvent : FsmStateAction
    {
        public enum EventType
        {
            None, LoadQuestionComplete, LoadQuestionFail, LoadAnimationComplete, LoadAnimationFail
        }

        public EventType eventType = EventType.None;
        public FsmString storeErrorMessage;
        public FsmEvent sendEvent;


        public override void Reset()
        {
            storeErrorMessage = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            if(eventType == EventType.LoadQuestionComplete)
                ContentManager.instance.OnLoadQuestionComplete += OnComplete;
            else if(eventType == EventType.LoadQuestionFail)
                ContentManager.instance.OnLoadQuestionFail += OnFail;
            else if(eventType == EventType.LoadAnimationComplete)
                ContentManager.instance.OnLoadAnimationComplete += OnComplete;
            else if(eventType == EventType.LoadAnimationFail)
                ContentManager.instance.OnLoadAnimationFail += OnFail;
        }

        public override void OnExit()
        {
            if(eventType == EventType.LoadQuestionComplete)
                ContentManager.instance.OnLoadQuestionComplete -= OnComplete;
            else if(eventType == EventType.LoadQuestionFail)
                ContentManager.instance.OnLoadQuestionFail -= OnFail;
            else if(eventType == EventType.LoadAnimationComplete)
                ContentManager.instance.OnLoadAnimationComplete -= OnComplete;
            else if(eventType == EventType.LoadAnimationFail)
                ContentManager.instance.OnLoadAnimationFail -= OnFail;
        }

        void OnComplete ()
        {
            Fsm.Event(sendEvent);
        }

        void OnFail(string error)
        {
            storeErrorMessage.Value = error;
            Fsm.Event(sendEvent);
        }


    }
}