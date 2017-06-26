using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class DistributionQManager : QuestionManager
    {
        public float NarrationDuration = 2.0f;
        List<DistributionQPoint> distributionPoints;
        List<DistributionQItem> dropItems;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);

            Init();
            BeforeAnimStart();
            StartCoroutine(BeforeAnimStop(NarrationDuration));
        }

        void Init()
        {
            dropItems = new List<DistributionQItem>(steps[currentStep].GetComponentsInChildren<DistributionQItem>());
            distributionPoints = new List<DistributionQPoint>(steps[currentStep].GetComponentsInChildren<DistributionQPoint>());
            foreach (DistributionQItem item in dropItems)
                item.Init();
            foreach (DistributionQPoint point in distributionPoints)
                point.Init();
        }
        public void ConfirmAnswer()
        {
            foreach (DistributionQPoint distributionPoint in distributionPoints)
                if (!distributionPoint.isAnswer)
                    return;
            StartCoroutine(branchAnswer());
        }
        public IEnumerator IncorrectDrop()
        {
            Init();
            yield return StartCoroutine(IncorrectAnswer());
        }
        protected override IEnumerator branchAnswer()
        {
            yield return StartCoroutine(base.branchAnswer());
            Init();
        }
    }
}