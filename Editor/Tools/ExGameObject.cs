using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class ExGameObject
{
    const string MENU_ITEM = "GameObject/Ex functions/";
    const string TRANSFORM_ITEM = "Transform/";
    const string HIERARCHY_ITEM = "Hierarchy/";
    const string OTHERS_ITEM = "Others/";
    const int TRANSFORM_ORDER = 0;
    const int HIERARCHY_ORDER = 100;
    const int OTHERS_ORDER = 10000;

#if UNITY_EDITOR
    static float _lastMenuCallTimestamp = 0f;
    [MenuItem(MENU_ITEM + TRANSFORM_ITEM + "Create Parent", false, TRANSFORM_ORDER)]
    static void ParentSingle()
    {

        if (Time.unscaledTime.Equals(_lastMenuCallTimestamp)) return;
        // place your code here
        CreateParent(Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable));
        _lastMenuCallTimestamp = Time.unscaledTime;
    }

    [MenuItem(MENU_ITEM + TRANSFORM_ITEM + "Un Parent", false, TRANSFORM_ORDER)]
    static void UnParent()
    {
        if (Selection.gameObjects == null) return;

        foreach (GameObject go in Selection.gameObjects)
        {
            Undo.SetTransformParent(go.transform, null, "Parent");
            go.transform.parent = null;
        }
    }

    [MenuItem(MENU_ITEM + TRANSFORM_ITEM + "Parent Up", false, TRANSFORM_ORDER)]
    static void ParentUp()
    {
        if (Selection.gameObjects == null) return;


        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.transform.parent == null)
            {
                Undo.SetTransformParent(go.transform, null, "ParentUp");
            }
            else
            {
                Undo.SetTransformParent(go.transform, go.transform.parent.parent, "ParentUp");
            }
        }
    }



    [MenuItem(MENU_ITEM + TRANSFORM_ITEM + "Clear childs", false, TRANSFORM_ORDER)]
    public static void ClearChilds()
    {
        GameObject go = Selection.activeGameObject;
        go.transform.Clear();
    }

    public static void CreateParent(params Transform[] gos)
    {
        Vector3 center = Vector3.zero;
        foreach (Transform go in gos)
        {
            center += go.position;
        }
        center /= gos.Length;

        GameObject goparent = new GameObject("Parent");
        goparent.transform.parent = gos[0].parent;
        goparent.transform.position = center;
        Undo.RegisterCreatedObjectUndo(goparent, "Object parent");


        foreach (Transform go in gos)
        {
            Undo.SetTransformParent(go.transform, goparent.transform, "Parent");
        }
    }

    [MenuItem(MENU_ITEM + TRANSFORM_ITEM + "Sort children by name", false, TRANSFORM_ORDER)]
    public static void SortChildrenByName()
    {
        SortChildrenByName(Selection.gameObjects);
    }

    public static void SortChildrenByName(params GameObject[] gameobjects)
    {
        foreach (GameObject obj in gameobjects)
        {
            List<Transform> children = new List<Transform>();
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = obj.transform.GetChild(i);
                children.Add(child);
                child.parent = null;
            }
            children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
            foreach (Transform child in children)
            {
                child.parent = obj.transform;
            }
        }
    }


    #region Clean GameObjects

    [MenuItem(MENU_ITEM + OTHERS_ITEM + "Try Clean Empty GameObjects", false, OTHERS_ORDER)]
    public static void TryCleanEmptyGameObjects()
    {
        Transform[] ts = GameObject.FindObjectsOfType<Transform>().Where(s => s.name.StartsWith("GameObject") && s.childCount == 0).ToArray();
        Debug.Log($"Primera criba {ts.Length}");
        ts = ts.Where(s => s.GetComponents<Component>().Length == 1).ToArray();
        Debug.Log($"Segunda criba {ts.Length}");
        foreach (Transform t in ts)
        {
            try { Undo.DestroyObjectImmediate(t.gameObject); } catch { }
        }

        Debug.Log($"Criba final {GameObject.FindObjectsOfType<Transform>().Where(s => s.name.StartsWith("GameObject") && s.childCount == 0).Count()}");
    }
    #endregion


    [MenuItem(MENU_ITEM + HIERARCHY_ITEM + "Send first (In Hierarchy)", false, HIERARCHY_ORDER)]
    public static void SendFirst()
    {
        GameObject go = Selection.activeGameObject;
        go.transform.SetAsFirstSibling();
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }

    [MenuItem(MENU_ITEM + HIERARCHY_ITEM + "Send last (In Hierarchy)", false, HIERARCHY_ORDER)]
    public static void SendLast()
    {
        GameObject go = Selection.activeGameObject;
        go.transform.SetAsLastSibling();
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }


#endif

    #region Component
    public static void MoveUp(this Component comp) => UnityEditorInternal.ComponentUtility.MoveComponentUp(comp);
    public static void MoveDown(this Component comp) => UnityEditorInternal.ComponentUtility.MoveComponentDown(comp);
    public static void MoveTOP(this Component comp)
    {
        int index = ComponentIndex(comp);
        for (int i = index; 0 < i; i--)
        {
            UnityEditorInternal.ComponentUtility.MoveComponentUp(comp);
        }
    }
    public static int ComponentIndex(this Component comp) => comp.gameObject.GetComponents<Component>().ToList().IndexOf(comp);

    [MenuItem(MENU_ITEM + OTHERS_ITEM + "Clean components", false, HIERARCHY_ORDER)]
    public static void CleanComponents()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            go.GetComponents<Component>().ForEach(s =>
            {
                if (!(s is Transform))
                {
                    GameObject.DestroyImmediate(s);
                }
            });
        }
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }


    #endregion

    #region Hierarchy
    public static int GetDepth(this GameObject t) => GetDepth(t.transform);
    public static int GetDepth(this Transform t) => (t.parent == null) ? 0 : GetDepth(t.parent) + 1;
    #endregion



    #region Scene
#if UNITY_EDITOR

    [MenuItem(MENU_ITEM + TRANSFORM_ITEM + "Clean names", false, 0)]
    public static void CleanNameOfSelectedPrefab()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;
        Undo.RecordObject(go, "Name changed");
        go.name = CleanNumberName(go.name);
        Unityx.SetSceneDirty();
    }

    [MenuItem(MENU_ITEM + TRANSFORM_ITEM + "Clean names", false, 0)]
    public static void CleanNamesOfSelectedPrefab()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;
        if (go.name.Trim().EndsWith(")"))
        {
            string nameToFind = CleanNumberName(go.name);
            GameObject[] gos = GameObject.FindObjectsOfType<Transform>().Where(s => s.gameObject.name.StartsWith(nameToFind)).Select(s => s.gameObject).ToArray();
            Undo.RecordObjects(gos, "Names changed");
            gos.ForEach(s => s.name = CleanNumberName(s.name));
            Debug.Log($"{gos.Length} names changed");
        }
        Unityx.SetSceneDirty();
    }
    //[MenuItem("GameObject/Refactorx/GetGameObjectType", false, 0)]
    //public static void GetGameObjectType()
    //{
    //    GameObject go = Selection.activeGameObject;
    //    if (go == null) return;
    //    try
    //    {
    //        Debug.Log(PrefabUtility.GetPrefabAssetType(PrefabUtility.GetOutermostPrefabInstanceRoot(go)).ToString());
    //    }
    //    catch { }
    //}

    [MenuItem("Assets/Ex functions/Replace model in scene with Prefab", false, 0)]
    public static void ReplaceModelWithPrefab()
    {
        Object obj = Selection.activeObject;
        if (obj == null) return;
        switch (PrefabUtility.GetPrefabAssetType(obj))
        {
            case PrefabAssetType.Model:
            case PrefabAssetType.MissingAsset:
            case PrefabAssetType.NotAPrefab:
                return;
        }
        GameObject prefab = (GameObject)obj;
        GameObject[] gos = GameObject.FindObjectsOfType<Transform>().Where(s => s.gameObject.name.StartsWith(obj.name) && PrefabUtility.GetPrefabAssetType(s.gameObject) == PrefabAssetType.Model).Select(s => s.gameObject).ToArray();

        foreach (GameObject model in gos)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, model.transform.parent);
            instance.transform.SetPositionAndRotation(model.transform.position, model.transform.rotation);
            Undo.RegisterCreatedObjectUndo(instance, "Replaced GameObject");
            Undo.DestroyObjectImmediate(model);
        }
        Unityx.SetSceneDirty();
        //AEDataEditor.fi
    }

    static bool IsModel(GameObject go)
    {
        if (go == null) return false;
        GameObject model = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
        if (model == null) return false;
        return PrefabUtility.GetPrefabAssetType(model) == PrefabAssetType.Model;
    }
    static string CleanNumberName(string name)
    {
        if (name.Trim().EndsWith(")"))
        {
            return name.Remove(name.LastIndexOf(" ("));
        }
        return name;
    }
#endif
    #endregion
}
