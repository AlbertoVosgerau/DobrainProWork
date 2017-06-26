using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Com.Dobrain.Dobrainproject.Manager
{
    public class NotificationManager : MonoBehaviour {

        public delegate void EventHandler();
        public event EventHandler OnShowToastComplete;
 
        public Texture2D localNotificationBigPicture;

        string title = "선물이 도착했습니다!";
        string message;

        public void SetNewContentNotification()
        {
            int inited = PlayerPrefs.GetInt("notification_new_content_inited");
            if(inited == 1)
                return;


            #if UNITY_IOS && !UNITY_EDITOR
            UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert| UnityEngine.iOS.NotificationType.Badge |  UnityEngine.iOS.NotificationType.Sound);
            #endif


            int hour = 0;

            for(int i = 0 ; i < 3 ; i++)
            {
                if(i < 2)
                {
                    message = "화, 목엔 똑똑해지는 마법동화가 열려요!";
                    hour = 16;
                }
                else
                {
                    message = "주말엔 똑똑해지는 마법게임이 열려요!";
                    hour = 12;
                }

//                DateTime now = DateTime.Now;
//                DateTime after = DateTime.Now;
                DateTime now = UnbiasedTime.Instance.Now();
                DateTime after = UnbiasedTime.Instance.Now();
                if(i == 0)
                {
                    switch(now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            after = now.AddDays(1);
                            break;
                        case DayOfWeek.Tuesday:
                            after = now.AddDays(7);
                            break;
                        case DayOfWeek.Wednesday:
                            after = now.AddDays(6);
                            break;
                        case DayOfWeek.Thursday:
                            after = now.AddDays(5);
                            break;
                        case DayOfWeek.Friday:
                            after = now.AddDays(4);
                            break;
                        case DayOfWeek.Saturday:
                            after = now.AddDays(3);
                            break;
                        case DayOfWeek.Sunday:
                            after = now.AddDays(2);
                            break;
                    }
                }
                else if(i == 1)
                {
                    switch(now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            after = now.AddDays(3);
                            break;
                        case DayOfWeek.Tuesday:
                            after = now.AddDays(2);
                            break;
                        case DayOfWeek.Wednesday:
                            after = now.AddDays(1);
                            break;
                        case DayOfWeek.Thursday:
                            after = now.AddDays(7);
                            break;
                        case DayOfWeek.Friday:
                            after = now.AddDays(6);
                            break;
                        case DayOfWeek.Saturday:
                            after = now.AddDays(5);
                            break;
                        case DayOfWeek.Sunday:
                            after = now.AddDays(4);
                            break;
                    }
                }
                else if(i == 2)
                {
                    switch(now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            after = now.AddDays(5);
                            break;
                        case DayOfWeek.Tuesday:
                            after = now.AddDays(4);
                            break;
                        case DayOfWeek.Wednesday:
                            after = now.AddDays(3);
                            break;
                        case DayOfWeek.Thursday:
                            after = now.AddDays(2);
                            break;
                        case DayOfWeek.Friday:
                            after = now.AddDays(1);
                            break;
                        case DayOfWeek.Saturday:
                            after = now.AddDays(7);
                            break;
                        case DayOfWeek.Sunday:
                            after = now.AddDays(6);
                            break;
                    }
                }
                after = new DateTime(after.Year, after.Month, after.Day, hour, 0, 0);
                TimeSpan timeSpan = after - now;
                int time = (int)timeSpan.TotalSeconds;


                #if UNITY_ANDROID && !UNITY_EDITOR
                SetNewContentNotificationForAOS(i, title, message, time);
                #elif UNITY_IOS && !UNITY_EDITOR
                SetNewContentNotificationForiOS(i.ToString(), title, message, time);
                #endif
            }

            PlayerPrefs.SetInt("notification_new_content_inited", 1);
        }

        public void ShowToast(string message, int duration = 2)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidNotificationManager.Instance.ShowToastNotification(message, duration);
            if(OnShowToastComplete != null)
                OnShowToastComplete();
            #elif UNITY_IOS && !UNITY_EDITOR
            IOSMessage iOSMessage = IOSMessage.Create("", message);
            iOSMessage.OnComplete += delegate()
            {  
                if(OnShowToastComplete != null)
                    OnShowToastComplete();
            };
            #else
            Debug.Log("Show Toast : " + message);
            if(OnShowToastComplete != null)
                OnShowToastComplete();
            #endif
        }

        void SetNewContentNotificationForAOS(int id, string title, string message, int time)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidNotificationBuilder builder = new AndroidNotificationBuilder(id, title, message, time);

            TimeSpan weekTime = DateTime.Now.AddDays(7) - DateTime.Now;
            builder.SetRepeatDelay((int)weekTime.TotalSeconds);

            builder.SetRepeating(true);
            AndroidNotificationManager.Instance.ScheduleLocalNotification(builder);
            #endif
        }


        void SetNewContentNotificationForiOS(string id, string title, string message, int time)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            UnityEngine.iOS.LocalNotification noti = new UnityEngine.iOS.LocalNotification();
            IDictionary userInfo = new Dictionary<string, string>();
            userInfo["id"] = id;
            noti.userInfo = userInfo;
            noti.alertAction = title;
            noti.alertBody = message;
            noti.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
            noti.applicationIconBadgeNumber = 0;
            noti.fireDate = DateTime.Now.AddSeconds(time);
            noti.repeatInterval = UnityEngine.iOS.CalendarUnit.Week;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(noti);
            #endif
        }

       
    }
}
