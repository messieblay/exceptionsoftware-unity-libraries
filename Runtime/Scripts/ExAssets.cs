using UnityEngine;

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

    public static T CreateAsset<T>(string path, string name, bool refresh = true, bool avoidIfExist = false) where T : ScriptableObject
    {
#if UNITY_EDITOR 
        return CreateAsset(path, name, typeof(T), refresh, avoidIfExist) as T;
#else
        return default(T);
#endif
    }
    public static ScriptableObject CreateAsset(string path, string name, System.Type type, bool refresh = true, bool avoidIfExist = false)
    {
#if UNITY_EDITOR
        if (path.EndsWith("/"))
        {
            path = path.Remove(path.Length - 1);
        }
        string finalPath = $"{path}/{name}.asset";
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(finalPath);

        if (avoidIfExist)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(finalPath, type);
            if (obj)
            {
                return obj as ScriptableObject;
            }

            if (finalPath != assetPathAndName)
            {
                return null;
            }
        }

        ScriptableObject asset = ScriptableObject.CreateInstance(type);
        AssetDatabase.CreateAsset(asset, assetPathAndName);

        if (refresh)
        {
            SaveAssets();
        }

        return asset;
#endif
    }

    public static void SaveAsset(UnityEngine.Object asset)
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
        UnityEngine.Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
        string path = AssetDatabase.GetAssetPath(prefab);
        UnityEditor.AssetDatabase.RenameAsset(path, name);
#endif
    }


    /// <summary>
    /// Search asset by Type with 
    /// AssetDatabase.FindAssets($"t: {typeof(T).Name} {nameFilter}");
    /// and 
    /// Resources.FindObjectsOfTypeAll<T>()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nameFilter"></param>
    /// <returns></returns>
    public static List<T> FindAssetsByType<T>(string nameFilter = "") where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        T[] assetsResourceS = Resources.FindObjectsOfTypeAll<T>();
        if (assetsResourceS != null && assetsResourceS.Length > 0)
        {
            if (nameFilter == "")
            {
                assets.AddRange(assetsResourceS);
            }
            else
            {
                foreach (T t in assetsResourceS)
                {
                    if (t.name.Contains(nameFilter))
                    {
                        assets.Add(t);
                    }
                }
            }
        }
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets($"t: {typeof(T).Name} {nameFilter}");
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.AddUnique(asset);
            }
        }
        assets.ClearNulls();
        return assets;
#else
        return assets;
#endif
    }
    public static List<UnityEngine.Object> FindAssetsByType(System.Type type, string nameFilter = "")
    {
#if UNITY_EDITOR
        List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
        string[] guids = AssetDatabase.FindAssets(string.Format("{1} t: {0}", type.Name, nameFilter));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
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


    public static void RenameAsset(ScriptableObject scriptable, string newname)
    {
        string path = AssetDatabase.GetAssetPath(scriptable);
        AssetDatabase.RenameAsset(path, newname);
    }


    public static void DeleteAsset(ScriptableObject scriptable)
    {
        string path = AssetDatabase.GetAssetPath(scriptable);
        AssetDatabase.DeleteAsset(path);
    }



    private static void CleanMissingSubAssets(UnityEngine.Object toDelete)
    {
        //Create a new instance of the object to delete
        ScriptableObject newInstance = ScriptableObject.CreateInstance(toDelete.GetType());

        //Copy the original content to the new instance
        EditorUtility.CopySerialized(toDelete, newInstance);
        newInstance.name = toDelete.name;

        string toDeletePath = AssetDatabase.GetAssetPath(toDelete);
        string clonePath = toDeletePath.Replace(".asset", "CLONE.asset");

        //Create the new asset on the project files
        AssetDatabase.CreateAsset(newInstance, clonePath);
        AssetDatabase.ImportAsset(clonePath);

        //Unhide sub-assets
        var subAssets = AssetDatabase.LoadAllAssetsAtPath(toDeletePath);
        HideFlags[] flags = new HideFlags[subAssets.Length];
        for (int i = 0; i < subAssets.Length; i++)
        {
            //Ignore the "corrupt" one
            if (subAssets[i] == null)
                continue;

            //Store the previous hide flag
            flags[i] = subAssets[i].hideFlags;
            subAssets[i].hideFlags = HideFlags.None;
            EditorUtility.SetDirty(subAssets[i]);
        }

        EditorUtility.SetDirty(toDelete);
        AssetDatabase.SaveAssets();

        //Reparent the subAssets to the new instance
        foreach (var subAsset in AssetDatabase.LoadAllAssetRepresentationsAtPath(toDeletePath))
        {
            //Ignore the "corrupt" one
            if (subAsset == null)
                continue;

            //We need to remove the parent before setting a new one
            AssetDatabase.RemoveObjectFromAsset(subAsset);
            AssetDatabase.AddObjectToAsset(subAsset, newInstance);
        }

        //Import both assets back to unity
        AssetDatabase.ImportAsset(toDeletePath);
        AssetDatabase.ImportAsset(clonePath);

        //Reset sub-asset flags
        for (int i = 0; i < subAssets.Length; i++)
        {
            //Ignore the "corrupt" one
            if (subAssets[i] == null)
                continue;

            subAssets[i].hideFlags = flags[i];
            EditorUtility.SetDirty(subAssets[i]);
        }

        EditorUtility.SetDirty(newInstance);
        AssetDatabase.SaveAssets();

        //Here's the magic. First, we need the system path of the assets
        string globalToDeletePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), toDeletePath);
        string globalClonePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), clonePath);

        //We need to delete the original file (the one with the missing script asset)
        //Rename the clone to the original file and finally
        //Delete the meta file from the clone since it no longer exists

        System.IO.File.Delete(globalToDeletePath);
        System.IO.File.Delete(globalClonePath + ".meta");
        System.IO.File.Move(globalClonePath, globalToDeletePath);

        AssetDatabase.Refresh();
    }



}
