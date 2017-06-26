using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OperationQUnit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public delegate void EventHandler(OperationQUnit trigger, OperationQUnit target);
    public event EventHandler OnHitEnter;
    public event EventHandler OnHitExit;

    public enum Type
    {
        Target, Trigger
    }

    public Type type;

    bool isBeginDrag;
    Vector2 startPosition;

    void Awake()
    {
        startPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    void Start()
    {

    }

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(type == Type.Trigger)
            isBeginDrag = true;
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        if(isBeginDrag)
            transform.position = Input.mousePosition;
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        if(type == Type.Trigger)
            isBeginDrag = false;
    }

    #endregion

    void OnTriggerEnter2D(Collider2D hit)
    {
        Debug.Log(0);
        if(OnHitEnter != null)
            OnHitEnter(this, hit.gameObject.GetComponent<OperationQUnit>());
    }

    void OnTriggerExit2D(Collider2D hit)
    {
        if(OnHitExit != null)
            OnHitExit(this, hit.gameObject.GetComponent<OperationQUnit>());
    }

    public void ReturnToStartPosition()
    {
        GetComponent<RectTransform>().anchoredPosition = startPosition;
    }

}
