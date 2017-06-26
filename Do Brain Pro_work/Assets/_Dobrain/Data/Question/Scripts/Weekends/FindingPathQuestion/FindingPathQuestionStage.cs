using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Dobrain.contents.weekend
{
    public class FindingPathQuestionStage : MonoBehaviour
    {
        public iTweenPath path;
        public List<RectTransform> tileList;

        List<int> tileDirectionList;
        bool isCompleted;
        float count;

        public IEnumerator Excute()
        {   
            tileDirectionList = new List<int>();

            for(int i = 0 ; i < tileList.Count ; i++)
            {
                float angle = tileList[i].localRotation.eulerAngles.z;
                int direction = AngleToDirection(angle);
                tileDirectionList.Add(direction);

                Button tileButton = tileList[i].gameObject.AddComponent<Button>();

                tileButton.transition = Selectable.Transition.None;
                tileButton.onClick.AddListener(() => {
                    int tileIndex = tileList.IndexOf(tileButton.GetComponent<RectTransform>());
                    RectTransform tile = tileList[tileIndex];
                    Vector3 newTileEA = tile.localRotation.eulerAngles;
                    newTileEA.z -= 90f;
                    tile.localRotation = Quaternion.Euler(newTileEA);

                    tileDirectionList[tileIndex] = AngleToDirection(newTileEA.z);

                    int tileDirection = tileDirectionList[tileIndex];
                    isCompleted = true;
                    for(int answer_i = 0 ; answer_i < tileList.Count ; answer_i++)
                    {
                        if(!tileDirectionList[answer_i].Equals(0))
                            isCompleted = false;
                    }

                });
            }

            while(!isCompleted)
                yield return null;

            for(int i = 0 ; i < tileList.Count ; i++)
            {
                Button tileButton = tileList[i].GetComponent<Button>();
                Destroy(tileButton);
            }
        }

        public void PlayFinishAnimation(GameObject unit)
        {
            iTween.MoveTo(unit, iTween.Hash("path", iTweenPath.GetPath(path.pathName), "time", 7, "easeType", iTween.EaseType.linear));
        }

        int AngleToDirection(float angle)
        {
            int direction = 0;
            if(angle == 0f)
                direction = 0;    
            else if(angle == 270f)
                direction = 1;
            else if(angle == 180f)
                direction = 2;
            else if(angle == 90f)
                direction = 3;

            return direction;
        }


    }

}
