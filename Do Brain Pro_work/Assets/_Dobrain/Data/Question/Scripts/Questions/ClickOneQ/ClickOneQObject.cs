using UnityEngine;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class ClickOneQObject : MonoBehaviour
    {
        ClickOneQManager questionManager;
        Button button;
        void Awake()
        {
            questionManager = GameObject.FindGameObjectWithTag("QuestionManager").GetComponent<ClickOneQManager>();
            button = GetComponent<Button>();
            button.onClick.AddListener(() => questionManager.ConfirmAnswerWrapper(this.gameObject));
        }
    }
}