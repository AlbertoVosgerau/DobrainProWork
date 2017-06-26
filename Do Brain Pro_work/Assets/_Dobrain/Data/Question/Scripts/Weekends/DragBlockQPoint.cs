using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Dobrain.contents.weekend
{
    public class DragBlockQPoint : MonoBehaviour
    {
        [HideInInspector]
        public bool isAnswer = false;
        public GameObject[] items;
        public DragBlockQManager manager;
        bool isfull = false;

        Dictionary<int, bool> checkAnswerDict;

        void Awake()
        {
            checkAnswerDict = new Dictionary<int, bool>();
            Init();
        }
        public void Init()
        {
            isfull = false;
            isAnswer = false;
            foreach (GameObject item in items)
                checkAnswerDict[item.GetInstanceID()] = false;
        }
        public void ConfirmAnswer(DragBlockQItem item)
        {
            if (!isfull)
            {
                if (checkAnswerDict.ContainsKey(item.gameObject.GetInstanceID()))
                {
                    checkAnswerDict[item.gameObject.GetInstanceID()] = true;
                    item.transform.position = this.transform.position;
                    isfull = true;
                    isAnswer = true;
                    item.thisCol.enabled = false;


                    manager.audioSorce.clip = manager.correctSound;
                    manager.audioSorce.Play();

                    item.transform.DOPause();
                    item.transform.DORotate(Vector3.zero, 0);
                    item.transform.DOScale(new Vector3(1, 1, 1), 0);
                    manager.ConfirmAnswer();
                }
                else
                {
                    item.Init();
                    manager.IncorrectDrop();
                }
                if (!checkAnswerDict.ContainsValue(false))
                {
                   
                }
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (checkAnswerDict.ContainsKey(other.gameObject.GetInstanceID()))
                checkAnswerDict[other.gameObject.GetInstanceID()] = false;
        }
    }
}