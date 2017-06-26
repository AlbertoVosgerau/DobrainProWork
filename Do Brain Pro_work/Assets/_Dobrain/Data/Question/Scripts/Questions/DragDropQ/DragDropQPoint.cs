using System.Collections.Generic;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class DragDropQPoint : MonoBehaviour
    {
        [HideInInspector]
        public bool isAnswer = false;
        public GameObject[] items;
        public DragDropQManager manager;
        
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
        public void ConfirmAnswer(DragItem item)
        {
            if (checkAnswerDict.ContainsKey(item.gameObject.GetInstanceID()))
            {
                checkAnswerDict[item.gameObject.GetInstanceID()] = true;
                item.transform.position = this.transform.position;
            }
            else
                manager.StartCoroutine(manager.IncorrectDrop());
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