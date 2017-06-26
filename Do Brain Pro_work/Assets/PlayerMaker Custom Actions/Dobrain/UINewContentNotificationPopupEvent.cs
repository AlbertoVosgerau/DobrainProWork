using UnityEngine;
using UnityEngine.UI;

using Com.Dobrain.Dobrainproject.UI.Home;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UINewContentNotificationPopupEvent : FsmStateAction
    {
        public UINewContentNotificationPopup newContentNotificationPopup;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            sendEvent = null;
        }

        public override void OnEnter()
        {
            newContentNotificationPopup.OnClose += OnEvent;
        }

        public override void OnExit()
        {
            newContentNotificationPopup.OnClose -= OnEvent;
        }

        void OnEvent()
        {
            Fsm.Event(sendEvent);
        }

    }
}