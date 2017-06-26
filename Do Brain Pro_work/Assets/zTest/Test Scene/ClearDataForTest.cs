using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

using Com.Dobrain.Dobrainproject.Manager;

public class ClearDataForTest : MonoBehaviour {

    public void CleanData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void CleanCache()
    {
        Caching.CleanCache();
    }

    public void CleanAni()
    {   
        for(int di = 1 ; di <= 10 ; di++)
        {
            string local_dir = Application.persistentDataPath + "/Contents/Weekday/Weekday" + di.ToString("D3");
            if(Directory.Exists(local_dir))
            {
                for(int i = 0 ; i < 9 ; i++)
                {
                    string filePath = local_dir + "/weekday_ani_" + di.ToString("D3") + "_" + i.ToString() + ".mp4";
                    if(File.Exists(filePath))
                        File.Delete(filePath);
                }
                Directory.Delete(local_dir);
            }
        }
    }

    public void Next(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
