using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

using Com.Dobrain.Dobrainproject.Manager;


public class TestSynchronization : MonoBehaviour {

    public InputField userIdInputField;
    public Text logView;
    public SynchronizationManager synchronizationManager;
    public LMSManager lmsManager;

    string userId;
    DataSnapshot userData;

    public void Authenticate()
    {
        DebugLog("Authenticate User-ID");
        synchronizationManager.OnCompleteAuthenticateUserID += OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID += OnFailAuthenticateUserID;
        synchronizationManager.AuthenticateUserID();
    }

    void OnCompleteAuthenticateUserID (string userId)
    {
        DebugLog("Complete Authenticate");
        DebugLog("User ID : " + userId);

        this.userId = userId;

        synchronizationManager.OnCompleteAuthenticateUserID -= OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID -= OnFailAuthenticateUserID;
    }

    void OnFailAuthenticateUserID ()
    {
        DebugLog("Fail Authenticate");

        synchronizationManager.OnCompleteAuthenticateUserID -= OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID -= OnFailAuthenticateUserID;
    }

    public void SaveData()
    {
        DebugLog("Save Data");

        if(!string.IsNullOrEmpty(userIdInputField.text))
            userId = userIdInputField.text;

        synchronizationManager.SaveData(userId, UnbiasedTime.Instance.Now(), lmsManager.GetStep(), lmsManager.GetContentDatas());
    }

    public void LoadData()
    {
        synchronizationManager.OnCompleteLoadData += OnCompleteLoadData;
        synchronizationManager.OnFailLoadData += OnFailLoadData;
        synchronizationManager.LoadData(userId);
    }

    void OnCompleteLoadData (DataSnapshot snapshot)
    {
        DebugLog("Complete Load Data");

        synchronizationManager.OnCompleteLoadData -= OnCompleteLoadData;
        synchronizationManager.OnFailLoadData -= OnFailLoadData;

        userData = snapshot;
    }

    void OnFailLoadData()
    {
        DebugLog("Fail Load Data");

        synchronizationManager.OnCompleteLoadData -= OnCompleteLoadData;
        synchronizationManager.OnFailLoadData -= OnFailLoadData;
    }

    public void SetData()
    {
        DebugLog("Set Data");

        string dateStr = userData.Child("date").Value.ToString();
        DateTime date = Convert.ToDateTime(dateStr);
//        synchronizationManager.SetLocalLatestDate(date);

        Dictionary<string, object> step = (Dictionary<string, object>)userData.Child("step").Value;
        DebugLog("inited : " + step["inited"].ToString());
        DebugLog("profileInited : " + step["profileInited"].ToString());
        DebugLog("profileName : " + step["profileName"].ToString());
        DebugLog("profileLevel : " + step["inited"].ToString());
        DebugLog("startDate : " + step["startDate"].ToString());
        DebugLog("currentContentNo : " + step["currentContentNo"].ToString());
        DebugLog("lastContentNo : " + step["lastContentNo"].ToString());
        DebugLog("lastWeekdayContentNo : " + step["lastWeekdayContentNo"].ToString());
        DebugLog("lastWeekendContentNo : " + step["lastWeekendContentNo"].ToString());
        DebugLog("contentLimitNum : " + step["contentLimitNum"].ToString());
//        lmsManager.SetStep(step);

        List<Dictionary<string, object>> contents = new List<Dictionary<string, object>>();
        for(int i = 0 ; i < userData.Child("contents").ChildrenCount ; i++)
        {
            Dictionary<string, object> content = (Dictionary<string, object>)userData.Child("contents").Child(i.ToString()).Value;
            DebugLog("Content : " + content["date"].ToString());
            contents.Add(content);
        }
//        lmsManager.SetContentDatas(contents);
    }

    void DebugLog(string log)
    {
        logView.text += log + "\n\n";
    }

    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

}
