using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.UI
{
    public class UIContentButton : MonoBehaviour {
        
        public Image bodyShadow;
        public Image bodyLock;
        public Image bodyCurrentLight;
        public Image headOpenCenter;
        public Image headCurrentCenter;
        public Image headOpenOutline;
        public Image neckOpen;


        public Text noViewer;
        public Image giftViewer;
        public Text timeViewer;
        public Image progressViewer;

        [HideInInspector]
        public Button button;
        Animator animator;
        int loadingAnimDirection = 1;
        float loadingProgressStartY;
        float loadingProgress;

        Def.ContentState state;
        bool isSettingLoadingProgress;


        void Awake()
        {
            button = GetComponent<Button>();
            animator = GetComponent<Animator>();
            noViewer.text = "";
            loadingProgressStartY = progressViewer.GetComponent<RectTransform>().localPosition.y;

            SetState(Def.ContentState.Lock);
        }

        void Update()
        {
            if(isSettingLoadingProgress)
            {
                RectTransform transform = progressViewer.GetComponent<RectTransform>();
                float minX = -250f;
                float maxX = 250f;
                float x = 0;
                if(loadingAnimDirection.Equals(-1))
                {
                    x = transform.localPosition.x - 5f;
                    if(x < minX)
                        loadingAnimDirection = 1;
                }
                else
                {
                    x = transform.localPosition.x + 5f;
                    if(maxX < x)
                        loadingAnimDirection = -1;
                }

                transform.Translate(5f * loadingAnimDirection, 0f, 0f, Space.Self);
//                transform.localPosition = new Vector2(x, transform.localPosition.y);
            }
        }

        public void SetNo(int no)
        {
            noViewer.text = no.ToString();
        }

        public void SetState(Def.ContentState state)
        {
            if(state == Def.ContentState.Lock || state == Def.ContentState.HalfOpen)
            {
                bodyCurrentLight.gameObject.SetActive(false);
                headOpenCenter.gameObject.SetActive(false);
                headCurrentCenter.gameObject.SetActive(false);
                headOpenOutline.gameObject.SetActive(false);
                neckOpen.gameObject.SetActive(false);

                giftViewer.gameObject.SetActive(false);
            }
            else if(state == Def.ContentState.Open || state == Def.ContentState.Cleared)
            {
                bodyCurrentLight.gameObject.SetActive(true);
                headOpenCenter.gameObject.SetActive(true);
                headCurrentCenter.gameObject.SetActive(true);
                headOpenOutline.gameObject.SetActive(true);
                neckOpen.gameObject.SetActive(true);
            }

            this.state = state;
        }

        public void PlayAnimation(string ani)
        {
            animator.Play(ani);
            transform.SetSiblingIndex(30);
        }

        public void SetGift(Sprite gift)
        {
            giftViewer.gameObject.SetActive(true);
            giftViewer.sprite = gift;
        }

        public void SetWaitingTime(int[] timeData)
        {
            int day = timeData[0];
            int hour = timeData[1];
            int minute = timeData[2];

            string time = "";

            if(0 < day)
                time = string.Format("{0}일", day);

            if(0 < hour)
            {
                if(string.IsNullOrEmpty(time))
                    time = string.Format("{0}시간", hour);
                else
                    time+= " " + string.Format("{0}시간", hour);
            }

            if(string.IsNullOrEmpty(time))
                time = string.Format("{0}분", minute);


            time += "\n남음";

            timeViewer.text = time;
        }

        public void SetLoaingProgress(float progress)
        {
            StopCoroutine("SetLoadingProgressRoutine");

            if(0f == progress)
            {
                isSettingLoadingProgress = false;
                progressViewer.GetComponent<RectTransform>().localPosition = new Vector2(0f, loadingProgressStartY);
            }
            else
            {
                if(!isSettingLoadingProgress)
                {
                    isSettingLoadingProgress = true;
                    progressViewer.GetComponent<RectTransform>().localPosition = new Vector2(0f, loadingProgressStartY + 50f);
                }
                StartCoroutine("SetLoadingProgressRoutine", progress);
            }
        }

        IEnumerator SetLoadingProgressRoutine(float progress)
        {
            RectTransform transform = progressViewer.GetComponent<RectTransform>();
            float targetY = loadingProgressStartY + (750f * progress);

            while(true)
            {
//                float y = transform.localPosition.y + 5f;

                transform.Translate(0f, 5f, 0f, Space.Self);
//                transform.localPosition = new Vector2(transform.localPosition.x, y);

                if(targetY <= transform.localPosition.y)
                    yield break;

                yield return null;
            }
        }

    }
}
