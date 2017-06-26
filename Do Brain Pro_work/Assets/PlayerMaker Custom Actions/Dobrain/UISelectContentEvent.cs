using System;
using UnityEngine;

using Com.Dobrain.Dobrainproject.UI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UISelectContentEvent : FsmStateAction
    {
        public FsmInt storeSelectedContentNo;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            storeSelectedContentNo = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            UIHomeScene.instance.OnSelectContent += OnSelectContent;
        }

        public override void OnExit()
        {
            UIHomeScene.instance.OnSelectContent -= OnSelectContent;
        }

        void OnSelectContent (int selectedIndex)
        {
            storeSelectedContentNo.Value = selectedIndex + 1;
            Fsm.Event(sendEvent);
        }


    }
}