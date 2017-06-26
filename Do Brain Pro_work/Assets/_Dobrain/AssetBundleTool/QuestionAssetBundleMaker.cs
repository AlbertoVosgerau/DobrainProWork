using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Coolchoon.AssetBundleMaker
{
    public class QuestionAssetBundleMaker : MonoBehaviour {
        
        public enum Week
        {
            weekday, weekend
        }

        public int uno;
        public Week week;
        public List<GameObject> assets;
        public GameObject go;

    }
}
