using UnityEngine;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class EraseQObject : MonoBehaviour
    {
        public EraseQManager manager;
        public bool isDone;
        public byte fadeSpeed;
        private Image image;
        private Color32 fadeColor;
        BoxCollider2D bx2d;
        void Awake()
        {
            bx2d = GetComponent<BoxCollider2D>();
            image = GetComponent<Image>();
            Init();
        }

        void OnMouseOver()
        {
            if (fadeColor.a > 20)
                fadeColor.a -= fadeSpeed;
            if (fadeColor.a <= 20)
            {
                isDone = true;
                manager.ConfirmAnswer();
                fadeColor.a = 0;
                bx2d.enabled = false;
            }
            image.color = fadeColor;
        }

        void Init()
        {
            showImage();
            fadeColor = image.color;
            isDone = false;
        }
        void showImage()
        {
            Color32 tempColor = image.color;
            tempColor.a = 255;
            image.color = tempColor;
        }
    }
}
