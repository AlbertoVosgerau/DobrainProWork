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


public class SceneHome : MonoBehaviour {
    
    public SynchronizationManager synchronizationManager;
    public LMSManager lmsManager;

    string userId;
    DataSnapshot userData;
    public bool failed;
    public bool completed;

    void Awake()
    {
        failed = false;
        completed = false;
    }

    public void Authenticate()
    {
        Debug.Log("Authenticate User-ID");
        synchronizationManager.OnCompleteAuthenticateUserID += OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID += OnFailAuthenticateUserID;
        synchronizationManager.AuthenticateUserID();
    }

    void OnCompleteAuthenticateUserID (string userId)
    {
        Debug.Log("Complete Authenticate");
        Debug.Log("User ID : " + userId);

        this.userId = userId;

        synchronizationManager.OnCompleteAuthenticateUserID -= OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID -= OnFailAuthenticateUserID;

        //====================
        LoadLatestDate();
    }

    void OnFailAuthenticateUserID ()
    {
        Debug.Log("Fail Authenticate");

        synchronizationManager.OnCompleteAuthenticateUserID -= OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID -= OnFailAuthenticateUserID;

        failed = true;
    }

    void LoadLatestDate()
    {
        Debug.Log("Load Latest-Date");

        DateTime localDate = synchronizationManager.GetLocalLatestDate();
        DateTime nullDate = new DateTime();
        if(localDate.Equals(nullDate))
        {
            Debug.Log("Local Lastest-date is null");
            LoadData();
        }
        else
        {
            Debug.Log("Local Lastest-date exist");
            synchronizationManager.OnCompleteLoadLatestDate += OnCompleteLoadLatestDate;
            synchronizationManager.OnFailLoadLatestDate += OnFailLoadLatestDate;
            synchronizationManager.LoadLatestDate(userId);
        }
    }

    void OnCompleteLoadLatestDate (DateTime date)
    {
        Debug.Log("Complete Load Latest-Date");

        synchronizationManager.OnCompleteLoadLatestDate -= OnCompleteLoadLatestDate;
        synchronizationManager.OnFailLoadLatestDate -= OnFailLoadLatestDate;

        DateTime localDate = synchronizationManager.GetLocalLatestDate();
        if(localDate < date)
            LoadData();
        else
        {
            Debug.Log("Local Date is same Server Date");
            failed = true;
        }
    }

    void OnFailLoadLatestDate ()
    {
        Debug.Log("Fail Load Latest-Date");

        synchronizationManager.OnCompleteLoadLatestDate -= OnCompleteLoadLatestDate;
        synchronizationManager.OnFailLoadLatestDate -= OnFailLoadLatestDate;

        failed = true;
    }

    void LoadData()
    {
        Debug.Log("Load Data");

        synchronizationManager.OnCompleteLoadData += OnCompleteLoadData;
        synchronizationManager.OnFailLoadData += OnFailLoadData;
        synchronizationManager.LoadData(userId);
    }

    void OnCompleteLoadData (DataSnapshot snapshot)
    {
        Debug.Log("Complete Load Data");

        synchronizationManager.OnCompleteLoadData -= OnCompleteLoadData;
        synchronizationManager.OnFailLoadData -= OnFailLoadData;

        userData = snapshot;

        //====================
        SetData();
    }

    void OnFailLoadData()
    {
        Debug.Log("Fail Load Data");

        synchronizationManager.OnCompleteLoadData -= OnCompleteLoadData;
        synchronizationManager.OnFailLoadData -= OnFailLoadData;

        failed = true;
    }

    public void SetData()
    {
        Debug.Log("Set Data");

        string dateStr = userData.Child("date").Value.ToString();
        DateTime date = Convert.ToDateTime(dateStr);
        synchronizationManager.SetLocalLatestDate(date);

        Dictionary<string, object> step = (Dictionary<string, object>)userData.Child("step").Value;
        Debug.Log("inited : " + step["inited"].ToString());
        Debug.Log("profileInited : " + step["profileInited"].ToString());
        Debug.Log("profileName : " + step["profileName"].ToString());
        Debug.Log("profileLevel : " + step["inited"].ToString());
        Debug.Log("startDate : " + step["startDate"].ToString());
        Debug.Log("currentContentNo : " + step["currentContentNo"].ToString());
        Debug.Log("lastContentNo : " + step["lastContentNo"].ToString());
        Debug.Log("lastWeekdayContentNo : " + step["lastWeekdayContentNo"].ToString());
        Debug.Log("lastWeekendContentNo : " + step["lastWeekendContentNo"].ToString());
        Debug.Log("contentLimitNum : " + step["contentLimitNum"].ToString());
        lmsManager.SetStep(step);

        List<Dictionary<string, object>> contents = new List<Dictionary<string, object>>();
        Debug.Log("Contents ChildrenCount : " + userData.Child("contents").ChildrenCount.ToString());
        for(int i = 0 ; i < userData.Child("contents").ChildrenCount ; i++)
        {
            Dictionary<string, object> content = (Dictionary<string, object>)userData.Child("contents").Child(i.ToString()).Value;
            Debug.Log("Content : " + content["date"].ToString());
            contents.Add(content);
        }
        lmsManager.SetContentDatas(contents);

        completed = true;
    }

}
