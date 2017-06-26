using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

namespace Com.Dobrain.Dobrainproject.Manager
{
    public class SynchronizationManager : MonoBehaviour {

        public delegate void EventHandlerCompleteAuthenticateUserID(string userId);
        public event EventHandlerCompleteAuthenticateUserID OnCompleteAuthenticateUserID;
        public delegate void EventHandlerFailAuthenticateUserID();
        public event EventHandlerFailAuthenticateUserID OnFailAuthenticateUserID;

        public delegate void EventHandlerCompleteLoadData(DataSnapshot snapshot);
        public event EventHandlerCompleteLoadData OnCompleteLoadData;
        public delegate void EventHandlerFailLoadData();
        public event EventHandlerFailLoadData OnFailLoadData;

        public delegate void EventHandlerCompleteLoadLatestDate(DateTime date);
        public event EventHandlerCompleteLoadLatestDate OnCompleteLoadLatestDate;
        public delegate void EventHandlerFailLoadLatestDate();
        public event EventHandlerFailLoadLatestDate OnFailLoadLatestDate;


        DatabaseReference reference;

        void Awake()
        {
            // Set up the Editor before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dobrain-pro.firebaseio.com/");

            // Get the root reference location of the database.
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void AuthenticateUserID()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            Social.localUser.Authenticate(success => {
                if (success)
                {
                    if(OnCompleteAuthenticateUserID != null)
                        OnCompleteAuthenticateUserID(Social.localUser.id);
                }
                else
                {
                    if(OnFailAuthenticateUserID != null)
                        OnFailAuthenticateUserID();
                }
            });
            #elif UNITY_ANDROID && !UNITY_EDITOR
            if(OnFailAuthenticateUserID != null)
                OnFailAuthenticateUserID();
            #endif
        }

        public void LoadData(string userId)
        {
            FirebaseDatabase.DefaultInstance
                .GetReference("users").Child(userId)
                .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                        if(OnFailLoadData != null)
                            OnFailLoadData();
                    }
                    else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        if(snapshot.Value == null)
                        {
                            if(OnFailLoadData != null)
                                OnFailLoadData();
                        }
                        else
                        {
                            if(OnCompleteLoadData != null)
                                OnCompleteLoadData(snapshot);
                        }
                    }
                });
        }

        public void SaveData(string userId, DateTime date, Dictionary<string, object> step, List<Dictionary<string, object>> contents)
        {
            reference.Child("users").Child(userId).Child("date").SetValueAsync(date.ToString());
            reference.Child("users").Child(userId).Child("step").SetValueAsync(step);
            for(int i = 0 ; i < contents.Count ; i++)
                reference.Child("users").Child(userId).Child("contents").Child(i.ToString()).SetValueAsync(contents[i]);
        }

        public DateTime GetLocalLatestDate()
        {
            string dateStr = PlayerPrefs.GetString("synchronization_latest_date");
            if(!string.IsNullOrEmpty(dateStr))
                return Convert.ToDateTime(dateStr);

            return new DateTime();
        }

        public void SetLocalLatestDate(DateTime date)
        {
            PlayerPrefs.SetString("synchronization_latest_date", date.ToString());
        }

        public void LoadLatestDate(string userId)
        {
            FirebaseDatabase.DefaultInstance
                .GetReference("users").Child(userId).Child("date")
                .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                        Debug.Log("fail");
                        if(OnFailLoadLatestDate != null)
                            OnFailLoadLatestDate();
                    }
                    else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        string latestDateStr = snapshot.Value.ToString();
                        if(string.IsNullOrEmpty(latestDateStr))
                        {
                            if(OnFailLoadLatestDate != null)
                                OnFailLoadLatestDate();
                        }
                        else
                        {
                            DateTime latestDate = Convert.ToDateTime(latestDateStr);
                            if(OnCompleteLoadLatestDate != null)
                                OnCompleteLoadLatestDate(latestDate);
                        }
                    }
                });
        }



    }
}
