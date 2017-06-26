using System.Collections;
using System.Collections.Generic;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class RepeatChoiceQManager : QuestionManager
    {
        Dictionary<int, int> answerDict;
        List<RepeatChoiceQItem> items;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);
            Init();
        }
        void Init()
        {
            answerDict = new Dictionary<int, int>();
            items = new List<RepeatChoiceQItem>(steps[currentStep].GetComponentsInChildren<RepeatChoiceQItem>());
            foreach (RepeatChoiceQItem item in items)
                answerDict[item.GetInstanceID()] = item.repeatNum;
        }
        IEnumerator ConfirmAnswer()
        {
            foreach (int answerValue in answerDict.Values)
                if (answerValue > 0)
                    yield break;
            yield return StartCoroutine(base.branchAnswer());
            Init();
        }

        public IEnumerator ChoiceItem(RepeatChoiceQItem item)
        {
            if (answerDict[item.GetInstanceID()] > 0)
            {
                answerDict[item.GetInstanceID()] -= 1;
                StartCoroutine(ConfirmAnswer());
            }
            else
            {
                yield return StartCoroutine(IncorrectAnswer());
                Init();
            }
        }
    }
}
