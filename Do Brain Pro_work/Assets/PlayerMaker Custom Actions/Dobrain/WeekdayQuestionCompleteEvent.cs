// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using Com.Dobrain.Dobrainproject.Content.Question.Scene;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class WeekdayQuestionCompleteEvent : FsmStateAction
    {
        [ObjectTypeAttribute(typeof(SceneWeekdayQuestion))]
        public FsmObject questionScene;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            questionScene = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            ((SceneWeekdayQuestion)questionScene.Value).OnComplete += OnComplete;
        }

        public override void OnExit()
        {
            ((SceneWeekdayQuestion)questionScene.Value).OnComplete -= OnComplete;
        }

        void OnComplete()
        {
            Fsm.Event(sendEvent);
        }

    }
}