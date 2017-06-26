using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Dobrain.Dobrainproject.UI.Content
{
    public class UIContentNavigationPanel : MonoBehaviour {

        public enum ButtonType
        {
            Home, Prev, Next
        }

        public Button homeButton;
        public Button prevButton;
        public Button nextButton;

        [HideInInspector]
        bool visible;

        Animator animator;


        void Awake()
        {
            animator = GetComponent<Animator>();
            visible = false;
        }

        public void SetButtonInteractable(ButtonType buttonType, bool interactable)
        {
            switch(buttonType)
            {
                case ButtonType.Home:
                    homeButton.interactable = interactable;
                    break;
                case ButtonType.Prev:
                    prevButton.interactable = interactable;
                    break;
                case ButtonType.Next:
                    nextButton.interactable = interactable;
                    break;
            }
        }

        public void ToggleVisible()
        {
            Show(!visible);
        }

        public void Show(bool visible)
        {
            if(visible)
                animator.Play("Show");
            else
                animator.Play("Hide");

            this.visible = visible;
        }

        public void SetWeekendMode()
        {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(115, homeButton.GetComponent<RectTransform>().anchoredPosition.y);
        }


    }
}
