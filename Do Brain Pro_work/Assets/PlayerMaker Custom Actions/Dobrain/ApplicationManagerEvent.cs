using System;
using UnityEngine;

using Com.Dobrain.Dobrainproject.Manager;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class ApplicationManagerEvent : FsmStateAction
    {
        public enum EventType
        {
            None, OnCheckLatestVersionComplete, OnCheckLatestVersionFail
        }

        [ObjectTypeAttribute(typeof(ApplicationManager))]
        public FsmObject applicationManager;

        public EventType eventType = EventType.None;

        public FsmBool storeResult;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            applicationManager = null;
            storeResult = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            if(eventType == EventType.OnCheckLatestVersionComplete)
                ((ApplicationManager)applicationManager.Value).OnCheckLatestVersionComplete += OnCheckLatestVersionComplete;
            else if(eventType == EventType.OnCheckLatestVersionFail)
                ((ApplicationManager)applicationManager.Value).OnCheckLatestVersionFail += OnCheckLatestVersionFail;
        }

        public override void OnExit()
        {
            if(eventType == EventType.OnCheckLatestVersionComplete)
                ((ApplicationManager)applicationManager.Value).OnCheckLatestVersionComplete -= OnCheckLatestVersionComplete;
            else if(eventType == EventType.OnCheckLatestVersionFail)
                ((ApplicationManager)applicationManager.Value).OnCheckLatestVersionFail -= OnCheckLatestVersionFail;
        }

        void OnCheckLatestVersionFail()
        {
            Fsm.Event(sendEvent);
        }

        void OnCheckLatestVersionComplete (bool result)
        {
            storeResult.Value = result;
            Fsm.Event(sendEvent);
        }


    }
}