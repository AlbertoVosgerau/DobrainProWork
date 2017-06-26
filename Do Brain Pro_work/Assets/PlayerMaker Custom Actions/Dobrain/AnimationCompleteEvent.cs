// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class AnimationCompleteEvent : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [RequiredField]
        public FsmObject animation;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            animation = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            ((MediaPlayerCtrl)animation.Value).OnEnd += OnComplete;
        }

        public override void OnExit()
        {
            ((MediaPlayerCtrl)animation.Value).OnEnd -= OnComplete;
        }

        void OnComplete()
        {
            Fsm.Event(sendEvent);
        }

    }
}