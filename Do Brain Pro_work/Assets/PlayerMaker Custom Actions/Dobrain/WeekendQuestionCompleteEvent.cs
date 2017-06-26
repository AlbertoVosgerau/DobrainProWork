// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using Dobrain.contents;
using Com.Dobrain.Dobrainproject.Content.Question.Scene;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dobrain")]
    public class WeekendQuestionCompleteEvent : FsmStateAction
    {
        [ObjectTypeAttribute(typeof(SceneWeekendQuestion))]
        public FsmObject questionScene;

        [ObjectTypeAttribute(typeof(WeekendManager))]
        public FsmObject storeWeekendManager;

        public FsmEvent sendEvent;

        public override void Reset()
        {
            questionScene = null;
            storeWeekendManager = null;
            sendEvent = null;
        }

        public override void OnEnter()
        {
            ((SceneWeekendQuestion)questionScene.Value).OnComplete += OnComplete;
        }

        public override void OnExit()
        {
            ((SceneWeekendQuestion)questionScene.Value).OnComplete -= OnComplete;
        }

        void OnComplete(WeekendManager weekendManager)
        {
            storeWeekendManager.Value = weekendManager;
            Fsm.Event(sendEvent);
        }

    }
}