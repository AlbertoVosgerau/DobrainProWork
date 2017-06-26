using UnityEngine;
using System.Collections;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class StackQObject : MonoBehaviour
    {
        public enum Shape
        {
            None, Triangle, Polygon
        }

        public Shape shape;

        private StackQManager stackQManager;
        private IEnumerator coroutine = null;
        private Rigidbody2D rb2d;
        private bool isDragging = false;
        private AudioSource soundEffect;
        private Animator anim;

        void Awake()
        {
            if(shape != Shape.None)
            {
                PolygonCollider2D polygon = gameObject.AddComponent<PolygonCollider2D>();

                if(shape == Shape.Triangle)
                {
                    Vector2[] points = new Vector2[3];
                    points[0] = new Vector2(0f, 1.3f);
                    points[1] = new Vector2(-1.4f, -1.1f);
                    points[2] = new Vector2(1.4f, -1.1f);
                    polygon.points = points;
                }
                else if(shape == Shape.Polygon)
                {
                    Vector2[] points = new Vector2[6];
                    points[0] = new Vector2(-0.7f, 1.2f);
                    points[1] = new Vector2(0.7f, 1.2f);
                    points[2] = new Vector2(1.4f, 0f);
                    points[3] = new Vector2(0.7f, -1.2f);
                    points[4] = new Vector2(-0.7f, -1.2f);
                    points[5] = new Vector2(-1.4f, 0f);
                    polygon.points = points;
                }
            }

        }

        void Start()
        {
            soundEffect = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
            stackQManager = GameObject.Find("StackQManager").GetComponent<StackQManager>();
            rb2d = GetComponent<Rigidbody2D>();
        }
        public void SelectObject(Vector3 mousePos)
        {

            if (!soundEffect.isPlaying)
                soundEffect.Play();
            if (!anim.GetBool("pushed"))
                anim.SetBool("pushed", true);
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            transform.position = new Vector3(mousePos.x,mousePos.y,0);
            
            isDragging = true;
        }
        public void UnselectObject()
        {
            if (soundEffect.isPlaying)
                soundEffect.Stop();
            anim.SetBool("pushed", false);
            isDragging = false;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            
            if (!isDragging)
            {
                coroutine = stayOnGoalLine();
                StartCoroutine(coroutine);
            }
            else if (isDragging)
                StopAllCoroutines();
            if (rb2d.velocity.x > 0.01f)
                StopAllCoroutines();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            StopAllCoroutines();
        }

        IEnumerator stayOnGoalLine()
        {
            yield return new WaitForSeconds(1.0f);
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            stackQManager.ConfirmAnswer();
        }
    }
}