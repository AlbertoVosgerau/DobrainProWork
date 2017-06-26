using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.UI
{
    public class UISceneContent : MonoBehaviour {

        public GameObject visibleToggleButton;

        public void SetVisibleToggleButtonActive(bool active)
        {
            visibleToggleButton.SetActive(active);
        }

    }
}
