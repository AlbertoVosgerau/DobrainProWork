using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class DistributionQItem : DragItem
    {
        QuestionSoundManager qSoundManager;
        BoxCollider2D bx2d;
        DistributionQPoint point = null;
        protected override void Awake()
        {
            base.Awake();
            bx2d = GetComponent<BoxCollider2D>();
            qSoundManager = GameObject.FindGameObjectWithTag("QuestionSoundManager").GetComponent<QuestionSoundManager>();
        }
        public override void Init()
        {
            point = null;
            bx2d.enabled = true;
            base.Init();
        }

        void OnMouseDown()
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            this.GetComponent<RectTransform>().SetAsLastSibling();
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            if (isInArea && point != null)
            {
                bx2d.enabled = false;
                point.ConfirmAnswer(this);
            }
        }
        protected override void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerStay2D(other);
            point = other.GetComponent<DistributionQPoint>();
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            point = null;
        }
    }
}