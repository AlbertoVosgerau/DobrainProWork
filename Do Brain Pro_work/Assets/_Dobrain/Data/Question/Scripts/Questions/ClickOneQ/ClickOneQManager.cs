using UnityEngine;
using System.Collections;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class ClickOneQManager : QuestionManager
    {
        public bool animRepeat = false;
        public float beforeAnimDuration = 0;
        public bool isClicked = false;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);

            if (beforeAnimDuration == 0)
                beforeAnimDuration = qSoundManager.GetBeforeSoundDuration();

            InitGame();
        }

        void InitGame()
        {
            BeforeAnimStart();
            StartCoroutine(BeforeAnimStop(beforeAnimDuration));
        }

        public void ConfirmAnswerWrapper(GameObject clickedObject)
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            if(!isClicked)
                StartCoroutine(ConfirmAnswer(clickedObject));
        }

        IEnumerator ConfirmAnswer(GameObject clickedObject)
        {
            
            isClicked = true;

            if (clickedObject.tag == "True")
                yield return StartCoroutine(branchAnswer());
            else
            {
                yield return StartCoroutine(IncorrectAnswer());
                if (animRepeat)
                    InitGame();
            }
            yield return null;
            isClicked = false;
        }

        protected override IEnumerator branchAnswer()
        {
            yield return StartCoroutine(base.branchAnswer());
            InitGame();
        }
    }
}