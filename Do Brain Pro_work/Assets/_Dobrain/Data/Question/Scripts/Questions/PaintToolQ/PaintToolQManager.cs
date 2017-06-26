using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class PaintToolQManager : QuestionManager
    {
        public Camera screenShotCamera;
        public float StampScaleAmount = 11;
        public GameObject captureEffect;
        public float xBound;
        public float xAdjust;
        public float yAdjust;
        public float yBound;
        // public float yBoundMin = 2.7f;
        float erasorRadius = 0.35f;
        public Transform stampHolder;
        public Transform lineHolder;
        const float ERASOR_DEPTH = 150;
        enum COLOR { RED = 0, BLUE, GREEN, YELLOW }
        float brushWidth = 0.15f;
        float depth = 0.0f;
        float depthValue = 0.0001f;
        float ignoreNearestPointAmount = 0.01f;
        ObjectPooler stampPooler;
        ObjectPooler linePooler;
        List<Vector3> mousePositions;
        LineRenderer brushLine;
        Color brushColor = new Color(1f,0.4f,0.184f);
        bool isErasing = false;
        bool isMousePressed = false;
        bool isStampSelected = false;
        GameObject CaptureBtn;

        public override IEnumerator Initialize(int ch, int index, string level)
        {
            yield return base.Initialize(ch,index,level);

            linePooler = GetComponent<ObjectPooler>();
            mousePositions = new List<Vector3>();
            qSoundManager.PlayQuestionSound();
            CaptureBtn = GameObject.Find("CaptureAndSkipBtn");

            CaptureBtn.GetComponent<Animator>().enabled = false;
            yield return new WaitForSeconds(5f);
            CaptureBtn.GetComponent<Animator>().enabled = true;
        }

        void Update()
        {
            if (!isErasing && isMousePressed && !isStampSelected)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePositions.Find(vec => Vector2.Distance(vec, mousePos) < ignoreNearestPointAmount) == Vector3.zero)
                {
                    depth -= depthValue;
                    mousePos = new Vector3(Mathf.Clamp(mousePos.x, -xBound - xAdjust, xBound), Mathf.Clamp(mousePos.y, -yBound - yAdjust, yBound), depth);
                    mousePositions.Add(mousePos);
                    LerpLine(mousePositions);
                    InitBrushLine();
                    brushLine.numPositions = mousePositions.Count;
                    brushLine.SetPositions(mousePositions.ToArray());
                }
            }
        }
        void LerpLine(List<Vector3> DotList)
        {
            bool isLerp = true;
            int complete = 0;

            while (isLerp)
            {
                for (int i = 0; i < DotList.Count-1; i++)
                {
                    if (Vector3.Distance(mousePositions[i], mousePositions[i + 1]) > 0.1f)
                    {
                        DotList.Insert(i + 1, Vector3.Lerp(mousePositions[i], mousePositions[i + 1], 0.5f));
                        complete++;
                    }
                }

                if (complete == 0)
                    isLerp = false;   
                else
                    complete = 0;
            }
        }
        void InitBrushLine()
        {
            brushLine.startColor = brushColor;
            brushLine.endColor = brushColor;
            brushLine.startWidth = brushWidth;
            brushLine.endWidth = brushWidth;
        }


        public void ChangeColor(int colorInt)
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            isErasing = false;
            switch ((COLOR)colorInt)
            {
                case COLOR.RED:
                    brushColor = new Color(1f,0.4f,0.184f);
                    break;
                case COLOR.BLUE:
                    brushColor = new Color(0.1f, 0.56f, 1f);
                    break;
                case COLOR.GREEN:
                    brushColor = new Color(0f,0.76f, 0.037f);
                    break;
                case COLOR.YELLOW:
                    brushColor = new Color(1f, 0.77f, 0.074f);
                    break;
                default:
                    brushColor = new Color(1f, 0.4f, 0.184f);
                    break;
            }
            disableStamp();
        }
        public void EraserBtn()
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            isErasing = true;
            disableStamp();
        }

        public void ToggleStamp(ObjectPooler targetPooler)
        {
            qSoundManager.PlayEffectSound(qSoundManager.effectSounds[0]);
            isErasing = false;
            isMousePressed = false;
            if (stampPooler != null && (targetPooler.GetInstanceID() == stampPooler.GetInstanceID()))
            {
                disableStamp();
                return;
            }
            stampPooler = targetPooler;
            isStampSelected = true;
        }
        void disableStamp()
        {
            isStampSelected = false;
            stampPooler = null;
        }
        public void CaptureAndCorrectAnswer()
        {
//            string localTime = System.DateTime.Now.ToLocalTime().ToString().Replace('/', '_').Trim();
            string localTime = UnbiasedTime.Instance.Now().ToLocalTime().ToString().Replace('/', '_').Trim();
            localTime = localTime.Replace(' ', '_');
            localTime = localTime.Replace(':', '_');
            string screenShotFileName = localTime + ".png";
            string savePath;
            // TODO: dev code
            // if (ContentsManager.instance != null)
//            savePath =  ContentsManager.instance.screenShotPath;
            savePath = Application.persistentDataPath + "/screenshots";
            // else
            //     savePath = Path.Combine(Application.streamingAssetsPath, "screenshots");
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            savePath = Path.Combine(savePath, screenShotFileName);

            
            Texture2D screenShotTexture = new Texture2D
                (screenShotCamera.targetTexture.width, screenShotCamera.targetTexture.height, TextureFormat.ARGB32, true);
            screenShotCamera.Render();
            RenderTexture.active = screenShotCamera.targetTexture;
            screenShotTexture.ReadPixels(
                new Rect(0, 0, screenShotCamera.targetTexture.width, screenShotCamera.targetTexture.height), 0, 0);
            screenShotTexture.Apply();
            byte[] screenShotBytes = screenShotTexture.EncodeToPNG();
            File.WriteAllBytes(savePath, screenShotBytes);
            DestroyObject(screenShotTexture);
            

            captureEffect.SetActive(true);
            StartCoroutine(base.branchAnswer());
        }

        void OnMouseDown()
        {
            if (!isErasing)
            {
                if (isStampSelected)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    GameObject stamp = stampPooler.GetObject();
                    depth -= depthValue;
                    mousePos.z = depth;
                    stamp.transform.position = mousePos;
                    stamp.transform.localScale = new Vector2(StampScaleAmount, StampScaleAmount);
                }
                else
                {
                    GameObject brushLineObject = linePooler.GetObject();
                    brushLine = brushLineObject.GetComponent<LineRenderer>();
                    InitBrushLine();
                    isMousePressed = true;
                }
            }
        }
        void OnMouseDrag()
        {
            if (isErasing)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                foreach (LineRenderer line in lineHolder.GetComponentsInChildren<LineRenderer>())
                {
                    if (line.gameObject.activeInHierarchy)
                    {
                        for (int i = 0; i < line.numPositions; i++)
                        {
                            Vector2 pos = line.GetPosition(i);
                            if (Vector2.Distance(mousePos, pos) < erasorRadius)
                                line.SetPosition(i, new Vector3(pos.x, pos.y, ERASOR_DEPTH));
                        }
                    }
                }

                foreach (Transform stampObject in stampHolder.GetComponentsInChildren<Transform>())
                {
                    if (stampObject.gameObject.activeInHierarchy)
                    {
                        Vector2 pos = stampObject.position;
                        if (Vector2.Distance(mousePos, pos) < erasorRadius)
                            stampObject.position = new Vector3(pos.x, pos.y, ERASOR_DEPTH);
                    }
                }
            }
        }
        void OnMouseUp()
        {
            if (!isErasing)
            {
                isMousePressed = false;
            }
            if(!isStampSelected && !isErasing)
            {
                brushLine.numPositions = mousePositions.Count + 1;
                brushLine.SetPosition(mousePositions.Count, new Vector3(mousePositions[mousePositions.Count - 1].x, mousePositions[mousePositions.Count - 1].y, ERASOR_DEPTH));
                mousePositions.Clear();
            }
        }
    }
}
