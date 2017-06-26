using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dobrain.contents.weekend
{
    public class DragBlockQItem : DragItem
    {
        public DragBlockQPoint point = null;
        DragBlockQManager manager;
        public Collider2D thisCol;

        protected override void Awake()
        {
            base.Awake();
            thisCol = GetComponent<Collider2D>();       
        }
         public override void Init()
        {
            base.Init();
            transform.DOScale(1, 1);
            thisCol.enabled = true;
            point = null;
            transform.DOShakeRotation(10f, 10f, 2, 90f, false).SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
            transform.DOShakeRotation(10f, 20f, 2, 90f, false).SetLoops(-1, LoopType.Yoyo).SetSpeedBased(); 
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            if (isInArea && point != null)
            {
                point.ConfirmAnswer(this);
            }
        }
        protected override void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerStay2D(other);
            point = other.GetComponent<DragBlockQPoint>();
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            point = null;
        }

        void Update()
        {
            if(isDragging)
            {
                transform.DORotate(Vector3.zero, 0);
                transform.DOScale(new Vector3(1,1,1), 0);
            }
        }
    }
}