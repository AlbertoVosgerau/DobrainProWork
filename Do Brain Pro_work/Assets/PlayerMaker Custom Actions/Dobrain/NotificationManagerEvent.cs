using System;
using UnityEngine;

using Com.Dobrain.Dobrainproject.Manager;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class NotificationManagerEvent : FsmStateAction
    {
        public enum EventType
        {
            None, ShowToastComplete
        }

        public NotificationManager notificationManager;
        public EventType eventType = EventType.None;
        public FsmEvent sendEvent;


        public override void Reset()
        {
            sendEvent = null;
        }

        public override void OnEnter()
        {
            if(eventType == EventType.ShowToastComplete)
                notificationManager.OnShowToastComplete += OnComplete;
        }

        public override void OnExit()
        {
            if(eventType == EventType.ShowToastComplete)
                notificationManager.OnShowToastComplete -= OnComplete;
        }

        void OnComplete ()
        {
            Fsm.Event(sendEvent);
        }


    }
}