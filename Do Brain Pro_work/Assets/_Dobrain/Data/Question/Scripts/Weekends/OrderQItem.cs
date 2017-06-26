using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dobrain.contents.weekend
{
    public class OrderQItem : DragItem
    {
        OrderQPoint point = null;
        OrderQManager manager;
        public BoxCollider2D thisCol;

        protected override void Awake()
        {
            base.Awake();
            manager = GameObject.Find("OrderQManager").GetComponent<OrderQManager>();
            thisCol = GetComponent<BoxCollider2D>();
        }

        public override void Init()
        {
            base.Init();
            point = null;
            transform.DOPlay();
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
            point = other.GetComponent<OrderQPoint>();
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            point = null;
        }

    }
}