using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Dobrain.contents.quiz;

namespace Dobrain.contents.weekend
{
    public class BalloonQPoint : MonoBehaviour
    {
        [HideInInspector]
        public bool isAnswer = false;
        public GameObject[] items;
        public BalloonQManager manager;
        public Animator anim;
        public AudioClip charactorSound;

        Dictionary<int, bool> checkAnswerDict;

        void Awake()
        {
            checkAnswerDict = new Dictionary<int, bool>();
            anim = GetComponent<Animator>();
            Init();
        }
        public void Init()
        {
            isAnswer = false;
            foreach (GameObject item in items)
                checkAnswerDict[item.GetInstanceID()] = false;
        }
        public void ConfirmAnswer(BalloonQItem item)
        {
            if (checkAnswerDict.ContainsKey(item.gameObject.GetInstanceID()))
            {
                checkAnswerDict[item.gameObject.GetInstanceID()] = true;
                item.transform.position = this.transform.position;
                manager.audioSorce.clip = charactorSound;
                manager.audioSorce.Play();
                item.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                item.Init();
                manager.IncorrectDrop();
            }
            if (!checkAnswerDict.ContainsValue(false))
            {
                isAnswer = true;
                manager.ConfirmAnswer();
                if (anim != null)
                    anim.SetTrigger("Happy");
                //
                //item.transform.DOMoveY(2.5f,5f);
                
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (checkAnswerDict.ContainsKey(other.gameObject.GetInstanceID()))
                checkAnswerDict[other.gameObject.GetInstanceID()] = false;
        }
    }
}