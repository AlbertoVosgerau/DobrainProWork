using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Dobrain.contents;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class QuestionManager : MonoBehaviour
    {
        public delegate void EventHandler(QuestionManager questionManager);
        public event EventHandler OnComplete;

        public MissionManager missionManager;

        public const int ABILITY_NUM = 7;
        public enum ABILITY
        {
            None = 0,
            VelocityPerceptual, SpacePerceptual, Organizing,
            Inference, Numerical, Memory, Discrimination
        }
        public ABILITY ability = ABILITY.None;

        public string questionString = "";

        public GameObject[] ASteps;
        public GameObject[] BSteps;
        public GameObject[] CSteps;

        public Animator[] aAnimators;
        public Animator[] bAnimators;
        public Animator[] cAnimators;
        public Animator[] aAfterAnim;
        public Animator[] bAfterAnim;
        public Animator[] cAfterAnim;

        public float afterAnimDuration = 0;

        public AudioClip BeforeAnimSound = null;
        public AudioClip QuestionSound = null;

        protected Animator[] animators;
        protected Animator[] afterAnims;
        protected GameObject[] steps;
        protected char[] score = { 'A', 'B', 'C', 'D' };
        protected int scoreChance = 0;
        protected int lastStep = 0;
        protected int currentStep = 0;

        protected QuestionSoundManager qSoundManager;

        private float correctAnimDuration = 2;
        private float subCorrectAnimDuration = 1.5f;
        protected float incorrectAnimDuration = 1;
        private float qBoardStartAnimDuration = 0.1f;
        private Animator correctAnim;

        protected int thischapter;
        protected int thisindex;
        protected float recordscore;
        protected int questioncount;

        void Start()
        {
            StartCoroutine(Initialize(1, 1, "C"));
        }
       
        public virtual IEnumerator Initialize(int ch,int index, string level)
        {
            missionManager = GameObject.Find("Scene Question").GetComponent<MissionManager>();

            correctAnim = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Animator>();
            qSoundManager = GameObject.FindGameObjectWithTag("QuestionSoundManager").GetComponent<QuestionSoundManager>();
            if (BeforeAnimSound != null)
                qSoundManager.SetBeforeAnimSound(BeforeAnimSound);
            if (QuestionSound != null)
                qSoundManager.SetQuestionSound(QuestionSound);

            thischapter = ch;
            thisindex = index;

            string userLevel = level;
            switch (userLevel.ToUpper())
            {
                case "A":
                    steps = ASteps;
                    animators = aAnimators;
                    afterAnims = aAfterAnim;
                    break;
                case "B":
                    steps = BSteps;
                    animators = bAnimators;
                    afterAnims = bAfterAnim;
                    break;
                case "C":
                    steps = CSteps;
                    animators = cAnimators;
                    afterAnims = cAfterAnim;
                    break;
            }

            lastStep = steps.Length == 0 ? 0 : steps.Length - 1;
            Text questionText = GameObject.FindGameObjectWithTag("QuestionText").GetComponent<Text>();
            questionText.text = questionString;
            Text categoryText = GameObject.FindGameObjectWithTag("CategoryText").GetComponent<Text>();
            categoryText.text = AbilityToText();

            yield return new WaitForSeconds(1.5f);

            steps[currentStep].SetActive(false);
            yield return new WaitForSeconds(qBoardStartAnimDuration);
            steps[currentStep].SetActive(true);
        }

        protected virtual IEnumerator branchAnswer()
        {
            if (steps.Length != 0)
            {
                yield return new WaitForSeconds(0.5f);
                steps[currentStep++].SetActive(false);
            }

            yield return StartCoroutine(AfterAnimStart(afterAnimDuration));
            
            if (currentStep > lastStep)
            {
                ScoreCalculate(scoreChance);
                questioncount++;

                yield return StartCoroutine(CorrectAnimStart());

                if (AbilityToText() != "창의력")
                    missionManager.Record(System.DateTime.Today.ToString("yyyy-MM-dd"), thischapter, thisindex, AbilityToText(), recordscore / questioncount);
                else
                    missionManager.Record(System.DateTime.Today.ToString("yyyy-MM-dd"), thischapter, thisindex, AbilityToText(), 0);

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

        protected virtual void BeforeAnimStart()
        {
            if (currentStep < steps.Length)
                steps[currentStep].SetActive(false);
            if (animators.Length > currentStep && animators[currentStep] != null)
            {
                animators[currentStep].gameObject.SetActive(true);
                qSoundManager.PlayBeforeSound();
                if (animators[currentStep].parameterCount != 0)
                    animators[currentStep].SetTrigger("beforeStart");
            }
        }

        protected virtual IEnumerator BeforeAnimStop(float time)
        {
            if (animators.Length > currentStep && animators[currentStep] != null)
            {
                yield return new WaitForSeconds(time);
                if (animators[currentStep].parameterCount != 0)
                    animators[currentStep].SetTrigger("beforeStop");
                animators[currentStep].gameObject.SetActive(false);
            }
            if (currentStep < steps.Length)
                steps[currentStep].SetActive(true);
            qSoundManager.PlayQuestionSoundJustOnce();
        }

        protected virtual IEnumerator AfterAnimStart(float time)
        {
            if (afterAnims.Length > currentStep - 1 && afterAnims[currentStep - 1] != null)
            {
                afterAnims[currentStep - 1].gameObject.SetActive(true);
                yield return new WaitForSeconds(time);
                afterAnims[currentStep - 1].gameObject.SetActive(false);
            }
            else
                yield return null;
        }

        protected IEnumerator IncorrectAnswer()
        {
            yield return new WaitForSeconds(0.5f);
            steps[currentStep].SetActive(false);
            scoreChance++;

            qSoundManager.StopAll();
            correctAnim.SetTrigger("IncorrectAnim");
            yield return new WaitForSeconds(incorrectAnimDuration);
            steps[currentStep].SetActive(true);
        }
        protected IEnumerator CorrectAnimStart()
        {
            qSoundManager.StopAll();
            correctAnim.SetTrigger("CorrectAnim");
            yield return new WaitForSeconds(correctAnimDuration);
        }
        protected IEnumerator SubCorrectAnimStart()
        {
            qSoundManager.StopAll();
            correctAnim.SetTrigger("SmallCorrectAnim");
            yield return new WaitForSeconds(subCorrectAnimDuration);
        }
        protected string AbilityToText()
        {
            string ret = "";
            switch (ability)
            {
                case ABILITY.Discrimination:
                    ret = "변별력";
                    break;
                case ABILITY.Inference:
                    ret = "추론력";
                    break;
                case ABILITY.Memory:
                    ret = "기억력";
                    break;
                case ABILITY.Numerical:
                    ret = "수리력";
                    break;
                case ABILITY.Organizing:
                    ret = "구성력";
                    break;
                case ABILITY.SpacePerceptual:
                    ret = "공간지각";
                    break;
                case ABILITY.VelocityPerceptual:
                    ret = "지각속도";
                    break;
                case ABILITY.None:
                    ret = "창의력";
                    break;
                default:
                    ret = "변별력";
                    break;
            }
            return ret;
        }

        protected virtual void ScoreCalculate(int incorrectCount)
        {
            int _recordscore;

            switch (incorrectCount)
            {
                case 0:
                    _recordscore = 10;
                    break;
                case 1:
                    _recordscore = 5;
                    break;
                default:
                    _recordscore = 3;
                    break;
            }

            recordscore += _recordscore;

        }

        protected void Complete()
        {
            if(OnComplete != null)
                OnComplete(this);
        }
    }
}