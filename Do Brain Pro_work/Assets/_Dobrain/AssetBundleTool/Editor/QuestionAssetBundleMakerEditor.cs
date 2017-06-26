using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Com.Coolchoon.AssetBundleMaker
{
    [CustomEditor(typeof(QuestionAssetBundleMaker))]
    public class QuestionAssetBundleMakerEditor : Editor {

        static QuestionAssetBundleMaker instance;

        static BuildTarget targetPlatform;

        static string assetBundleName;

        void OnEnable()
        {
            if(instance == null)
            {
                instance = target as QuestionAssetBundleMaker;
            }
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);


            // Set Platform
            string platformStr = EditorUserBuildSettings.activeBuildTarget.ToString().ToLower();
            if(platformStr == "ios")
                targetPlatform = BuildTarget.iOS;
            else if(platformStr == "android")
                targetPlatform = BuildTarget.Android;
            

            // Set Name
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Name");
                assetBundleName = platformStr + "_" + instance.week.ToString() + "_question_" + instance.uno.ToString("D3");
                EditorGUILayout.TextField(assetBundleName);
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(5);



            // Set Platform
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Platform");
                GUILayout.Label(platformStr);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);



            // Set Week
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Week");
                instance.week = (QuestionAssetBundleMaker.Week)EditorGUILayout.EnumPopup(instance.week);
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(5);



            // Set UNO
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("UNO");
                instance.uno = EditorGUILayout.IntField(instance.uno);
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(5);



            // Set Assets
            instance.assets = new List<GameObject>();
            if(instance.week == QuestionAssetBundleMaker.Week.weekday)
            {
                for(int i = 0 ; i < 8 ; i++)
                {
                    string path = "Assets/_Dobrain/Data/Question/" + instance.week.ToString() + "/Chapters/ch" + instance.uno.ToString() + "/Prefab/ch" + instance.uno.ToString() + "_q" + i.ToString() + ".prefab";
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    instance.assets.Add(prefab);
                }
            }
            else
            {
                string path = "Assets/_Dobrain/Data/Question/" + instance.week.ToString() + "/ch" + instance.uno.ToString() + ".prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                instance.assets.Add(prefab);
            }

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
            if(instance.assets.Count.Equals(0))
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

                string[] assetNames = new string[instance.assets.Count];
                for(int i = 0 ; i < instance.assets.Count ; i++)
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
