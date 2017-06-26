using System.Collections;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public enum MoveType
    {
        random,
        donw
    }
    public class WhackAMoleQObject : MonoBehaviour
    {
        public WhackAMoleQManager whackAMoleQManager;
        public MoveType moveType = MoveType.random;
        public int chance = 3;
        public float xBound = 700;
        public float yBound = 325;
        CircleCollider2D col;
        public float speed = 2000;
        public float downSpeed = 0.03f;
        float timeGap = 1f;
        float fadeDuration = 0.25f;
        RectTransform rectTransform;
        Vector2 randomDirection;
        Animator fadeAnim;
        bool isMoving;


        void Start()
        {
            col = GetComponent<CircleCollider2D>();
            rectTransform = GetComponent<RectTransform>();
            isMoving = whackAMoleQManager.isMoving;
            Vector2 startPos = new Vector2(Random.Range(-xBound, xBound), Random.Range(-yBound, yBound));
            rectTransform.anchoredPosition = startPos;
            if (isMoving)
            {
                chance = 0;
                StartCoroutine(ChangePositionCoroutine());
            }
            else
            {
                fadeAnim = GetComponent<Animator>();
            }
            if (gameObject.tag == "False")
                chance = 0;
        }

        void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            if (isMoving)
                StartCoroutine(ChangePositionCoroutine());
        }

        void Update()
        {
            if (isMoving)
            {
                if (moveType == MoveType.random)
                {
                    rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, randomDirection, speed * Time.deltaTime);
                    Mathf.Clamp(rectTransform.anchoredPosition.x, -xBound, xBound);
                    Mathf.Clamp(rectTransform.anchoredPosition.y, -yBound, yBound);
                }
                else
                {
                    if (rectTransform.anchoredPosition.y > -yBound)
                        rectTransform.Translate(new Vector3(0.5f, -1) * downSpeed);
                    else
                    {
                        rectTransform.anchoredPosition = new Vector2(Random.Range(-xBound, xBound / 2), yBound);
                    }
                }

            }
        }

        public void Init()
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = Vector2.one;
        }

        IEnumerator ChangePositionCoroutine()
        {
            while (true)
            {
                ChangePosition();
                yield return new WaitForSecondsRealtime(timeGap);
            }
        }
        void ChangePosition()
        {
            randomDirection = new Vector2(Random.Range(-xBound, xBound), Random.Range(-yBound, yBound));
            if (!isMoving)
                rectTransform.anchoredPosition = randomDirection;
        }

        void OnMouseDown()
        {
            if (isMoving)
            {
                rectTransform.localScale *= 1.2f;
                whackAMoleQManager.ConfirmAnswer(this.gameObject);
            }
            else
            {
                if (chance > 1)
                {
                    chance--;
                    whackAMoleQManager.FadeOutAndIn();
                }
                else
                {
                    whackAMoleQManager.ConfirmAnswer(this.gameObject);
                }
            }
        }

        IEnumerator FadeIn()
        {
            //yield return new WaitForSecondsRealtime(fadeDuration);
            fadeAnim.SetTrigger("fadeIn");
            ChangePosition();
            col.enabled = true;
            yield return null;
        }

        void FadeOut()
        {
            col.enabled = false;
            fadeAnim.SetTrigger("fadeOut");
        }

        public void FadeOutAndIn()
        {
            FadeOut();
            StartCoroutine(FadeIn());
        }
    }
}