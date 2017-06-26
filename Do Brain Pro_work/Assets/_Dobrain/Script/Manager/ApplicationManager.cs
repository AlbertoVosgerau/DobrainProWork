using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

namespace Com.Dobrain.Dobrainproject.Manager
{
    public class ApplicationManager : MonoBehaviour {
        
        public delegate void EventHandler(bool result);
        public event EventHandler OnCheckLatestVersionComplete;
        public delegate void EventFailHandler();
        public event EventFailHandler OnCheckLatestVersionFail;

        public static int IOS_BUILD = 2;

        public bool CanUseInternet()
        {
            bool result = true;
            NetworkReachability networkReachability = Application.internetReachability;
            if(networkReachability == NetworkReachability.NotReachable)
                result = false;

            return result;
        }

        public void CheckLatestVersion()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidAppInfoLoader.ActionPacakgeInfoLoaded += OnPackageInfoLoadedForAOS;
            AndroidAppInfoLoader.Instance.LoadPackageInfo ();
            #elif UNITY_IOS && !UNITY_EDITOR
            CheckLatestVersion_(IOS_BUILD);
            #else
            CheckLatestVersion_(1);
//            if(OnCheckLatestVersionFail != null)
//                OnCheckLatestVersionFail();
            #endif
        }

        void OnPackageInfoLoadedForAOS(PackageAppInfo PacakgeInfo) {
            AndroidAppInfoLoader.ActionPacakgeInfoLoaded -= OnPackageInfoLoadedForAOS; 

            // Get Current Version
            int currentVersionCode = int.Parse(PacakgeInfo.versionCode);


            CheckLatestVersion_(currentVersionCode);
        }

        void CheckLatestVersion_(int currentVersionCode)
        {
            // Set up the Editor before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dobrain-pro.firebaseio.com/");

            // Get the root reference location of the database.
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            string key = string.Empty;
            #if UNITY_ANDROID && !UNITY_EDITOR
            key = "version_code";
            #elif UNITY_IOS && !UNITY_EDITOR
            key = "build";
            #else
            key = "version_code";
            #endif


            FirebaseDatabase.DefaultInstance
                .GetReference("package_info").Child(key)
                .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                        if(OnCheckLatestVersionFail != null)
                            OnCheckLatestVersionFail();
                    }
                    else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        // Do something with snapshot...

                        if(OnCheckLatestVersionComplete != null)
                        {
                            // Get Latest Version
                            int latestVersionCode = int.Parse(snapshot.Value.ToString());
                            OnCheckLatestVersionComplete(currentVersionCode < latestVersionCode);
                        }
                    }
                });
        }

        public void SetNeverSleep()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
            

    }
}