using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

using Com.Dobrain.Dobrainproject.Manager;


public class SceneContent : MonoBehaviour {

    public SynchronizationManager synchronizationManager;
    public LMSManager lmsManager;

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

        synchronizationManager.OnCompleteAuthenticateUserID -= OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID -= OnFailAuthenticateUserID;

        SaveData(userId);
    }

    void OnFailAuthenticateUserID ()
    {
        Debug.Log("Fail Authenticate");

        synchronizationManager.OnCompleteAuthenticateUserID -= OnCompleteAuthenticateUserID;
        synchronizationManager.OnFailAuthenticateUserID -= OnFailAuthenticateUserID;

        failed = true;
    }

    void SaveData(string userId)
    {
        Debug.Log("Save Data");

        synchronizationManager.SetLocalLatestDate(UnbiasedTime.Instance.Now());

        synchronizationManager.SaveData(userId, UnbiasedTime.Instance.Now(), lmsManager.GetStep(), lmsManager.GetContentDatas());

        completed = true;
    }
	
}
