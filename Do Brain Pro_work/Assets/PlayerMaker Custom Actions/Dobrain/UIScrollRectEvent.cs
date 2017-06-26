// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UIScrollRectEvent : FsmStateAction
    {
        public enum Type
        {
            BeginDrag, EndDrag
        }

        public Type eventType;

        public UIScrollRect scrollRect;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            scrollRect = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            switch(eventType)
            {
                case Type.BeginDrag:
                    scrollRect.OnBeginDragEvent += OnEvent;
                    break;
                case Type.EndDrag:
                    scrollRect.OnEndDragEvent += OnEvent;
                    break;
            }
        }

        public override void OnExit()
        {
            switch(eventType)
            {
                case Type.BeginDrag:
                    scrollRect.OnBeginDragEvent -= OnEvent;
                    break;
                case Type.EndDrag:
                    scrollRect.OnEndDragEvent -= OnEvent;
                    break;
            }
        }

        void OnEvent(PointerEventData data)
        {
            Fsm.Event(sendEvent);
        }

    }
}