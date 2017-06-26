using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Dobrain.contents.weekend
{
    public class BalloonQManager : WeekendManager
    {
        List<BalloonQPoint> dropPoints;
        List<BalloonQItem> dropItems;

        public Sprite[] BalloonPictures;

        public RectTransform[] Pos;
        public GameObject beforeAnim;
        Animator beforeStepAnim;
        GameObject rope;

        public AudioClip beforeDinong;
        public AudioClip dinongSound;
        public AudioClip naration;
        public AudioClip shockSound;
        public AudioClip BalloonflySound;

        void Start()
        {
            StartCoroutine(Initialize("C"));
        }

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
            RandomPicture();

            yield return new WaitForSeconds(4f);
            StartCoroutine(GameStart());
        }

        IEnumerator GameStart()
        {
           

            rope = GameObject.Find("Rope");
            audioSorce.clip = beforeDinong;
            audioSorce.Play();
            yield return new WaitForSeconds(2f);
            beforeAnim.SetActive(true);
            audioSorce.clip = dinongSound;
            audioSorce.Play();

            yield return new WaitForSeconds(1f);
            foreach (BalloonQPoint point in dropPoints)
                point.GetComponent<Animator>().SetTrigger("Shock");
            yield return new WaitForSeconds(2f);
            audioSorce.clip = shockSound;
            audioSorce.Play();

            beforeAnim.SetActive(false);

            yield return new WaitForSeconds(0.3f);
            rope.transform.DOScaleY(0, 0);
            yield return new WaitForSeconds(0.3f);
            rope.transform.DOScaleY(1, 0);
            yield return new WaitForSeconds(0.3f);
            rope.transform.DOScaleY(0, 0);
            yield return new WaitForSeconds(0.3f);

            beforeStepAnim.enabled = true;
            audioSorce.clip = BalloonflySound;
            audioSorce.Play();

            yield return new WaitForSeconds(2f);
            audioSorce.clip = naration;
            audioSorce.Play();

            foreach (BalloonQPoint point in dropPoints)
                point.GetComponent<Animator>().SetTrigger("Sad");
            beforeStepAnim.Stop();
            GameObject.Find("BalloonPool").transform.DOMoveY(0, 2f);

            Randombatch();

            foreach (BalloonQItem item in dropItems)
                item.GetComponent<BoxCollider2D>().enabled = true;


        }
        void InitLists()
        {
            dropItems = new List<BalloonQItem>(steps[currentStep].GetComponentsInChildren<BalloonQItem>());
            dropPoints = new List<BalloonQPoint>(steps[currentStep].GetComponentsInChildren<BalloonQPoint>());
        }

        void ResetGame()
        {
            InitLists();
            //BackPos();
            beforeStepAnim = GameObject.Find("BalloonPool").GetComponent<Animator>();
            rope = GameObject.Find("Rope");
            beforeStepAnim.enabled = false;
            

        }
        void BackPos()
        {
            foreach (BalloonQItem item in dropItems)
                item.Init();
            foreach (BalloonQPoint point in dropPoints)
                point.Init();
        }

        void RandomPicture()
        {
            List<Sprite> balloonSpriteList = new List<Sprite>(BalloonPictures);

            foreach (BalloonQItem item in dropItems)
            {
                int targetIndex = Random.Range(0, balloonSpriteList.Count);
                item.GetComponent<Image>().sprite = balloonSpriteList[targetIndex];
                balloonSpriteList.Remove(balloonSpriteList[targetIndex]);
            }
        }


        void Randombatch()
        {
            Pos = GameObject.Find("Pool").GetComponentsInChildren<RectTransform>();

            List<BalloonQItem> cactusList = new List<BalloonQItem>(steps[currentStep].GetComponentsInChildren<BalloonQItem>());

            for (int i = 1; i <= dropItems.Count; i++)
            {
                int targetIndex = Random.Range(0, cactusList.Count);
                cactusList[targetIndex].GetComponent<RectTransform>().anchoredPosition = Pos[i].anchoredPosition;
                cactusList.Remove(cactusList[targetIndex]);
            }

            foreach (BalloonQItem item in dropItems)
                item.originPosition = item.GetComponent<RectTransform>().anchoredPosition;
        }

        public void ConfirmAnswer()
        {
            foreach (BalloonQPoint dropPoint in dropPoints)
                if (!dropPoint.isAnswer)
                    return;

            StartCoroutine(branchAnswer());


        }
        protected override IEnumerator branchAnswer()
        {
            rope.transform.DOScaleY(1, 1.8f);
            foreach (BalloonQItem item in dropItems)
                item.GetComponent<RectTransform>().DOLocalMoveY(351f, 2f);
            yield return new WaitForSeconds(3f);

            yield return base.branchAnswer();
            if (count < 10)
            {
                ResetGame();
                RandomPicture();
                foreach (BalloonQPoint point in dropPoints)
                    point.Init();

                StartCoroutine(GameStart());
            }
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