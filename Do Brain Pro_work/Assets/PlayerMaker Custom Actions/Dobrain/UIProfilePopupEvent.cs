// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;

using Com.Dobrain.Dobrainproject.UI.Home;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UIProfilePopupEvent : FsmStateAction
    {
        public enum EventType
        {
            None, OnSubmit, OnClose
        }

        public UIProfilePopup profilePopup;
        public EventType eventType = EventType.None;
        public FsmString storeName;
        public FsmInt storeSelectedLevel;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            profilePopup = null;
            storeName = null;
            storeSelectedLevel = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            if(eventType == EventType.OnSubmit)
                profilePopup.OnSubmit += OnSubmit;
            else if(eventType == EventType.OnClose)
                profilePopup.OnClose += OnClose;
        }

        public override void OnExit()
        {
            if(eventType == EventType.OnSubmit)
                profilePopup.OnSubmit -= OnSubmit;
            else if(eventType == EventType.OnClose)
                profilePopup.OnClose -= OnClose;
        }

        void OnSubmit (string name, int selectedLevel)
        {
            storeName.Value = name;
            storeSelectedLevel.Value = selectedLevel;
            Fsm.Event(sendEvent);
        }

        void OnClose()
        {
            Fsm.Event(sendEvent);
        }

    }
}