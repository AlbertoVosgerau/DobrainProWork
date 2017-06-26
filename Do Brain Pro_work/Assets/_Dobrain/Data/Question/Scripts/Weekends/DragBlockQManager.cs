using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Dobrain.contents.weekend
{
    public class DragBlockQManager : WeekendManager
    {
        List<DragBlockQPoint> dropPoints;
        List<DragBlockQItem> dropItems;

        public GameObject attoAnim;
        Animator beforeStepAnim;

        public AudioClip footStepSound;
        public AudioClip oingSound;
        public AudioClip laughSound;

        public GameObject fullBlock;

        public override IEnumerator Initialize(string level)
        {
            
            yield return base.Initialize(level);
            
            ResetGame();
        }

        IEnumerator GameStart()
        {
            fullBlock.SetActive(false);

            audioSorce.clip = footStepSound;
            audioSorce.Play();

            attoAnim.SetActive(false);
            attoAnim.SetActive(true);
            yield return new WaitForSeconds(2f);

            audioSorce.clip = oingSound;
            audioSorce.Play();
            //beforeStepAnim.Stop();
        }

        void InitLists()
        {
            dropItems = new List<DragBlockQItem>(steps[currentStep].GetComponentsInChildren<DragBlockQItem>());
            dropPoints = new List<DragBlockQPoint>(steps[currentStep].GetComponentsInChildren<DragBlockQPoint>());
        }

        void ResetGame()
        {
            InitLists();
            BackPos();
            StartCoroutine(GameStart());
        }

        void BackPos()
        {
            foreach (DragBlockQItem item in dropItems)
                item.Init();
            foreach (DragBlockQPoint point in dropPoints)
                point.Init();
        }


        public void ConfirmAnswer()
        {
            foreach (DragBlockQPoint dropPoint in dropPoints)
                if (!dropPoint.isAnswer)
                    return;

            StartCoroutine(branchAnswer());
        }

        protected override IEnumerator branchAnswer()
        {
            fullBlock.SetActive(true);
            yield return new WaitForSeconds(1f);
            foreach (DragBlockQItem item in dropItems)
                item.transform.DOScale(0, 0);
            attoAnim.GetComponent<Animator>().SetTrigger("Correct");

            audioSorce.clip = laughSound;
            audioSorce.Play();

            yield return new WaitForSeconds(3f);
            
            yield return base.branchAnswer();
            ResetGame();

        }


        public void IncorrectDrop()
        {
            //BackPos();
            IncorrectAnswer();

            //BeforeAnimStart();
            //StartCoroutine(BeforeAnimStop(NarrationDuration));
        }
    }

}
