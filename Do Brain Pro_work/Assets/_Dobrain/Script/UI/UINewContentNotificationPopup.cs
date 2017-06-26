using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.UI.Home
{
    public class UINewContentNotificationPopup : MonoBehaviour 
    {
        public delegate void EventHandler();
        public event EventHandler OnClose;

        public Animator animator;
        public GameObject panel;


        public void Show(bool visible)
        {
            if(visible)
            {
                animator.Play("Show");
                StartCoroutine("Routine");
            }
            else
                animator.Play("Hide");
        }

        IEnumerator Routine()
        {
            yield return new WaitForSeconds(1f);

            float count = 0f;

            while(true)
            {
                if(Input.GetMouseButtonDown(0))
                    break;

                count += Time.deltaTime;
                if(4f < count)
                    break;

                yield return null;
            }

            Show(false);

            yield return new WaitForSeconds(1f);

            if(OnClose != null)
                OnClose();
        }



    }
}
