using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Storage;
using Firebase.Unity.Editor;

using SimpleDiskUtils;

public class ResourceManager : MonoBehaviour {

    static ResourceManager _instance;
    public static ResourceManager instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = Resources.Load("ResourceManager") as GameObject;
                _instance = Instantiate(go).GetComponent<ResourceManager>();


                #if UNITY_ANDROID && !UNITY_EDITOR
                _instance.platform = "android";
                #elif UNITY_IOS && !UNITY_EDITOR
                _instance.platform = "ios";
                #endif

                _instance.storage = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://dobrain-pro.appspot.com");
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }


    public delegate void EventHandler();
    public event EventHandler OnLoadCachedGiftsComplete;
    public event EventHandler OnLoadCachedGiftsFail;
    public event EventHandler OnLoadGiftsComplete;
    public delegate void EventFailHandler(string error);
    public event EventFailHandler OnLoadGiftsFail;

    public Sprite weekendGiftSprite;
    public Sprite[] pointSpriteList;
    public string platform = "android";

    StorageReference storage;
    AssetBundle giftAssetBundle;

    public void LoadCachedGifts()
    {
        StartCoroutine("LoadCachedGiftsRoutine");
    }

    IEnumerator LoadCachedGiftsRoutine()
    {
        if(giftAssetBundle == null)
        {
            string gift_url = PlayerPrefs.GetString(platform + "_ui_gift_url");
            if(string.IsNullOrEmpty(gift_url))
            {
                if(OnLoadCachedGiftsFail != null)
                    OnLoadCachedGiftsFail();

                yield break;
            }

            while(!Caching.ready)
                yield return null;

            int gift_version = int.Parse(PlayerPrefs.GetString(platform + "_ui_gift_version"));
            WWW www = WWW.LoadFromCacheOrDownload(gift_url, gift_version);

            yield return www;

            giftAssetBundle = www.assetBundle;

            Caching.MarkAsUsed(gift_url, gift_version);

            www.Dispose();
        }

        if(OnLoadCachedGiftsComplete != null)
            OnLoadCachedGiftsComplete();
    }

    public void LoadGifts()
    {
        StartCoroutine("LoadGiftsRoutine");
    }

    public IEnumerator LoadGiftsRoutine()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dobrain-pro.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        bool isNew = false;
        bool isGetGiftVersion = false;
        int gift_version = -1;
        string prev_gift_version_str = PlayerPrefs.GetString(platform + "_ui_gift_version");
        if(string.IsNullOrEmpty(prev_gift_version_str))
        {
            gift_version = 0;
            isNew = true;
            isGetGiftVersion = true;
        }
        else
        {
            FirebaseDatabase.DefaultInstance
                .GetReference("ui").Child("gift_version")
                .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                        if(OnLoadGiftsComplete != null)
                            OnLoadGiftsComplete();
                    }
                    else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        // Do something with snapshot...

                        int new_gift_version = int.Parse(snapshot.Value.ToString());
                        int prev_gift_version = int.Parse(prev_gift_version_str);
                        if(prev_gift_version < new_gift_version)
                        {
                            Debug.Log("gift version check : " + prev_gift_version + " / " + new_gift_version);
                            gift_version = new_gift_version;
                            isNew = true;
                        }

                        isGetGiftVersion = true;
                    }
                });
        }

        while(!isGetGiftVersion)
            yield return null;

        string url = "";
        string storagePath = "UI/Gift/" + platform + "_ui_gift";
        StorageReference giftRef = storage.Child(storagePath);
        giftRef.GetDownloadUrlAsync().ContinueWith(task => {
            if(!task.IsFaulted && !task.IsCanceled)
                url = task.Result.ToString();
        });

        while(isGetGiftVersion && string.IsNullOrEmpty(url))
            yield return null;

        Debug.Log("Gift version : " + gift_version);
        Debug.Log("gift version is new : " + isNew);
        if(isNew)
        {
            if(DiskUtils.CheckAvailableSpace() < 100)
            {
                if(OnLoadGiftsFail != null)
                    OnLoadGiftsFail("저장 공간이 부족합니다!");

                yield break;
            }
        }
        else
        {
            if(OnLoadGiftsComplete != null)
                OnLoadGiftsComplete();

            yield break;
        }

        while(!Caching.ready)
            yield return null;
        
        WWW www = WWW.LoadFromCacheOrDownload(url, gift_version);

        yield return www;

        giftAssetBundle = www.assetBundle;

        Caching.MarkAsUsed(url, gift_version);

        www.Dispose();

        PlayerPrefs.SetString(platform + "_ui_gift_version", gift_version.ToString());
        PlayerPrefs.SetString(platform + "_ui_gift_url", url);

        if(OnLoadGiftsComplete != null)
            OnLoadGiftsComplete();
    }

    public Sprite GetWeekdayGift(int uno, Def.ContentState state)
    {
        string fileName = "";
        if(state == Def.ContentState.Cleared)
            fileName = "gift_" + uno.ToString("D3") + "_cleared";
        else
            fileName = "gift_" + uno.ToString("D3") + "_lock";
        
        if(uno <= 30)
            return Resources.Load<Sprite>("UI/Gift/" + fileName);
        else
            return giftAssetBundle.LoadAsset<Sprite>(fileName);
    }

    public Sprite GetWeekendGift(int point)
    {
        Sprite sprite = null;

        if(point == 0)
            sprite = weekendGiftSprite;
        else
            sprite = pointSpriteList[point - 1];
        
        return sprite;
    }

    public Sprite GetPoint(int point)
    {
        return pointSpriteList[point];
    }


}
