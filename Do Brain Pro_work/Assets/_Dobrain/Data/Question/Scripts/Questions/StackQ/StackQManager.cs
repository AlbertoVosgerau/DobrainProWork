using System.Collections;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class StackQManager : QuestionManager
    {

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);
            qSoundManager.PlayQuestionSound();
        }
        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mousePos.z = Mathf.Infinity;
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Element")
            {
                mousePos.z = 0;
                if (Input.GetMouseButton(0))
                {
                    StackQObject stackObject = hit.collider.GetComponent<StackQObject>();
                    stackObject.SelectObject(mousePos);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    StackQObject stackObject = hit.collider.GetComponent<StackQObject>();
                    stackObject.UnselectObject();
                }
            }
        }
        public void ConfirmAnswer()
        {
            StartCoroutine(base.branchAnswer());
        }
    }
}