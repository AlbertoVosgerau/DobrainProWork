using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class DistributionQPoint : MonoBehaviour
    {
        [HideInInspector]
        public bool isAnswer = false;
        public int distributionNum = 0;
        int count;
        public DistributionQManager manager;

        void Start()
        {
            Init();
        }
        public void Init()
        {
            count = distributionNum;
            isAnswer = false;
        }
        public void ConfirmAnswer(DragItem item)
        {
            if (count > 0)
            {
                item.transform.position = this.transform.position;
                count--;
                if (count == 0)
                {
                    isAnswer = true;
                    manager.ConfirmAnswer();
                }
            }
            else
                StartCoroutine(manager.IncorrectDrop());
        }
    }
}