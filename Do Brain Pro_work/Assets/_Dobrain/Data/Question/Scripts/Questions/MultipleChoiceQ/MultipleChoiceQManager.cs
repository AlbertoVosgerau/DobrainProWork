using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class MultipleChoiceQManager : QuestionManager
    {
        public float beforeAnimDuration = 2.0f;
        public bool isRepeat = false;
        Dictionary<int, bool> checkAnswerDict;
        List<MultipleChoiceQObject> trueObjects;
        List<MultipleChoiceQObject> itemList;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);
            InitGame();
        }
        void InitGame()
        {
            checkAnswerDict = new Dictionary<int, bool>();
            itemList = new List<MultipleChoiceQObject>(steps[currentStep].GetComponentsInChildren<MultipleChoiceQObject>());
            trueObjects = new List<MultipleChoiceQObject>();
            foreach (MultipleChoiceQObject item in itemList)
                if (item.gameObject.tag == "True")
                    trueObjects.Add(item);
            InitAllItems();
            AnimStart();
        }
        void AnimStart()
        {
            BeforeAnimStart();
            StartCoroutine(BeforeAnimStop(beforeAnimDuration));
        }
        void InitAllItems()
        {
            foreach (MultipleChoiceQObject item in itemList)
            {
                item.Init();
                if (item.gameObject.tag == "True")
                    checkAnswerDict[item.gameObject.GetInstanceID()] = false;
            }
        }
        IEnumerator _StartAnim()
        {
            yield return new WaitForSeconds(incorrectAnimDuration);
            AnimStart();
        }
        public IEnumerator ConfirmAnswer(GameObject clickedObject)
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            if (clickedObject.tag == "True")
            {
                checkAnswerDict[clickedObject.GetInstanceID()] = !checkAnswerDict[clickedObject.GetInstanceID()];
                if (!checkAnswerDict.ContainsValue(false))
                    StartCoroutine(branchAnswer());
            }
            else
            {
                StartCoroutine(IncorrectAnswer());

                if (isRepeat)
                {
                    StartCoroutine(_StartAnim());
                }

                foreach (MultipleChoiceQObject trueObject in trueObjects)
                    trueObject.ReShowImage();
                InitAllItems();


            }
            yield return null;
        }

        protected override IEnumerator branchAnswer()
        {
            yield return StartCoroutine(base.branchAnswer());
            if (currentStep < steps.Length)
                InitGame();
        }
    }
}