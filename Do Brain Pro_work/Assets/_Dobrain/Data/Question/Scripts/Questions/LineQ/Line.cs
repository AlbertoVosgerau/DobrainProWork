using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class Line
    {
        public LinePatternQDot start;
        public LinePatternQDot end;
        public GameObject lineGameObject;
        private LineRenderer lineRenderer;
        private bool isAnswerLine = false;
        public bool IsAnswerLine
        {
            get { return isAnswerLine; }
            set { isAnswerLine = value; }
        }
        public Line(float lineWidth, Material lineMaterial)
        {
            lineGameObject = new GameObject("line instance");
            lineGameObject.transform.position = Vector3.zero;
            GameObject lineHolder = GameObject.FindGameObjectWithTag("ObjectHolder");
            lineGameObject.transform.SetParent(lineHolder.transform);
            lineRenderer = lineGameObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.enabled = false;
        }
        public void Render()
        {
            lineRenderer.enabled = true;
            Vector3 startPos = new Vector3(start.transform.position.x, start.transform.position.y, 89);
            Vector3 endPos = new Vector3(end.transform.position.x, end.transform.position.y, 89);
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
}