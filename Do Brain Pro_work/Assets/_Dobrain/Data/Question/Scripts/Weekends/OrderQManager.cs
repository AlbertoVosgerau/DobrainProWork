using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

//using Dobrain.contents.quiz;

namespace Dobrain.contents.weekend
{
    public class OrderQManager : WeekendManager
    {
        public Sprite[] CactusPictures;

        List<OrderQPoint> dropPoints;
        List<OrderQItem> dropItems;

        public RectTransform[] Pos;
        public GameObject beforeAnim;
        public Animator beforeStepAnim;
        public RectTransform CacusPool;

        public bool beforeAnimEnd;
        public AudioClip dinongSound;
        public AudioClip dropSound;
        public AudioClip naration;

        public override IEnumerator Initialize(string level)
        {
            //Get userLevel

            string userLevel = level;
            switch (userLevel.ToUpper())
            {
                case "A":
                    steps = ASteps;
                    break;
                case "B":
                    steps = BSteps;
                    break;
                case "C":
                    steps = CSteps;
                    break;
            }
            lastStep = steps.Length == 0 ? 0 : steps.Length - 1;

            steps[currentStep].SetActive(true);
            yield return new WaitForSeconds(4f);
            titleAnim.SetTrigger("onStart");

            ResetGame();
            BackPos();

            yield return new WaitForSeconds(4f);


            StartCoroutine(GameStart());
        }

        IEnumerator GameStart()
        {
            CacusPool = GameObject.Find("CactusPool").GetComponent<RectTransform>();

            beforeAnimEnd = false;

            foreach (OrderQItem item in dropItems)
                item.thisCol.enabled = false;

            yield return new WaitForSeconds(2f);
            beforeAnim.SetActive(true);
            audioSorce.clip = dinongSound;
            audioSorce.Play();
            yield return new WaitForSeconds(1f);
            beforeStepAnim.SetTrigger("Drop");
            yield return new WaitForSeconds(2f);
            audioSorce.clip = dropSound;
            audioSorce.Play();
            yield return new WaitForSeconds(1f);
            beforeStepAnim.Stop();
            beforeAnim.SetActive(false);
            Randombatch();

            CacusPool.DOMoveY(0, 2);

            beforeAnimEnd = true;

            yield return new WaitForSeconds(1f);
            audioSorce.clip = naration;
            audioSorce.Play();
            yield return new WaitForSeconds(1f);
            foreach (OrderQItem item in dropItems)
            {
                item.thisCol.enabled = true;
                item.transform.DOShakeRotation(10f, 10f, 5, 90f, false).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
            }
        }
        void InitLists()
        {
            dropItems = new List<OrderQItem>(steps[currentStep].GetComponentsInChildren<OrderQItem>());
            dropPoints = new List<OrderQPoint>(steps[currentStep].GetComponentsInChildren<OrderQPoint>());
        }

        void ResetGame()
        {
            InitLists();
            beforeStepAnim = GameObject.Find("CactusPool").GetComponent<Animator>();

        }

        void BackPos()
        {
            foreach (OrderQItem item in dropItems)
                item.Init();
            foreach (OrderQPoint point in dropPoints)
                point.Init();
        }

        void RandomPicture()
        {
            List<Sprite> cactusSpriteList = new List<Sprite>(CactusPictures);

            foreach (OrderQItem item in dropItems)
            {
                int targetIndex = Random.Range(0,cactusSpriteList.Count);
                item.GetComponent<Image>().sprite = cactusSpriteList[targetIndex];
                cactusSpriteList.Remove(cactusSpriteList[targetIndex]);
            }
        }

        void Randombatch()
        {
            Pos = GameObject.Find("Pool").GetComponentsInChildren<RectTransform>();

            List<OrderQItem> cactusList = new List<OrderQItem>(steps[currentStep].GetComponentsInChildren<OrderQItem>());
            
            for(int i =1; i<=dropItems.Count; i++)
            {
                int targetIndex = Random.Range(0, cactusList.Count);
                cactusList[targetIndex].GetComponent<RectTransform>().anchoredPosition = Pos[i].anchoredPosition;
                cactusList.Remove(cactusList[targetIndex]);
            }

            foreach (OrderQItem item in dropItems)
                item.originPosition = item.GetComponent<RectTransform>().anchoredPosition;
        }

        public void ConfirmAnswer()
        {
            foreach (OrderQPoint dropPoint in dropPoints)
                if (!dropPoint.isAnswer)
                    return;
            StartCoroutine(branchAnswer());        
        }
        protected override IEnumerator branchAnswer()
        {
            yield return base.branchAnswer();

            if (count < 10)
            {
                ResetGame();

                foreach (OrderQPoint point in dropPoints)
                    point.Init();

                RandomPicture();
                StartCoroutine(GameStart());
            }
        }

        protected override IEnumerator branchAnim()
        {
            yield return base.branchAnim();

            yield return new WaitForSeconds(2f);
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