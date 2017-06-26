using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dobrain.contents.weekend
{
    public class FoodQItem : DragItem
    {
        BoxCollider2D bx2d;
        FoodQPoint point = null;
        protected override void Awake()
        {
            base.Awake();
            bx2d = GetComponent<BoxCollider2D>();
        }
        public override void Init()
        {
            point = null;
            bx2d.enabled = true;
            base.Init();
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
            point = other.GetComponent<FoodQPoint>();
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            point = null;
        }
    }
}