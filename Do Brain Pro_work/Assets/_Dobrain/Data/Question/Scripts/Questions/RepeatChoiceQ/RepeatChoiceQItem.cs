using UnityEngine;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class RepeatChoiceQItem : MonoBehaviour
    {
        public int repeatNum = 0;
        public RepeatChoiceQManager manager;

        void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(ClickItem);
        }
        void ClickItem()
        {
            StartCoroutine(manager.ChoiceItem(this));
        }
    }
}