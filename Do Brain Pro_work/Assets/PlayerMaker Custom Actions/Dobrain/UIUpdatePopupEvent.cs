using System;
using UnityEngine;

using Com.Dobrain.Dobrainproject.Manager;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UIUpdatePopupEvent : FsmStateAction
    {
        public enum EventType
        {
            None, OnClose
        }

        public UIUpdatePopup updatePopup;

        public EventType eventType = EventType.None;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            sendEvent = null;
        }

        public override void OnEnter()
        {
            if(eventType == EventType.OnClose)
                updatePopup.OnClose += OnClose;
        }

        public override void OnExit()
        {
            if(eventType == EventType.OnClose)
                updatePopup.OnClose -= OnClose;
        }

        void OnClose ()
        {
            Fsm.Event(sendEvent);
        }


    }
}