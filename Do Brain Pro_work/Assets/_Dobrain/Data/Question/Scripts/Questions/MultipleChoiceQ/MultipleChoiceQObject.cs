using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class MultipleChoiceQObject : MonoBehaviour
    {
        public bool hideImage;
        public MultipleChoiceQManager manager;
        public float hidetime = 1;
        bool selected;
        Image image;
        Button button;
        RectTransform rect;
        bool isHiding = false;

        void Awake()
        {
            selected = false;
            rect = GetComponent<RectTransform>();
            button = GetComponent<Button>();
            image = GetComponent<Image>();
            button.onClick.AddListener(toggleSelect);
        }
        void OnEnable()
        {
            if (hideImage)
                StartCoroutine(HideOnStart());
        }
        public void toggleSelect()
        {
            if (isHiding)
                return;
            SelectedImageFeedback();
            selected = !selected;
            if (gameObject.tag == "True" && selected && hideImage)
                ShowImage();
            else if (gameObject.tag == "True" && !selected && hideImage)
                HideImage();
            StartCoroutine(manager.ConfirmAnswer(this.gameObject));
        }
        public void Init()
        {
            rect.localScale = Vector2.one;
            selected = false;
        }
        void HideImage()
        {
            Color color = image.color;
            color.a = 0;
            image.color = color;
        }
        public void ShowImage()
        {
            Color color = image.color;
            color.a = 255;
            image.color = color;
        }
        void SelectedImageFeedback()
        {
            if (rect.localScale == new Vector3(1.2f, 1.2f, 1))
                rect.localScale = Vector3.one;
            else
                rect.localScale = new Vector3(1.2f, 1.2f, 1);
        }
        IEnumerator HideOnStart()
        {
            isHiding = true;
            yield return new WaitForSeconds(hidetime);
            HideImage();
            isHiding = false;
        }
        public void ReShowImage()
        {
            ShowImage();
            if (hideImage)
                StartCoroutine(HideOnStart());
        }
    }
}