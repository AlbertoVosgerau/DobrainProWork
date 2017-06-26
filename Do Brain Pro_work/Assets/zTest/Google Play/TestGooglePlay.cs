using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGooglePlay : MonoBehaviour {

    public TestUIGooglePlay ui;

    void Start()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        ui.SetLog("Start RetrieveDeviceGoogleAccounts");
        GooglePlayManager.ActionAvailableDeviceAccountsLoaded += ActionAvailableDeviceAccountsLoaded;
        GooglePlayManager.Instance.RetrieveDeviceGoogleAccounts();
        #endif
    }

    private void ActionAvailableDeviceAccountsLoaded(List<string> accounts) {
        #if UNITY_ANDROID && !UNITY_EDITOR
        GooglePlayManager.ActionAvailableDeviceAccountsLoaded -= ActionAvailableDeviceAccountsLoaded;

        string msg = "Device contains following google accounts:" + "\n";
        foreach(string acc in GooglePlayManager.Instance.deviceGoogleAccountList) {
            msg += acc + "\n";
        } 

        ui.SetLog("Accounts Loaded");
        ui.SetLog(msg);
        ui.SetLog("Sign With Fitst one Do Nothing");
        #endif
    }

}
