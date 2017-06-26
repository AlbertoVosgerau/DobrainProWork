using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Dobrain.contents.quiz;

namespace Dobrain.contents.weekend
{
    public class OrderQPoint : MonoBehaviour
    {
        [HideInInspector]
        public bool isAnswer = false;
        public GameObject[] items;
        public OrderQManager manager;

        Dictionary<int, bool> checkAnswerDict;

        void Awake()
        {
            checkAnswerDict = new Dictionary<int, bool>();
            Init();
        }
        public void Init()
        {
            isAnswer = false;
            foreach (GameObject item in items)
                checkAnswerDict[item.GetInstanceID()] = false;
        }
        public void ConfirmAnswer(OrderQItem item)
        {
            if (checkAnswerDict.ContainsKey(item.gameObject.GetInstanceID()))
            {
                manager.audioSorce.clip = manager.correctSound;
                manager.audioSorce.Play();

                checkAnswerDict[item.gameObject.GetInstanceID()] = true;
                item.transform.position = this.transform.position;
                item.transform.DOPause();
                item.transform.DORotate(Vector3.zero, 1f);
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
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (checkAnswerDict.ContainsKey(other.gameObject.GetInstanceID()))
                checkAnswerDict[other.gameObject.GetInstanceID()] = false;
        }
    }
}