using UnityEngine;
using System.Collections;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class WhackAMoleQManager : QuestionManager
    {
        public bool isMoving = true;
        float NarrationDuration;
        WhackAMoleQObject[] whackedObjects;

        float time;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);

            Init();
            NarrationDuration = qSoundManager.GetBeforeSoundDuration();
            BeforeAnimStart();
            StartCoroutine(BeforeAnimStop(NarrationDuration));
        }

        void Init()
        {
            if (currentStep <= lastStep)
                whackedObjects = steps[currentStep].GetComponentsInChildren<WhackAMoleQObject>();
            InitItems();

            time = 10f;
            StartCoroutine("ScoreTime");
        }

        void InitItems()
        {
            foreach (WhackAMoleQObject item in whackedObjects)
                item.Init();
        }

        public void ConfirmAnswer(GameObject whackedObject)
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            if (whackedObject.tag == "True")
            {
                StartCoroutine(branchAnswer());
                Init();
            }
            else
            {
                if (!isMoving)
                    FadeOutAndIn();
                InitItems();
                StartCoroutine(IncorrectAnswer());
            }
        }

        public void FadeOutAndIn()
        {
            foreach (WhackAMoleQObject whackedObject in whackedObjects)
                whackedObject.FadeOutAndIn();
        }

        protected override IEnumerator branchAnswer()
        {

            StopCoroutine("ScoreTime");

            if (steps.Length != 0)
                steps[currentStep++].SetActive(false);
            yield return StartCoroutine(AfterAnimStart(afterAnimDuration));
            if (currentStep > lastStep)
            {
                ScoreCalculate(time);
                questioncount++;

                yield return StartCoroutine(CorrectAnimStart());

                missionManager.Record(System.DateTime.Today.ToString("yyyy-MM-dd"), thischapter, thisindex, AbilityToText(), recordscore / questioncount);

                Complete();
            }
            else
            {
                scoreChance = 0;
                ScoreCalculate(scoreChance);
                questioncount++;

                yield return StartCoroutine(SubCorrectAnimStart());
                // Next step activate and initiate
                steps[currentStep].SetActive(true);
                qSoundManager.PlayQuestionSound();
            }
        }

        IEnumerator ScoreTime()
        {
            while(time > 0)
            {
                yield return new WaitForSeconds(0.1f);
                time -= 0.1f;
            }
        }

        protected void ScoreCalculate(float time)
        {
            int _recordscore;

            if(time > 8.5f)
            {
                _recordscore = 10;
            }
            else if(time > 6)
            {
                _recordscore = 5;
            }
            else
            {
                _recordscore = 3;
            }
           
            recordscore += _recordscore;

        }
    }
}
