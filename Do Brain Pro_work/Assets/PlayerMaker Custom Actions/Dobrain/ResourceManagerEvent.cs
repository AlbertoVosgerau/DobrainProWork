// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class ResourceManagerEvent : FsmStateAction
    {
        public enum EventType
        {
            None, LoadGiftsComplete, LoadGiftsFail, LoadCachedGiftsComplete, LoadCachedGiftsFail
        }

        public EventType eventType;
        public FsmString storeError;
        public FsmEvent sendEvent;

        public override void Reset()
        {
            eventType = EventType.None;
            storeError = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            if(eventType == EventType.LoadGiftsComplete)
                ResourceManager.instance.OnLoadGiftsComplete += OnComplete;
            else if(eventType == EventType.LoadGiftsFail)
                ResourceManager.instance.OnLoadGiftsFail += OnFail;
            else if(eventType == EventType.LoadCachedGiftsComplete)
                ResourceManager.instance.OnLoadCachedGiftsComplete += OnComplete;
            else if(eventType == EventType.LoadCachedGiftsFail)
                ResourceManager.instance.OnLoadCachedGiftsFail += OnComplete;
        }

        public override void OnExit()
        {
            if(eventType == EventType.LoadGiftsComplete)
                ResourceManager.instance.OnLoadGiftsComplete -= OnComplete;
            else if(eventType == EventType.LoadGiftsFail)
                ResourceManager.instance.OnLoadGiftsFail -= OnFail;
            else if(eventType == EventType.LoadCachedGiftsComplete)
                ResourceManager.instance.OnLoadCachedGiftsComplete -= OnComplete;
            else if(eventType == EventType.LoadCachedGiftsFail)
                ResourceManager.instance.OnLoadCachedGiftsFail -= OnComplete;
        }

        void OnComplete()
        {
            Fsm.Event(sendEvent);
        }

        void OnFail(string error)
        {
            storeError.Value = error;
            Fsm.Event(sendEvent);
        }

    }
}