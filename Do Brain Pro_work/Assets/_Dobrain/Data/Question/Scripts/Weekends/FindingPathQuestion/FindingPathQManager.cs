using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Dobrain.contents.weekend
{
    public class FindingPathQManager : MonoBehaviour
    {
        public Text timeView;
        public RectTransform unit;

        public List<FindingPathQuestionStage> levelAStageList;
        public List<FindingPathQuestionStage> levelBStageList;
        public List<FindingPathQuestionStage> levelCStageList;

        public string level = "A";

        Vector2 unitStartPosition;
        List<FindingPathQuestionStage> currentStageList;
        int stageCount;

        IEnumerator Start()
        {
            unitStartPosition = unit.anchoredPosition;

            switch(level)
            {
                case "A":
                    currentStageList = levelAStageList;
                    break;
                case "B":
                    currentStageList = levelBStageList;
                    break;
                case "C":
                    currentStageList = levelCStageList;
                    break;
            }

            for(int i = 0 ; i < currentStageList.Count ; i++)
                currentStageList[i].gameObject.SetActive(false);

            for(int i = 0 ; i < currentStageList.Count ; i++)
            {
                yield return StartCoroutine("ProcessStage");

                stageCount++;
            }
        }

        IEnumerator ProcessStage()
        {
            FindingPathQuestionStage stage = currentStageList[stageCount];
            stage.gameObject.SetActive(true);

            unit.anchoredPosition = unitStartPosition;

            StartCoroutine("DisplayTimeRoutine");
            yield return StartCoroutine(stage.Excute());
            StopCoroutine("DisplayTimeRoutine");
            stage.PlayFinishAnimation(unit.gameObject);
            yield return new WaitForSeconds(8f);
            stage.gameObject.SetActive(false);
        }

        IEnumerator DisplayTimeRoutine()
        {
            float count = 0f;

            while(true)
            {
                count += Time.deltaTime;
                DisplayTime(count);
                yield return null;
            }
        }

        void DisplayTime(float time)
        {
            string timeStr = Mathf.Round(time).ToString();
            timeView.text = "Time : " + timeStr;
        }



    }

}
