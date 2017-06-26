using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dobrain.contents.weekend
{
    public class FoodQManager : WeekendManager
    {
        List<FoodQPoint> FoodPoints;
        List<FoodQItem> dropItems;

        public Sprite[] foodPictures;

        GameObject foodPanel;

        public AudioClip plateSound;
        public AudioClip[] naration;

        public override IEnumerator Initialize(string level)
        {
            yield return base.Initialize(level);

            StartCoroutine(GameStart());
        }

        IEnumerator GameStart()
        {
            Init();
            RandomPicture();

            foodPanel = GameObject.Find("FoodPanel");
            foodPanel.GetComponent<Animator>().enabled = true;
            audioSorce.clip = plateSound;
            audioSorce.Play();

            yield return new WaitForSeconds(1.5f);

            audioSorce.clip = naration[Random.Range(0,naration.Length)];
            audioSorce.Play();
        }

        void Init()
        {
            dropItems = new List<FoodQItem>(steps[currentStep].GetComponentsInChildren<FoodQItem>());
            FoodPoints = new List<FoodQPoint>(steps[currentStep].GetComponentsInChildren<FoodQPoint>());

            foreach (FoodQItem item in dropItems)
                item.Init();
            foreach (FoodQPoint point in FoodPoints)
                point.Init();
        }
        void RandomPicture()
        {
            int targetIndex = Random.Range(0, foodPictures.Length);

            foreach (FoodQItem item in dropItems)
                item.GetComponent<Image>().sprite = foodPictures[targetIndex];
        }

        public void ConfirmAnswer()
        {
            foreach (FoodQPoint FoodPoint in FoodPoints)
                if (!FoodPoint.isAnswer)
                    return;
            StartCoroutine(branchAnswer());
        }
        public void IncorrectDrop()
        {
            IncorrectAnswer();
        }

        protected override IEnumerator branchAnim()
        {
            yield return base.branchAnim();
            yield return new WaitForSeconds(1f);
        }
        protected override IEnumerator branchAnswer()
        {
            yield return StartCoroutine(base.branchAnswer());
            Init();
            StartCoroutine(GameStart());
        }
    }
}