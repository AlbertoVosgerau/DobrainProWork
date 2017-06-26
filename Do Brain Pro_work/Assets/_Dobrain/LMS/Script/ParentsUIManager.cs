using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;

using Com.Dobrain.Dobrainproject.Manager;

namespace Dobrain.LMS
{

    public class ParentsUIManager : MonoBehaviour
    {
        Analysis analysis;
        AudioSource audioSource;

        public Text childNameTitleText;
        public Text childNameTitleText2;

        public Text progressText;
        public Text bestTypeText;

        public Text todayPercentText;
        public Text todayMissionText;
        public Image todaySlider;
        public Text weekPercentText;
        public Text weekMissionText;
        public Image weekSlider;
        public Text monthPercetText;
        public Text monthMissionText;
        public Image monthSlider;
        public Text best1Text;
        public Image best1Slider;
        public Text best2Text;
        public Image best2Slider;
        public Text best3Text;
        public Image best3Slider;
        public Text repeatText;
        public Text repeatText2;

        public AudioClip buttonSound;

        void Start()
        {
            analysis = GetComponent<Analysis>();
            audioSource = GetComponent<AudioSource>();
            changeChildeNameTitleText(LMSManager.instance.profileName);
            SetContentText();
        }

        void SetContentText()
        {
            progressText.text = (Math.Truncate((float)analysis.weekcount / analysis.weekmax * 100) <100 ? Math.Truncate((float)analysis.weekcount / analysis.weekmax * 100) : 100)+ "%";
            bestTypeText.text = analysis.thisWeekBest[0].Type;

            todayMissionText.text = "" + analysis.todaytotalcount;
            weekMissionText.text = "" + analysis.weektotalcount;
            monthMissionText.text = "" + analysis.monthtotalcount;

            todayPercentText.text = (Math.Truncate(((float)analysis.todaycount / analysis.todaymax * 100)) < 100 ? Math.Truncate(((float)analysis.todaycount / analysis.todaymax * 100)):100) + "%";
            weekPercentText.text = (Math.Truncate(((float)analysis.weekcount / analysis.weekmax * 100)) < 100 ? Math.Truncate(((float)analysis.weekcount / analysis.weekmax * 100)) : 100) + "%";
            monthPercetText.text = (Math.Truncate(((float)analysis.monthcount / analysis.monthmax * 100))<100 ?Math.Truncate(((float)analysis.monthcount / analysis.monthmax * 100)):100) + "%";

            todaySlider.fillAmount = ((float)analysis.todaycount / analysis.todaymax);
            weekSlider.fillAmount = ((float)analysis.weekcount / analysis.weekmax);
            monthSlider.fillAmount = ((float)analysis.monthcount / analysis.monthmax);

            best1Text.text = analysis.thisWeekBest[0].Type;
            best2Text.text = analysis.thisWeekBest[1].Type;
            best3Text.text = analysis.thisWeekBest[2].Type;

            best1Slider.fillAmount = 1;
            best2Slider.fillAmount = ((analysis.thisWeekBest[1].avg / analysis.thisWeekBest[0].avg) > 0.3f) ? analysis.thisWeekBest[1].avg / analysis.thisWeekBest[0].avg : 0.3f;
            best3Slider.fillAmount = ((analysis.thisWeekBest[2].avg / analysis.thisWeekBest[0].avg) > 0.3f) ? analysis.thisWeekBest[2].avg / analysis.thisWeekBest[0].avg : 0.3f;

            repeatText.text = ((Math.Truncate(analysis.thisweekRepeat * 100) < 100 )? (Math.Truncate(analysis.thisweekRepeat * 100)) : 100) + "%";
            repeatText2.text = "복습률 : " + ( Math.Truncate(analysis.thisweekRepeat * 100) <100 ?Math.Truncate(analysis.thisweekRepeat * 100):100) + "%";
        }

        public void changeChildeNameTitleText(string name)
        {
            childNameTitleText.text = name + "의 \n창의력갤러리";
            childNameTitleText2.text = name + "의 \n창의력갤러리";
        }

        public void ButtonClick()
        {
            audioSource.clip = buttonSound;
            audioSource.Play();
        }

        public void LoadHomeScene()
        {
            SceneManager.LoadScene("Home");
        }
    }
}