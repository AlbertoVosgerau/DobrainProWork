using UnityEngine;

public class DragItem : MonoBehaviour
{
    protected bool isInArea = false;
    protected bool isDragging = false;
    public Vector3 originPosition;
    Canvas mainCanvas;
    public float bigSize = 1.2f;
    RectTransform rectTransform;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originPosition = rectTransform.anchoredPosition;
        GameObject canvasGO = GameObject.FindGameObjectWithTag("MainCanvas");
        mainCanvas = canvasGO.GetComponent<Canvas>();
    }
    public virtual void Init()
    {
        isInArea = false;
        isDragging = false;
        rectTransform.anchoredPosition = originPosition;
    }

    void OnMouseDrag()
    {
        isDragging = true;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform,
            Input.mousePosition, mainCanvas.worldCamera, out pos);
        transform.position = mainCanvas.transform.TransformPoint(pos);
        transform.localScale = new Vector2(bigSize, bigSize);
    }
    protected virtual void OnMouseUp()
    {
        isDragging = false;
        transform.localScale = Vector2.one;
        if (!isInArea)
        {
            Init();
            return;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<DragItem>() != null)
            return;
        isInArea = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<DragItem>() == null)
            isInArea = false;
    }
}
