using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler {
    
    public delegate void EventHandler(PointerEventData data);
    public event EventHandler OnEndDragEvent;
    public event EventHandler OnBeginDragEvent;

    public ScrollRect scrollRect;

    public Vector2 normalizedPosition { get{ return scrollRect.normalizedPosition; }}
    public Vector2 velocity { get{ return scrollRect.velocity; }}

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(OnBeginDragEvent != null)
            OnBeginDragEvent(eventData);
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        if(OnEndDragEvent != null)
            OnEndDragEvent(eventData);
    }
    #endregion

}
