using System;
using UnityEngine;

using Com.Dobrain.Dobrainproject.Manager;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class UIKakaoLinkPopupEvent : FsmStateAction
    {
        public UIKakaoLinkPopup kakaoLinkPopup;

        public FsmEvent sendYesEvent;
        public FsmEvent sendNoEvent;

        public override void Reset()
        {
            sendYesEvent = null;
            sendNoEvent = null;
        }

        public override void OnEnter()
        {
            kakaoLinkPopup.OnYes += OnYes;
            kakaoLinkPopup.OnNo += OnNo;
        }

        public override void OnExit()
        {
            kakaoLinkPopup.OnYes -= OnYes;
            kakaoLinkPopup.OnNo -= OnNo;
        }

        void OnYes ()
        {
            Fsm.Event(sendYesEvent);
        }

        void OnNo ()
        {
            Fsm.Event(sendNoEvent);
        }


    }
}