using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.SimpleAnimation
{
    public class SimpleUISpriteAnimation : MonoBehaviour {

        public Image background;
        public Image view;

        public Sprite[] spriteList;

        public void Play()
        {
            background.gameObject.SetActive(true);
            view.gameObject.SetActive(true);

            StopCoroutine("PlayRoutine");
            StartCoroutine("PlayRoutine");
        }

        public void Stop()
        {
            StopCoroutine("PlayRoutine");

            background.gameObject.SetActive(false);
            view.gameObject.SetActive(false);
        }

        IEnumerator PlayRoutine()
        {
            int count = 0;

            while(true)
            {
                view.sprite = spriteList[count];

                count++;

                if(count.Equals(spriteList.Length))
                    count = 0;

                yield return null;
            }
        }

    }
}