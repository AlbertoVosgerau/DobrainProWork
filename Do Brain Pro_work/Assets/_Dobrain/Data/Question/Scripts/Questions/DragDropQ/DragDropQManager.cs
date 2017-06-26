using System.Collections;
using System.Collections.Generic;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class DragDropQManager : QuestionManager
    {
        public bool isRepeat = false;
        public float NarrationDuration = 2.0f;
        List<DragDropQPoint> dropPoints;
        List<DragDropQItem> dropItems;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);

            InitLists();
            InitGame();
            BeforeAnimStart();
            StartCoroutine(BeforeAnimStop(NarrationDuration));

        }

        void InitLists()
        {
            BeforeAnimStart();
            StartCoroutine(BeforeAnimStop(NarrationDuration));

            dropItems = new List<DragDropQItem>(steps[currentStep].GetComponentsInChildren<DragDropQItem>());
            dropPoints = new List<DragDropQPoint>(steps[currentStep].GetComponentsInChildren<DragDropQPoint>());
        }

        void InitGame()
        {
            foreach (DragDropQItem item in dropItems)
                item.Init();
            foreach (DragDropQPoint point in dropPoints)
                point.Init();

        }
        public void ConfirmAnswer()
        {
            foreach (DragDropQPoint dropPoint in dropPoints)
                if (!dropPoint.isAnswer)
                    return;
            StartCoroutine(branchAnswer());
        }
        public IEnumerator IncorrectDrop()
        {
            InitGame();
            yield return StartCoroutine(IncorrectAnswer());

            if (isRepeat)
            {
                BeforeAnimStart();
                StartCoroutine(BeforeAnimStop(NarrationDuration));
            }
        }

        protected override IEnumerator branchAnswer()
        {
            yield return StartCoroutine(base.branchAnswer());
            if (currentStep < steps.Length)
            {
                InitLists();
                InitGame();
            }
        }
    }
}
