using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlide : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

    public ScrollRect scrollRect;
    public int pageNum;

    int currentPage = 0;
    float dragBeginPosition;
    bool isScrolling;
    float canScrollDistance;

    void Awake()
    {
        canScrollDistance = 0.2f / pageNum;
    }


    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isScrolling)
            dragBeginPosition = scrollRect.horizontalNormalizedPosition;
    }
    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isScrolling)
        {
            float dragEndPosition = scrollRect.horizontalNormalizedPosition;

            float dragMoveDistance = dragEndPosition - dragBeginPosition;

            if(canScrollDistance < dragMoveDistance)
                Move(1);
            else if(dragMoveDistance < -canScrollDistance)
                Move(-1);
            else
                Move(0);
        }
    }

    #endregion

    void Move(int direction)
    {
        StartCoroutine(MoveRoutine(direction));
    }

    IEnumerator MoveRoutine(int direction)
    {
        scrollRect.horizontal = false;

        float scrollSpeed = 0.01f;

        isScrolling = true;

        if(direction != 0)
            currentPage += direction;

        float targetPosition = (float)currentPage / (float)(pageNum - 1);
        float position = scrollRect.horizontalNormalizedPosition;

        if(position < targetPosition)
            direction = 1;
        else if(targetPosition < position)
            direction = -1;

        if(direction == 1)
        {
            while(position < targetPosition)
            {
                yield return null;
                position += scrollSpeed;
                scrollRect.horizontalNormalizedPosition = position;
            }
        }
        else if(direction == -1)
        {
            while(targetPosition < position)
            {
                yield return null;
                position -= scrollSpeed;
                scrollRect.horizontalNormalizedPosition = position;
            }
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;

        isScrolling = false;

        scrollRect.horizontal = true;
    }

}
