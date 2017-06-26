using UnityEngine;
using System.Collections;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class LinePatternQManager : QuestionManager
    {
        public string[] aCorrectAnswerString;
        public string[] bCorrectAnswerString;
        public string[] cCorrectAnswerString;
        public Material lineMaterial;
        public float dotRadius = 1.0f;
        public float lineWidth = 1.0f;
        public float yAdjustValue = 1.0f;

        private string[] correctAnswerString;
        private int numOfDots;
        private LinePatternQDot[] dots;
        private LinePatternQDot prevDot;
        private CircleCollider2D[] colliders;
        private LineRenderer lineRenderer;
        private int lineNum;
        private bool oneTime;
        private bool trying;
        private Graph answer;
        private Graph correctAnswer;
        private GameObject lineHolder;
        private int chance;
        private Transform patternLight;
        private int isInDot = -1;
        public int IsIn
        {
            get { return isInDot; }
            set { isInDot = value; }
        }
        public Graph CorrectAnswerGraph
        {
            get { return correctAnswer; }
            set { correctAnswer = value; }
        }

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);

            string userLevel = level;
            switch (userLevel.ToUpper())
            {
                case "A":
                    correctAnswerString = aCorrectAnswerString;
                    break;
                case "B":
                    correctAnswerString = bCorrectAnswerString;
                    break;
                case "C":
                    correctAnswerString = cCorrectAnswerString;
                    break;
            }
            lineHolder = GameObject.FindGameObjectWithTag("ObjectHolder");
            InitGame();
        }

        void InitGame()
        {
            qSoundManager.PlayQuestionSound();
            patternLight = steps[currentStep].transform.FindChild("PatternEffect");
            ClearLineHolder();

            // Initialize dots array from dot GameObjects by "Dot" tag.
            ResetDots();

            // LineRenderer Initialize
            lineRenderer = GetComponent<LineRenderer>();
            InitializeLineRenderer();

            // Set lineNum to chance.
            chance = correctAnswerString[currentStep].Split(',').Length;
            lineNum = chance;

            // PrevDot is line's start position.
            prevDot = null;

            // Initialize
            oneTime = true;
            trying = false;
            LineList.Clear();

            // Initialize Graphs
            answer = new Graph(numOfDots);
            correctAnswer = new Graph(numOfDots, correctAnswerString[currentStep].Split(','));
        }

        void Update()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dotPos;

            // TODO: if needed, convert mouse button logic into touch input
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < numOfDots; i++)
                {
                    dotPos = dots[i].transform.position;
                    // oneTime: prevent enter this loop for many times.
                    if (isInDot == i && oneTime && !dots[i].Selected && lineNum > 0)
                    {
                        dots[i].SelectDot();
                        if (!lineRenderer.enabled) { lineRenderer.enabled = true; }
                        Vector3 startPos = new Vector3(dotPos.x, dotPos.y, 80);
                        lineRenderer.SetPosition(0, startPos);

                        // if already drawing
                        if (prevDot != null)
                        {
                            Line line = new Line(lineWidth, lineMaterial);
                            line.start = prevDot;
                            line.end = dots[i];
                            int start = line.start.DotId;
                            int end = line.end.DotId;
                            if (!answer.Elements[start][end])
                            {
                                LineList.AddLine(line, answer, correctAnswer);
                                lineNum--;
                            }
                        }
                        prevDot = dots[i];
                        oneTime = false;
                        trying = true;
                    }
                    if (trying && lineNum > 0)
                    {
                        Vector3 endPos = new Vector3(mousePos.x, mousePos.y, 80);
                        lineRenderer.SetPosition(1, endPos);
                    }
                    if (lineNum == 0)
                        lineRenderer.enabled = false;
                }
            }

            // Reset oneTime if mouse position is in dot boundary 
            if (isInDot < 0) { oneTime = true; }

            // Confirm answer and reset every states 
            if (Input.GetMouseButtonUp(0))
            {
                // If click out of dot-boundary, ConfirmAnswer won't be triggered 
                if (trying) { ConfirmAnswer(); }
                // Reset selected-cover
                foreach (LinePatternQDot dot in dots) { dot.ResetSelected(); }

                ResetLineList();
                InitializeLineRenderer();
                oneTime = true;
                trying = false;
                lineNum = chance - LineList.Count();
                answer.ResetGraph(correctAnswer);
            }
        }

        private void InitializeLineRenderer()
        {
            lineRenderer.material = lineMaterial;
            lineRenderer.numPositions = 2;
            for (int i = 0; i < 2; i++)
            {
                lineRenderer.SetPosition(i, Vector3.zero);
            }
            lineRenderer.enabled = false;
        }
        private bool isInBoundary(Vector2 v1, Vector2 v2)
        {
            v1 = new Vector2(v1.x, v1.y + yAdjustValue);
            return Vector2.Distance(v1, v2) < dotRadius;
        }

        void ConfirmAnswer()
        {
            if (correctAnswer.isEqual(answer))
            {
                StartCoroutine(lightUpAndBranchAnswer());
            }
            else
            {
                StartCoroutine(IncorrectAnswer());
                StartCoroutine(LineHide());
                //ResetIncorrect();
            }
        }

        IEnumerator LineHide()
        {
            lineHolder.SetActive(false);
            yield return new WaitForSeconds(incorrectAnimDuration);
            lineHolder.SetActive(true);
        }

        IEnumerator lightUpAndBranchAnswer()
        {
            patternLight.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            ClearLineHolder();
            yield return StartCoroutine(branchAnswer());
            if (currentStep <= lastStep)
            {
                ResetLineList();
                InitGame();
            }
        }

        private void ResetIncorrect()
        {
            InitializeLineRenderer();

            foreach (LinePatternQDot dot in dots)
            {
                dot.ResetSelected();
                dot.Fix = false;
            }

            foreach (Line line in LineList.Get())
            {
                Destroy(line.lineGameObject);
            }
            LineList.Clear();

            chance = correctAnswerString[currentStep].Split(',').Length;
            lineNum = chance;

            oneTime = true;
            trying = false;

            prevDot = null;

            answer = new Graph(numOfDots);
            correctAnswer = new Graph(numOfDots, correctAnswerString[currentStep].Split(','));


        }

        private void ResetDots()
        {
            GameObject[] dotGameObjects = GameObject.FindGameObjectsWithTag("Element");
            numOfDots = dotGameObjects.Length;
            dots = new LinePatternQDot[numOfDots];
            colliders = new CircleCollider2D[numOfDots];
            for (int i = 0; i < numOfDots; i++)
            {
                int index = GetDotIndex(dotGameObjects[i].name);
                dots[index] = dotGameObjects[i].GetComponent<LinePatternQDot>();
                colliders[index] = dotGameObjects[i].GetComponent<CircleCollider2D>();
                dots[index].SetManager(this);
            }
        }

        private void ResetLineList()
        {
            foreach (Line line in LineList.Get())
            {
                if (!line.IsAnswerLine)
                    Destroy(line.lineGameObject);
            }
            LineList.Get().RemoveAll(line => !line.IsAnswerLine);
            prevDot = null;
        }
        void ClearLineHolder()
        {
            foreach (Transform child in lineHolder.GetComponentsInChildren<Transform>())
            {
                child.gameObject.SetActive(false);
            }
            lineHolder.SetActive(true);
        }

        int GetDotIndex(string dotName)
        {
            string[] dotNameSplit = dotName.Split(' ');
            int ascii_num = dotNameSplit[1].ToCharArray()[0];
            int ret = 0;
            if (ascii_num > 64)
                ret = ascii_num - 55;
            else
                ret = int.Parse(dotNameSplit[1]);
            return ret;
        }
    }
}