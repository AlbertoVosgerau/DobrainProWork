using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Com.Dobrain.Dobrainproject.Scene
{
    public class SceneParents : MonoBehaviour {
        
        public GameObject inputBlock;

        AsyncOperation profileAsyncOperation;

        void Awake()
        {
            inputBlock.SetActive(false);
        }

        public void ShowProfilePopup()
        {
            profileAsyncOperation = SceneManager.LoadSceneAsync("Profile", LoadSceneMode.Additive);
            StartCoroutine("ShowProfilePopupRoutine");
        }

        IEnumerator ShowProfilePopupRoutine()
        {
            inputBlock.SetActive(true);

            while(!profileAsyncOperation.isDone)
                yield return null;


            UnityEngine.SceneManagement.Scene profileScene = SceneManager.GetSceneByName("Profile");
            while(true)
            {
                if(profileScene.isLoaded)
                    yield return null;
                else
                {
                    inputBlock.SetActive(false);
                    yield break;
                }
            }
        }




    }
}
