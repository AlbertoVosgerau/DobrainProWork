using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class EraseQManager : QuestionManager
    {
        public float tutorialAnimDuration = 3.0f;
        List<EraseQObject> elements;
        bool justForOnce = true;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);
            Init();
        }
        void Init()
        {
            justForOnce = true;
            elements = new List<EraseQObject>(steps[currentStep].GetComponentsInChildren<EraseQObject>());
            BeforeAnimStart();
            StartCoroutine(BeforeAnimStop(tutorialAnimDuration));
        }
        public void ConfirmAnswer()
        {
            foreach (EraseQObject element in elements)
                if (!element.isDone)
                    return;
            if (justForOnce)
            {
                justForOnce = false;
                StartCoroutine(branchAnswer());
            }
        }
        protected override IEnumerator branchAnswer()
        {
            yield return StartCoroutine(base.branchAnswer());
            if (currentStep < steps.Length)
                Init();
        }
    }
}