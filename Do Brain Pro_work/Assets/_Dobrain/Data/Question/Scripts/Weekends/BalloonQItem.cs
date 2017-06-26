using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dobrain.contents.weekend
{
    public class BalloonQItem : DragItem
    {
        BalloonQPoint point = null;
        public override void Init()
        {
            base.Init();
            point = null;
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
            point = other.GetComponent<BalloonQPoint>();
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            point = null;
        }
    }
}