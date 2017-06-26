using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.UI
{
    public class UIContentPanel : MonoBehaviour {

        public delegate void EventHandler(UIContentPanel contentPanel, int selectedContentButtonIndex);
        public event EventHandler OnSelectContentButton;

        public UIScrollRect scrollRect;
        public Transform background;
        public UIContentButton buttonPrefab;
        public List<Transform> buttonPositionList;
        public Animator[] contentSelectAnimatorList;

        int index;

        public Transform buttonParent;
        float buttonParentStartX;
        List<UIContentButton> buttonList = new List<UIContentButton>();



        void Update()
        {
            if(buttonParent != null)
            {
                float normalizedPosition = ((float)index + 1f) * scrollRect.normalizedPosition.x;
                float x = buttonParentStartX + (normalizedPosition * -2000f);
                float y = buttonParent.localPosition.y;
                buttonParent.localPosition = new Vector2(x, y);
            }
        }

        public void Init(int index)
        {
            this.index = index;

            buttonParentStartX = buttonParent.localPosition.x;

            buttonPrefab.gameObject.SetActive(false);
            for(int i = 0 ; i < buttonPositionList.Count ; i++)
            {
                UIContentButton button = Instantiate(buttonPrefab);
                button.name = "Button (" + i.ToString() + ")";
                button.transform.SetParent(buttonParent);
                button.transform.localPosition = buttonPositionList[i].localPosition;
                button.transform.localScale = Vector2.one;
                button.gameObject.SetActive(true);
                button.SetLoaingProgress(0f);

                // Test
//                button.SetNo(i + 1);

                buttonList.Add(button);
                buttonPositionList[i].gameObject.SetActive(false);
            }
        }

        public UIContentButton GetButton(int index)
        {
            return buttonList[index];
        }

        public List<UIContentButton> GetButtonList()
        {
            return buttonList;
        }

        public void SetContentButtonEventHendler(UIContentButton contentButton)
        {
            contentButton.button.onClick.AddListener(() => {
                int selectedIndex = buttonList.IndexOf(contentButton);
                if(OnSelectContentButton != null)
                    OnSelectContentButton(this, selectedIndex);
            });
        }

        public void PlayAnimation()
        {
            for(int i = 0 ; i < contentSelectAnimatorList.Length ; i++)
                contentSelectAnimatorList[i].Play("Play");
        }

        public void StopAnimation()
        {
            for(int i = 0 ; i < contentSelectAnimatorList.Length ; i++)
                contentSelectAnimatorList[i].Play("Stop");
        }

    }
}
