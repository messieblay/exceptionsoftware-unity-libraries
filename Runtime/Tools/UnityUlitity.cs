using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class UnityUtility
{

    public static Transform TryFindChildOrCreate(Transform parent, string objname)
    {
        Transform t = parent.Find(objname);
        if (t == null)
        {
            t = (new GameObject(objname)).transform;
            t.parent = parent;
        }
        return t;
    }

    #region Assets
    public static List<T> FindAssetsByType<T>(string nameFilter = "") where T : Object
    {
#if UNITY_EDITOR
        List<T> assets = new List<T>();
        string[] guids = UnityEditor.AssetDatabase.FindAssets($"{nameFilter} t: { typeof(T).Name}");
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
#else
            return null;
#endif
    }

    #endregion

    #region Extensions
    public static string GetGUID(this UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
#else
            return "";
#endif
    }
    public static string GetAssetPath(this UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        return AssetDatabase.GetAssetPath(obj);
#else
            return "";
#endif
    }
    public static void GetAssetPath(UnityEngine.Object obj, out string guid, out string path)
    {
#if UNITY_EDITOR
        path = AssetDatabase.GetAssetPath(obj);
        guid = AssetDatabase.AssetPathToGUID(path);
#else
            path = "";
            guid = "";
#endif
    }

    public static Object GetObjectFromGUID(this string guid)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Object));
#else
            return null;
#endif
    }

    #endregion
}
