//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

namespace Dobrain.LMS
{
    public class GalleryImage : MonoBehaviour
    {
        GalleryManager galleyManager;
        public string filePath;
        public Button deleteBtn;

        void Start()
        {
            galleyManager = GameObject.Find("ParentsManager").GetComponent<GalleryManager>();

            GetComponent<Button>().onClick.AddListener(delegate() { ImageSelect(); });

            //deleteBtn = GameObject.Find("DeleteBtn").GetComponent<Button>();
        }

        public void ImageSelect()
        {
            galleyManager.SelectImage(gameObject);
            //deleteBtn.onClick.AddListener(delegate() { ImageDelete(); });
        }

        public void ImageDelete()
        {
            galleyManager.DeleteFile(filePath,this.gameObject);
        }
    }
}
