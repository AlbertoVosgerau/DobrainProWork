using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Com.Dobrain.Dobrainproject.UI.Home;

namespace Com.Dobrain.Dobrainproject.UI
{
    public class UIHomeScene : MonoBehaviour {

        public static UIHomeScene instance;

        public delegate void SelectContentEventHandler(int selectedIndex);
        public event SelectContentEventHandler OnSelectContent;
        public delegate void EventHandler();
        public event EventHandler OnParentsButtonClick;


        public GraphicRaycaster graphicRaycaster;
        public UIProfilePopup profilePopup;
        public UINewContentNotificationPopup ncnPopup;
        public ScrollRect scrollRect;
        public UIContentPanel contentPanel;
        public Button parentsButton;

        UIContentButton loadingProgressContentButton;

        public int contentNumPerPanel{
            get { return 30; }
        }

        List<UIContentPanel> contentPanelList = new List<UIContentPanel>();

        void Awake()
        {
            instance = this;

            contentPanel.gameObject.SetActive(false);
            profilePopup.gameObject.SetActive(false);
            ncnPopup.gameObject.SetActive(false);
            parentsButton.onClick.AddListener(() => {
                if(OnParentsButtonClick != null)
                    OnParentsButtonClick();
            });
        }

        public void SetInteractive(bool interactive)
        {
            graphicRaycaster.enabled = interactive;
        }

        public void SeekScroll(int currentContentNo)
        {
            int lastContentNo = (contentPanelList.Count * 30) - 1;
            currentContentNo -= 1;
            float seek = (float)currentContentNo / (float)lastContentNo;
            scrollRect.normalizedPosition = new Vector2(seek, scrollRect.normalizedPosition.y);
        }

        public void SetLoadingProgress(int selectedContentNo, float progress)
        {
            if(loadingProgressContentButton == null)
            {
                int contentPanelIndex = selectedContentNo / contentNumPerPanel;
                UIContentPanel contentPanel = contentPanelList[contentPanelIndex];

                int contentButtonIndex = (selectedContentNo % contentNumPerPanel) - 1;

                loadingProgressContentButton = contentPanel.GetButtonList()[contentButtonIndex];
            }

            loadingProgressContentButton.SetLoaingProgress(progress);
        }

        public UIContentButton GetContentButton(int contentNo)
        {
            int contentPanelIndex = contentNo / contentNumPerPanel;
            UIContentPanel contentPanel = contentPanelList[contentPanelIndex];

            int contentButtonIndex = (contentNo % contentNumPerPanel) - 1;

            return contentPanel.GetButtonList()[contentButtonIndex];
        }

        public UIContentPanel AddContentPanel()
        {
            UIContentPanel newContentPanel = Instantiate(contentPanel) as UIContentPanel;
            newContentPanel.name = "Content Button Panel (" + contentPanelList.Count.ToString() + ")";
            newContentPanel.GetComponent<RectTransform>().SetParent(scrollRect.content);
            newContentPanel.transform.localScale = Vector2.one;
            newContentPanel.gameObject.SetActive(true);
            newContentPanel.Init(contentPanelList.Count);
            newContentPanel.OnSelectContentButton += ContentPanel_OnSelectContentButton;
            contentPanelList.Add(newContentPanel);
            return newContentPanel;
        }

        public void PlayContentSelectAnimation(int selectedContentNo)
        {
            int contentPanelIndex = selectedContentNo / contentNumPerPanel;
            UIContentPanel contentPanel = contentPanelList[contentPanelIndex];
            contentPanel.PlayAnimation();
        }

        public void StopContentSelectAnimation(int selectedContentNo)
        {
            int contentPanelIndex = selectedContentNo / contentNumPerPanel;
            UIContentPanel contentPanel = contentPanelList[contentPanelIndex];
            contentPanel.StopAnimation();
        }

        public void ShowProfilePopup(bool visible)
        {
            if(!profilePopup.gameObject.activeSelf)
                profilePopup.gameObject.SetActive(true);
            
            profilePopup.Show(visible);
        }

        public void ShowNewContentNotification(bool visible)
        {
            if(!ncnPopup.gameObject.activeSelf)
                ncnPopup.gameObject.SetActive(true);

            ncnPopup.Show(visible);
        }

        public void ShowToast(string message)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidNotificationManager.Instance.ShowToastNotification(message);
            #endif
        }

        void ContentPanel_OnSelectContentButton (UIContentPanel contentPanel, int selectedContentButtonIndex)
        {
            int contentIndex = (contentPanelList.IndexOf(contentPanel) * contentNumPerPanel) + selectedContentButtonIndex;
            if(OnSelectContent != null)
                OnSelectContent(contentIndex);
        }



    }
}
