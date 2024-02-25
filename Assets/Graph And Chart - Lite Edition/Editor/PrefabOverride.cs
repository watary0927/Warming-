﻿//#if UNITY_2018_3_OR_NEWER
//#if UNITY_EDITOR
//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using ChartAndGraph;

//public class PrefabOverride : UnityEditor.AssetPostprocessor
//{
//    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//    {
//        foreach (string path in importedAssets)
//        {
//            string lowPath = path.ToLower();
//            if (lowPath.EndsWith(".prefab"))
//            {
//                if (ContainsPath(lowPath) == false)
//                {
//                  //  Debug.Log(lowPath);
//                    EditorApplication.delayCall+= ()=> CleanPrefab(lowPath);
//                    AddPath(lowPath);
//                }
//            }

//        }
//        foreach(string path in deletedAssets)
//        {
//            RemovePath( path.ToLower());
//        }
//    }
//    [MenuItem("EditorPrefs/Clear all Editor Preferences")]
//    static void deleteAllExample()
//    {

//            EditorPrefs.DeleteAll();
//    }
//    static bool ContainsPath(string path)
//    {
//        return EditorPrefs.GetBool("GraphAndChartPrefabOverride$" +path, false);
//    }
//    static void RemovePath(string path)
//    {
//        EditorPrefs.DeleteKey("GraphAndChartPrefabOverride$" + path);
//    }
//    static void AddPath(string path)
//    {
//        EditorPrefs.SetBool("GraphAndChartPrefabOverride$" + path, true);
//    }
//    static void CleanPrefab(string path)
//    {
//        GameObject obj = PrefabUtility.LoadPrefabContents(path);
//        //AssetDatabase.DeleteAsset(path);
//        foreach (var item in obj.GetComponentsInChildren<AnyChart>(true))
//        {
//            //          if (item == null)
//            //              continue;
//            //            if (item.gameObject == null)
//            //                continue;
//           // Debug.Log("destroy " + item.gameObject.name);
//            while(item.gameObject.transform.childCount > 0)
//            {
//                var innerObj = item.gameObject.transform.GetChild(0).gameObject;
//                if (innerObj != null)
//                {
//                 //   Debug.Log("destroy inner" + innerObj.name);
                    
//                    GameObject.DestroyImmediate(innerObj);
//                }
//            }
            
            
//        }

//        PrefabUtility.SaveAsPrefabAsset(obj,path);
//        PrefabUtility.UnloadPrefabContents(obj);
//    }

//}
//#endif
//#endif
