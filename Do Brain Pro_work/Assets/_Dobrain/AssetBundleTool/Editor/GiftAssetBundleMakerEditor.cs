using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Com.Coolchoon.AssetBundleMaker
{
    [CustomEditor(typeof(GiftAssetBundleMaker))]
    public class GiftAssetBundleMakerEditor : Editor {

        static GiftAssetBundleMaker instance;

        static BuildTarget targetPlatform = BuildTarget.Android;

        static string assetBundleName;

        void OnEnable()
        {
            if(instance == null)
                instance = target as GiftAssetBundleMaker;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);


            // Set Name
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Name");
                assetBundleName = targetPlatform.ToString().ToLower() + "_ui_gift";
                EditorGUILayout.TextField(assetBundleName);
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(5);


            // Set Platform
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Platform");
                targetPlatform = (BuildTarget)EditorGUILayout.EnumPopup(targetPlatform);
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(5);



            // Set Assets
            serializedObject.Update();
            SerializedProperty assets = serializedObject.FindProperty("assets");

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(assets, true);
            }
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }


            GUILayout.Space(5);





            // Check Can Make
            bool canMake = true;
            if(string.IsNullOrEmpty(assetBundleName))
            {
                GUILayout.Label("(!) Please, set name");
                canMake = false;
            }
            if(instance.assets.Length.Equals(0))
            {
                GUILayout.Label("(!) Please, set assets");
                canMake = false;
            }
            if(!canMake)
                return;



            // Make AssetBunlde
            if(GUILayout.Button("Make", GUILayout.Height(30f)))
            {
                AssetBundleBuild[] builds = new AssetBundleBuild[1];

                builds[0].assetBundleName = assetBundleName;

                string[] assetNames = new string[instance.assets.Length];
                for(int i = 0 ; i < instance.assets.Length ; i++)
                {
                    if(instance.assets[i] != null)
                        assetNames[i] = AssetDatabase.GetAssetPath(instance.assets[i]);
                }
                
                builds[0].assetNames = assetNames;

                Debug.Log(assetNames[0]);
                BuildPipeline.BuildAssetBundles("Assets/AssetBundles", builds, BuildAssetBundleOptions.None, targetPlatform);
            }
        }

        [MenuItem("Dobrain/Clean Cache")]
        public static void CleanCache()
        {
            Caching.CleanCache();
        }
    	
    }
}
