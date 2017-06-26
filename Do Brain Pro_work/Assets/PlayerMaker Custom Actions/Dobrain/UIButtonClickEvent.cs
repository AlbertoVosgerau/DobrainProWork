// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UIButtonClickEvent : FsmStateAction
    {
        public Button button;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            sendEvent = null;
        }

        public override void OnEnter()
        {
            button.onClick.AddListener(OnClick);
        }

        public override void OnExit()
        {
            button.onClick.RemoveListener(OnClick);
        }

        void OnClick()
        {
            Fsm.Event(sendEvent);
        }

    }
}