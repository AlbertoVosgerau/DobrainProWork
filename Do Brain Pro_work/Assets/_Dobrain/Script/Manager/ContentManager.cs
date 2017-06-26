using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

using Firebase.Storage;
using SimpleDiskUtils;

namespace Com.Dobrain.Dobrainproject.Manager
{
    public class ContentManager : MonoBehaviour 
    {
        static ContentManager _instance;
        public static ContentManager instance
        {
            get
            {
                if(_instance == null)
                {
                    GameObject go = Resources.Load("ContentManager") as GameObject;
                    _instance = Instantiate(go).GetComponent<ContentManager>();

                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _instance.platform = "android";
                    #elif UNITY_IOS && !UNITY_EDITOR
                    _instance.platform = "ios";
                    #endif

                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public class WeekdayAnimationCacheInfo
        {
            public List<int> cachedUNOList = new List<int>();
        }

        public delegate void EventCompleteHandler();
        public event EventCompleteHandler OnLoadQuestionComplete;
        public event EventCompleteHandler OnLoadAnimationComplete;
        public delegate void EventFailHandler(string error);
        public event EventFailHandler OnLoadQuestionFail;
        public event EventFailHandler OnLoadAnimationFail;

        public string platform = "ios";
       
        AssetBundle weekdayQuestionAssetBundle;
        AssetBundle weekendQuestionAssetBundle;


        public void ClearWeekdayQuestionAssetBundle()
        {
            if(weekdayQuestionAssetBundle != null)
            {
                weekdayQuestionAssetBundle.Unload(true);
                weekdayQuestionAssetBundle = null;
            }
        }

        public GameObject GetWeekdayQuestion(int uno, int index)
        {
            GameObject questionPrefab = weekdayQuestionAssetBundle.LoadAsset<GameObject>("ch" + uno.ToString() + "_q" + index.ToString() + ".prefab") as GameObject;
            GameObject question = Instantiate(questionPrefab, Vector2.zero, Quaternion.identity);
            return question;
        }

        public void LoadWeekdayQuestion(int uno)
        {
            StartCoroutine(LoadWeekdayQuestionRoutine(uno));
        }

        IEnumerator LoadWeekdayQuestionRoutine(int uno)
        {
            string url = PlayerPrefs.GetString(platform + "_weekday_question_" + uno.ToString("D3") + "_url");
            if(string.IsNullOrEmpty(url))
            {
                if(Application.internetReachability == NetworkReachability.NotReachable)
                {
                    if(OnLoadQuestionFail != null)
                        OnLoadQuestionFail("인터넷을 연결해주세요!");

                    yield break;
                }

                string fileName = platform + "_weekday_question_" + uno.ToString("D3");
                string storagePath = "Contents/Weekday/Weekday" + uno.ToString("D3") + "/" + fileName;
                StorageReference storage = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://dobrain-pro.appspot.com");
                StorageReference question_ref = storage.Child(storagePath);
                question_ref.GetDownloadUrlAsync().ContinueWith(task => {
                    if(!task.IsFaulted && !task.IsCanceled)
                        url = task.Result.ToString();
                });

                while(string.IsNullOrEmpty(url))
                    yield return null;

                PlayerPrefs.SetString(platform + "_weekday_question_" + uno.ToString("D3") + "_url", url);
            }

            while(!Caching.ready)
                yield return null;

            if(!Caching.IsVersionCached(url, 0))
            {
                if(Application.internetReachability == NetworkReachability.NotReachable)
                {
                    if(OnLoadQuestionFail != null)
                        OnLoadQuestionFail("인터넷을 연결해주세요!");

                    yield break;
                }

                if(DiskUtils.CheckAvailableSpace() < 100)
                {
                    if(OnLoadQuestionFail != null)
                        OnLoadQuestionFail("저장 공간이 부족합니다!");

                    yield break;
                }
            }

            using(WWW www = WWW.LoadFromCacheOrDownload(url, 0))
            {
                yield return www;
                if(www.error != null)
                    throw new Exception("www load has error : " + www.error);

                weekdayQuestionAssetBundle = www.assetBundle;

                Caching.MarkAsUsed(url, 0);

                www.Dispose();
            }

            if(OnLoadQuestionComplete != null)
                OnLoadQuestionComplete();
        }

        public void ClearWeekendQuestionAssetBundle()
        {
            if(weekendQuestionAssetBundle != null)
            {
                weekendQuestionAssetBundle.Unload(true);
                weekendQuestionAssetBundle = null;
            }
        }

        public GameObject GetWeekendQuestion(int uno)
        {
            GameObject questionPrefab = weekendQuestionAssetBundle.LoadAsset<GameObject>("ch" + uno.ToString() + ".prefab");
            GameObject question = Instantiate(questionPrefab, Vector2.zero, Quaternion.identity);
            return question;
        }

        public void LoadWeekendQuestion(int uno)
        {
            StartCoroutine("LoadWeekendQuestionRoutine", uno);
        }

        IEnumerator LoadWeekendQuestionRoutine(int uno)
        {
            string url = PlayerPrefs.GetString(platform + "_weekend_question_" + uno.ToString("D3") + "_url");
            if(string.IsNullOrEmpty(url))
            {
                if(Application.internetReachability == NetworkReachability.NotReachable)
                {
                    if(OnLoadQuestionFail != null)
                        OnLoadQuestionFail("인터넷을 연결해주세요!");

                    yield break;
                }

                string fileName = platform + "_weekend_question_" + uno.ToString("D3");
                string storagePath = "Contents/Weekend/" + fileName;
                StorageReference storage = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://dobrain-pro.appspot.com");
                StorageReference question_ref = storage.Child(storagePath);
                question_ref.GetDownloadUrlAsync().ContinueWith(task => {
                    if(!task.IsFaulted && !task.IsCanceled)
                        url = task.Result.ToString();
                });

                while(string.IsNullOrEmpty(url))
                    yield return null;

                PlayerPrefs.SetString(platform + "_weekend_question_" + uno.ToString("D3") + "_url", url);
            }

            while(!Caching.ready)
                yield return null;

            if(!Caching.IsVersionCached(url, 0))
            {
                if(Application.internetReachability == NetworkReachability.NotReachable)
                {
                    if(OnLoadQuestionFail != null)
                        OnLoadQuestionFail("인터넷을 연결해주세요!");

                    yield break;
                }

                if(DiskUtils.CheckAvailableSpace() < 100)
                {
                    if(OnLoadQuestionFail != null)
                        OnLoadQuestionFail("저장 공간이 부족합니다!");

                    yield break;
                }
            }

            using(WWW www = WWW.LoadFromCacheOrDownload(url, 0))
            {
                yield return www;
                if(www.error != null)
                    throw new Exception("www load has error : " + www.error);

                weekendQuestionAssetBundle = www.assetBundle;

                Caching.MarkAsUsed(url, 0);

                www.Dispose();
            }

            if(OnLoadQuestionComplete != null)
                OnLoadQuestionComplete();
        }

        public string GetWeekdayAnimationPath(int uno, int index)
        {
            string path = "file://" + Application.persistentDataPath + "/Contents/Weekday/Weekday" + uno.ToString("D3") + "/weekday_ani_" + uno.ToString("D3") + "_" + index.ToString() + ".mp4";
            return path;
        }

        public bool IsWeekdayAnimationCached(int uno, int index)
        {
            string local_dir = Application.persistentDataPath + "/Contents/Weekday/Weekday" + uno.ToString("D3");
            if(!Directory.Exists(local_dir))
                return false;

            string fileName = "weekday_ani_" + uno.ToString("D3") + "_" + index.ToString() + ".mp4";
            string local_path = local_dir + "/" + fileName;
            if(!File.Exists(local_path))
                return false;

            return true;
        }

        public void LoadWeekdayAnimation(int uno, int index)
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                if(OnLoadAnimationFail != null)
                    OnLoadAnimationFail("인터넷을 연결해주세요!");
            }
            else
            {
                if(DiskUtils.CheckAvailableSpace() < 100)
                {
                    if(OnLoadAnimationFail != null)
                        OnLoadAnimationFail("저장 공간이 부족합니다!");
                }
                else
                    StartCoroutine(LoadWeekdayAnimationRoutine(uno, index));
            }
        }

        IEnumerator LoadWeekdayAnimationRoutine(int uno, int index)
        {
            string local_dir = Application.persistentDataPath + "/Contents/Weekday/Weekday" + uno.ToString("D3");
            if(!Directory.Exists(local_dir))
                Directory.CreateDirectory(local_dir);
            

            string fileName = "weekday_ani_" + uno.ToString("D3") + "_" + index.ToString() + ".mp4";
            string local_path = local_dir + "/" + fileName;
            Debug.Log(local_path);
            Debug.Log("check file exits");
            if(!File.Exists(local_path))
            {
                Debug.Log("request url");

                string url = "";
                string storagePath = "Contents/Weekday/Weekday" + uno.ToString("D3") + "/" + fileName;
                StorageReference storage = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://dobrain-pro.appspot.com");
                StorageReference ani_ref = storage.Child(storagePath);
                ani_ref.GetDownloadUrlAsync().ContinueWith(task => {
                    if(!task.IsFaulted && !task.IsCanceled)
                        url = task.Result.ToString();
                });

                while(string.IsNullOrEmpty(url))
                    yield return null;

                Debug.Log("dowload file");

                WWW www = new WWW(url);

                yield return www;

                File.WriteAllBytes(local_path, www.bytes);

                www.Dispose();
            }


            if(OnLoadAnimationComplete != null)
                OnLoadAnimationComplete();
        }



        public void AddWeekdayAnimationCache(int uno)
        {
            string key = "weekday_animation_cache_info";

            string infoStr = PlayerPrefs.GetString(key);

            WeekdayAnimationCacheInfo info = null;

            if(string.IsNullOrEmpty(infoStr))
                info = new WeekdayAnimationCacheInfo();
            else
            {
                info = JsonUtility.FromJson<WeekdayAnimationCacheInfo>(infoStr);


                if(0 < info.cachedUNOList.Find(x => x.Equals(uno)))
                    return;

                if(info.cachedUNOList.Count == 5)
                {
                    int deleteUNO = info.cachedUNOList[0];
                    string local_dir = Application.persistentDataPath + "/Contents/Weekday/Weekday" + deleteUNO.ToString("D3");
                    if(Directory.Exists(local_dir))
                    {
                        for(int i = 0 ; i < 9 ; i++)
                        {
                            string filePath = local_dir + "/weekday_ani_" + deleteUNO.ToString("D3") + "_" + i.ToString() + ".mp4";
                            if(File.Exists(filePath))
                                File.Delete(filePath);
                        }
                        Directory.Delete(local_dir);
                        info.cachedUNOList.RemoveAt(0);
                    }
                }
            }
            
            info.cachedUNOList.Add(uno);

            infoStr = JsonUtility.ToJson(info);

            PlayerPrefs.SetString(key, infoStr);
        }
    	
    }
}
