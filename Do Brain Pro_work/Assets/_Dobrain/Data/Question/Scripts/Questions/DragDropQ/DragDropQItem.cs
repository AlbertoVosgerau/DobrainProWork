using UnityEngine;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class DragDropQItem : DragItem
    {
        QuestionSoundManager qSoundManager;
        DragDropQPoint point = null;
        Shadow shadow;
        public override void Init()
        {
            base.Init();
            point = null;
            shadow = GetComponentInChildren<Shadow>();
            if (shadow != null)
                shadow.enabled = true;

            qSoundManager = GameObject.FindGameObjectWithTag("QuestionSoundManager").GetComponent<QuestionSoundManager>();
        }
        void OnMouseDown()
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            this.GetComponent<RectTransform>().SetAsLastSibling();
        }
        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[1]);
            if (isInArea && point != null)
            {
                if (shadow != null)
                    shadow.enabled = false;
                point.ConfirmAnswer(this);
            }
        }
        protected override void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerStay2D(other);
            point = other.GetComponent<DragDropQPoint>();
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            point = null;
        }
    }
}