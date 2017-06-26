using UnityEngine;

namespace Dobrain.contents.weekend
{
    public class FoodQPoint : MonoBehaviour
    {
        [HideInInspector]
        public bool isAnswer = false;
        public int distributionNum = 0;
        public int count;
        public FoodQManager manager;
        int x;

        void Start()
        {
            Init();
        }
        public void Init()
        {
            count = distributionNum;
            x = 0;
            isAnswer = false;
        }
        public void ConfirmAnswer(FoodQItem item)
        {
            if (count > 0)
            {
                manager.audioSorce.clip = manager.correctSound;
                manager.audioSorce.Play();

                item.transform.position = new Vector3(this.transform.position.x + 0.1f * x, this.transform.position.y + 0.2f * x, this.transform.position.z);
                count--;
                x+=2;

                item.GetComponent<RectTransform>().SetAsLastSibling();

                if (count == 0)
                {  
                    isAnswer = true;
                    manager.ConfirmAnswer();
                    
                }
            }
            else
            {
                item.Init();
                manager.IncorrectDrop();
            }
            
        }
    }
}