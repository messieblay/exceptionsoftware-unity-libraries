#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
#endif

using UnityEngine.SceneManagement;

public static class Unityx
{
    #region Scene & Asset Management
    public static void SetDirty()
    {
        SetSceneDirty();
        SaveAssets();
    }


    public static void SetSceneDirty()
    {
#if UNITY_EDITOR
        EditorSceneManager.MarkAllScenesDirty();
#endif
    }

    public static void SetSceneDirty(Scene scene)
    {
#if UNITY_EDITOR
        EditorSceneManager.MarkSceneDirty(scene);
#endif
    }

    public static void SaveAssets()
    {
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }

    /// <summary>
    /// Set componente Dirty
    /// </summary>
    /// <param name="comp"></param>
    public static void SetDirty(Object comp)
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(comp);
#endif
    }

    public static void SetComponentDirty(this Component comp, bool dirty = true)
    {
#if UNITY_EDITOR
        if (dirty)
            EditorUtility.SetDirty(comp);
#endif
    }


    #endregion
    #region Display ProgressBar

#if UNITY_EDITOR
    public static void BeginDisplay()
    {
        _totalSteps = 8;
        _step = 0;
    }

    public static void Display(string description)
    {
        EditorUtility.DisplayProgressBar("Creating Scene", description, _step / _totalSteps);
        _step++;
    }

    public static void EndDisplay()
    {
        EditorUtility.ClearProgressBar();
    }

    static float _totalSteps = 8;
    static float _step = 0;
#endif

    #endregion


    public static Transform TryFindChildByNameOrCreate(Transform parent, string objname)
    {
        Transform t = parent.Find(objname);
        if (t == null)
        {
            t = (new GameObject(objname)).transform;
            t.parent = parent;
        }
        return t;
    }


}
