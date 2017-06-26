using System;
using UnityEngine;

using Com.Dobrain.Dobrainproject.UI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UIParentsButtonClickEvent : FsmStateAction
    {
        public FsmEvent sendEvent;

        public override void Reset()
        {
            sendEvent = null;
        }

        public override void OnEnter()
        {
            UIHomeScene.instance.OnParentsButtonClick += OnParentsButtonClick;
        }

        public override void OnExit()
        {
            UIHomeScene.instance.OnParentsButtonClick -= OnParentsButtonClick;
        }

        void OnParentsButtonClick ()
        {
            Fsm.Event(sendEvent);
        }


    }
}