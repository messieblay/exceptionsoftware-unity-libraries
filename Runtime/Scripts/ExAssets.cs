﻿using UnityEngine;

using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
public static class ExAssets
{

    public static string ProjectPath
    {
        get
        {
            if (_projectPath == "")
            {
                _projectPath = UnityEngine.Application.dataPath;
                if (_projectPath.EndsWith("/Assets"))
                {
                    _projectPath = _projectPath.Remove(_projectPath.Length - ("Assets".Length));
                }
            }
            return _projectPath;
        }
    }

    static string _projectPath = "";

    [System.Obsolete("Nose cual era el objetivo de esto. Crea un asset en el path de la seleccion actual")]
    public static void CreateAsset<T>(bool refresh = true) where T : ScriptableObject
    {
#if UNITY_EDITOR
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        if (refresh)
        {
            SaveAssets();
        }

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
#endif
    }

    public static T CreateAsset<T>(string path, string name, bool refresh = true) where T : ScriptableObject
    {
#if UNITY_EDITOR
        T asset = ScriptableObject.CreateInstance<T>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        if (refresh)
        {
            SaveAssets();
        }

        return asset;
#else
        return default(T);
#endif
    }

    public static void SaveAsset(Object asset)
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(asset);
        SaveAssets();
#endif
    }

    public static void SaveAssets()
    {
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

    public static void DeleteDirectory(string path)
    {
#if UNITY_EDITOR
        System.IO.DirectoryInfo di = new DirectoryInfo(path);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
        System.IO.Directory.Delete(path);
        AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// Renames the prefab.
    /// </summary>
    /// <param name="prefab">Prefab.</param>
    /// <param name="name">Name.</param>
    public static void RenamePrefab(GameObject prefab, string name)
    {
#if UNITY_EDITOR
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
        string path = AssetDatabase.GetAssetPath(prefab);
        UnityEditor.AssetDatabase.RenameAsset(path, name);
#endif
    }



    public static List<T> FindAssetsByType<T>(string nameFilter = "") where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("{1} t: {0}", typeof(T).Name, nameFilter));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
#else
        return new List<T>();
#endif
    }












}