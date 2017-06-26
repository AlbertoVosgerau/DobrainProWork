using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;

namespace Dobrain.LMS
{
    public class GalleryManager : MonoBehaviour
    {

        public GameObject galleryImageView;
        public GameObject imageObject;
        public Transform imageHolder;

        public string[] fName;
        public Object[] fObj;
        Texture2D img;

        // Use this for initialization
        void Start()
        {
            //fName = Directory.GetFiles("C:\\Users\\영일\\Downloads\\no", "*.png"); // 경로에 있는 지정한 확장자 파일 이름을 반환(경로를 포함한 풀 네임입니다) 
            string sDirPath;

            sDirPath = Application.persistentDataPath + "/screenshots";

            DirectoryInfo di = new DirectoryInfo(sDirPath);

            if (di.Exists == false)
            {
                di.Create();
            }

            fName = Directory.GetFiles(sDirPath , "*.png");

            FileInfo[] files = di.GetFiles("*.png");
            foreach (FileInfo file in files)
                file.Attributes = FileAttributes.Normal;

 
            imageHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(imageHolder.GetComponent<RectTransform>().sizeDelta.x, ((fName.Length / 4f)+1) * 350);

            for (int i=0; i < fName.Length; i++)
            {
                byte[] byteTexture = File.ReadAllBytes(fName[i]);

                if (byteTexture.Length > 0)
                {
                    img = new Texture2D(256, 128);
                    img.LoadImage(byteTexture);
                }
                GameObject imageInstance = Instantiate(imageObject);
                imageInstance.transform.SetParent(imageHolder, false);
                imageInstance.GetComponent<Image>().sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
                imageInstance.GetComponent<GalleryImage>().filePath = fName[0];
            }
        }

        public void SelectImage(GameObject thisobj)
        {
            ImageViewOpen();
            galleryImageView.GetComponentsInChildren<Image>()[1].sprite = thisobj.GetComponent<Image>().sprite;
        }

        public void CloseImage()
        {
            ImageViewClose();
        }

        void ImageViewOpen()
        {
            galleryImageView.transform.DOScale(1, 0.5f);
        }

        void ImageViewClose()
        {
            galleryImageView.transform.DOScale(0, 0.5f);
        }

        public void DeleteFile(string path, GameObject gm)
        {
            Destroy(gm);
            ImageViewClose();
            Directory.Delete(path,true);
        }
    }
}