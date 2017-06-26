using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.UI.Home
{
    public class UIProfilePopup : MonoBehaviour 
    {
        public delegate void EventHandler(string name, int selectedLevel);
        public event EventHandler OnSubmit;
        public delegate void EventCloseHandler();
        public event EventCloseHandler OnClose;

        public Animator animator;
        public GameObject panel;
        public InputField nameInputField;
        public Toggle[] levelToggleList;
        public Button submitButton;
        public Button closeButton;

        int selectedLevel = -1;

        void Awake()
        {
            // Set Level-toggle-group
            for(int i = 0 ; i < levelToggleList.Length ; i++)
            {
                Toggle toggleButton = levelToggleList[i];
                Outline outline = toggleButton.GetComponentInChildren<Outline>();
                outline.enabled = false;

                int index = i;
                toggleButton.onValueChanged.AddListener((bool toggle) => {
                    outline.enabled = toggle;
                    if(toggle)
                        selectedLevel = index;
                });
            }

            // Set Submit-button
            submitButton.onClick.AddListener(SubmitButton_OnClick);

            // Set Close-button
            closeButton.onClick.AddListener(() => {
                if(OnClose != null)
                    OnClose();
            });
    	}

        void SubmitButton_OnClick()
        {
            if(OnSubmit != null)
                OnSubmit(nameInputField.text, selectedLevel);
        }

        public void Init()
        {
            nameInputField.text = "";

            levelToggleList[0].group.SetAllTogglesOff();
            for(int i = 0 ; i < levelToggleList.Length ; i++)
                levelToggleList[i].GetComponentInChildren<Outline>().enabled = false;
            selectedLevel = -1;
        }

        public void Show(bool visible)
        {
            if(visible)
                animator.Play("Show");
            else
                animator.Play("Hide");
        }



        public void SetProfile(string name, int level)
        {
            nameInputField.text = name;

            Toggle toggleButton = levelToggleList[level];
            toggleButton.isOn = true;
            toggleButton.GetComponentInChildren<Outline>().enabled = true;
            
            selectedLevel = level;
        }

    }
}
