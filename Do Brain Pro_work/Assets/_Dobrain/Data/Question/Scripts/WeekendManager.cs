using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dobrain.contents
{
    public class WeekendManager : MonoBehaviour
    {
        public delegate void EventHandler(WeekendManager weekendManager);
        public event EventHandler OnComplete;

        public string Type;

        public GameObject[] ASteps;
        public GameObject[] BSteps;
        public GameObject[] CSteps;

        protected GameObject[] steps;
        protected int lastStep = 0;
        protected int currentStep = 0;
        public  int count = 0;
        public AudioSource audioSorce;

        public GameObject[] correctAnim;

        public AudioClip clearSound;
        public AudioClip correctSound;
        public AudioClip IncorrectSound;

        public Animator titleAnim;
        public Button startBtn;
        public GameObject exitAnim;

        public int point = 0;

        public int incorrectCount = 0;

        public int[] crownStack;
        public Sprite[] crowns;
        void Awake()
        {
            audioSorce = GetComponent<AudioSource>();

            correctAnim[0] = GameObject.Find("CorrectAnimationGold");
            correctAnim[1] = GameObject.Find("CorrectAnimationSilver");
            correctAnim[2] = GameObject.Find("CorrectAnimationBronze");

            correctAnim[0].SetActive(false);
            correctAnim[1].SetActive(false);
            correctAnim[2].SetActive(false);

            titleAnim = GameObject.Find("OpeningAnim").GetComponent<Animator>();
            startBtn = GameObject.Find("Circle").GetComponent<Button>();
            exitAnim = GameObject.Find("ExitAnimation");

            exitAnim.GetComponentInChildren<Text>().text = Type + " UP!";
            exitAnim.SetActive(false);

            crownStack = new int[5];
        }

        public virtual IEnumerator Initialize(string level)
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


            yield return new WaitForSeconds(2f);

            titleAnim.SetTrigger("onStart");
            
            yield return new WaitForSeconds(4f);
        }
        
        protected virtual IEnumerator branchAnim()
        {
            yield return null;
        }
         //정답
        protected virtual IEnumerator branchAnswer()
        {
            yield return StartCoroutine(branchAnim());
            if (steps.Length != 0)
                steps[currentStep].SetActive(false);

            yield return StartCoroutine(CorrectAnimStart());
            if (count < 4)
            {
                int randomStep = Random.Range(0, steps.Length);
                currentStep = randomStep;
                steps[currentStep].SetActive(true);

                incorrectCount = 0;
                count++;
            }
            else
            {
                titleAnim.SetTrigger("onExit");
                yield return new WaitForSeconds(4f);
                exitAnim.SetActive(true);
                yield return new WaitForSeconds(3f);
                OnComplete(this);
            }
        }

        protected IEnumerator CorrectAnimStart()
        {
            audioSorce.clip = clearSound;
            audioSorce.Play();

            if (incorrectCount == 0)
            {
                correctAnim[0].SetActive(true);
                yield return new WaitForSeconds(3f);
                correctAnim[0].SetActive(false);
                point = 3;
                crownStack[count] = 3;
                exitAnim.GetComponentsInChildren<Image>()[count].sprite = crowns[2];
            }
            else if(incorrectCount ==1)
            {
                correctAnim[1].SetActive(true);
                yield return new WaitForSeconds(3f);
                correctAnim[1].SetActive(false);
                if (point < 3)
                    point = 2;
                crownStack[count] = 2;
                exitAnim.GetComponentsInChildren<Image>()[count].sprite = crowns[1];
            }
            else
            {
                correctAnim[2].SetActive(true);
                yield return new WaitForSeconds(3f);
                correctAnim[2].SetActive(false);
                if (point < 2)
                    point = 1;
                crownStack[count] = 1;
                exitAnim.GetComponentsInChildren<Image>()[count].sprite = crowns[0];
            }
        }

        //오답
        protected void IncorrectAnswer()
        {
            audioSorce.clip = IncorrectSound;
            audioSorce.Play();
            incorrectCount++;
        }

        void Complete()
        {
            if (OnComplete != null)
                OnComplete(this);
        }
    }
}