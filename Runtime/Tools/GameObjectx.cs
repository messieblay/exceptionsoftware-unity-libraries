using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameObjectx
{
    public static void DestroyChildren(this GameObject go, Func<Transform, bool> condition)
    {
        for (int i = go.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = go.transform.GetChild(i);
            if (condition(child))
            {
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }
    }

    public static void DestroyChildren(this Transform cmp, Func<Transform, bool> condition)
    {
        for (int i = cmp.childCount - 1; i >= 0; i--)
        {
            Transform child = cmp.GetChild(i);
            if (condition(child))
            {
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }
    }

    public static void DestroyChildren(this Component cmp, Func<Transform, bool> condition)
    {
        for (int i = cmp.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = cmp.transform.GetChild(i);
            if (condition(child))
            {
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }
    }

    public static void DestroyChildren(this GameObject go)
    {
        while (go.transform.childCount > 0)
        {
            UnityEngine.Object.DestroyImmediate(go.transform.GetChild(0).gameObject);
        }
    }

    public static void DestroyChildren(this Transform cmp)
    {
        while (cmp.childCount > 0)
        {
            UnityEngine.Object.DestroyImmediate(cmp.GetChild(0).gameObject);
        }
    }

    public static void DestroyChildren(this Component cmp)
    {
        while (cmp.transform.childCount > 0)
        {
            UnityEngine.Object.DestroyImmediate(cmp.transform.GetChild(0).gameObject);
        }
    }

    public static IEnumerable<Transform> GetChildren(this Component cmp)
    {
        Transform transform = cmp.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            yield return transform.GetChild(i);
        }
        yield break;
    }

    public static IEnumerable<Transform> GetChildren(this GameObject go)
    {
        Transform transform = go.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            yield return transform.GetChild(i);
        }
        yield break;
    }

    public static IEnumerable<GameObject> GetAllChildren(this GameObject go)
    {
        Transform transform = go.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            yield return child.gameObject;
            foreach (GameObject current in child.gameObject.GetAllChildren())
            {
                yield return current;
            }
        }
        yield break;
    }

    public static IEnumerable<GameObject> GetAllChildrenAndThis(this GameObject go)
    {
        yield return go;
        foreach (GameObject current in go.GetAllChildren())
        {
            yield return current;
        }
        yield break;
    }

    public static void EnumAllChildren(this GameObject go, Action<GameObject> onChild)
    {
        Transform transform = go.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            onChild(child.gameObject);
            child.gameObject.EnumAllChildren(onChild);
        }
    }

    public static void EnumAllChildrenAndThis(this GameObject go, Action<GameObject> onChild)
    {
        onChild(go);
        go.EnumAllChildren(onChild);
    }

    public static int GetTotalChildCount(this GameObject go)
    {
        Transform transform = go.transform;
        int num = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            num += transform.GetChild(i).GetTotalChildCount();
        }
        return num;
    }

    public static int GetTotalChildCount(this Transform tr)
    {
        int num = tr.childCount;
        for (int i = 0; i < tr.childCount; i++)
        {
            num += tr.GetChild(i).GetTotalChildCount();
        }
        return num;
    }

    public static string GetFullName(this Component cmp)
    {
        string text = '/' + cmp.name;
        Transform parent = cmp.transform.parent;
        while (parent)
        {
            text = '/' + parent.name + text;
            parent = parent.parent;
        }
        return text;
    }

    public static string GetFullName(this GameObject go)
    {
        string text = '/' + go.name;
        Transform parent = go.transform.parent;
        while (parent)
        {
            text = '/' + parent.name + text;
            parent = parent.parent;
        }
        return text;
    }

    public static void ApplyLayerRecursively(this Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform root2 in root)
        {
            root2.ApplyLayerRecursively(layer);
        }
    }

    public static void ApplyLayerRecursively(this GameObject root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform root2 in root.transform)
        {
            root2.ApplyLayerRecursively(layer);
        }
    }

    public static float CalcBoundingSphereRadius(this Component cmp)
    {
        return cmp.gameObject.CalcBoundingSphereRadius();
    }

    public static float CalcBoundingSphereRadius(this GameObject go)
    {
        float num = 0f;
        Vector3 position = go.transform.position;
        MeshFilter[] componentsInChildren = go.GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            MeshFilter meshFilter = componentsInChildren[i];
            Transform transform = meshFilter.transform;
            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            for (int j = 0; j < vertices.Length; j++)
            {
                Vector3 position2 = vertices[j];
                Vector3 b = transform.TransformPoint(position2);
                float num2 = Vector3.SqrMagnitude(position - b);
                if (num2 > num)
                {
                    num = num2;
                }
            }
        }
        return Mathf.Sqrt(num);
    }

    public static Bounds? CalcBounds(this GameObject go, bool byColliders, string ignoreTag = null)
    {
        return go.transform.CalcBounds(byColliders, ignoreTag);
    }

    public static Bounds? CalcBounds(this Transform tr, bool byColliders, string ignoreTag = null)
    {
        Bounds? result;
        if (!string.IsNullOrEmpty(ignoreTag) && tr.CompareTag(ignoreTag))
        {
            result = null;
        }
        else
        {
            Bounds value = default(Bounds);
            bool flag = true;
            if (byColliders)
            {
                foreach (Collider current in from c in tr.GetComponents<Collider>()
                                             where c.enabled
                                             select c)
                {
                    if (flag)
                    {
                        value = current.bounds;
                        flag = false;
                    }
                    else
                    {
                        value.Encapsulate(current.bounds);
                    }
                }
            }
            else
            {
                Renderer component = tr.GetComponent<Renderer>();
                if (component != null && component.enabled)
                {
                    value = component.bounds;
                    flag = false;
                }
            }
            for (int i = 0; i < tr.childCount; i++)
            {
                Transform child = tr.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    Bounds? bounds = child.CalcBounds(byColliders, ignoreTag);
                    if (bounds.HasValue)
                    {
                        if (flag)
                        {
                            value = bounds.Value;
                            flag = false;
                        }
                        else
                        {
                            value.Encapsulate(bounds.Value);
                        }
                    }
                }
            }
            result = (flag ? null : new Bounds?(value));
        }
        return result;
    }


    public static Transform FindTransform(this Transform parent, string name)
    {
        if (parent.name.Equals(name)) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindTransform(child, name);
            if (result != null) return result;
        }
        return null;
    }
    public static Transform FindTransformEndsWith(this Transform parent, string name)
    {
        if (parent.name.EndsWith(name)) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindTransformEndsWith(child, name);
            if (result != null) return result;
        }
        return null;
    }
    public static List<Transform> FindAllTransformEndsWith(this Transform parent, string name) => parent.GetComponentsInChildren<Transform>().Where(s => s.name.EndsWith(name)).ToList();

    //public static IEnumerable<Transform> FindAllTransformEndsWith(this Transform parent, string name)
    //{
    //    if (parent.name.EndsWith(name)) yield return parent;
    //    foreach (Transform child in parent)
    //    {
    //        FindAllTransformEndsWith(child, name);
    //    }
    //}

    public static Transform FindTransformContains(this Transform parent, string name)
    {
        if (parent.name.Contains(name)) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindTransformContains(child, name);
            if (result != null) return result;
        }
        return null;
    }

    #region Multi Scene
    public static List<T> FindObjectsOfTypeInScene<T>(Scene scene, bool includeInactive = true) => scene.GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<T>(includeInactive)).Distinct().ToList();
    public static List<T> FindObjectsOfTypeInActiveScene<T>(bool includeInactive = true) => FindObjectsOfTypeInScene<T>(SceneManager.GetActiveScene(), includeInactive);
    public static List<T> FindObjectsOfTypeAllScenes<T>(bool includeInactive = true) => SceneManager.GetAllScenes().Where(s => s.isLoaded).SelectMany(s => s.GetRootGameObjects()).SelectMany(g => g.GetComponentsInChildren<T>(includeInactive)).ToList();

#if UNITY_EDITOR
    public static List<T> FindObjectsOfTypeAllScenesEditor<T>(bool includeInactive = true) => EditorSceneManager.GetAllScenes().Where(s => s.isLoaded).SelectMany(s => s.GetRootGameObjects()).SelectMany(g => g.GetComponentsInChildren<T>(includeInactive)).ToList();
#endif

    public static GameObject FindRootObjectOrCreateInActiveScene(string name, int sibling = -1) => FindRootObjectOrCreate(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), name, sibling);
    public static GameObject FindRootObjectOrCreate(Scene scene, string name, int sibling = -1)
    {
        GameObject go = scene.GetRootGameObjects().Where(s => s.name.ToUpper() == name.ToUpper()).FirstOrDefault();
        go = go ?? new GameObject(name);
        if (sibling >= 0)
        {
            go.transform.SetSiblingIndex(sibling);
        }
        return go;
    }

    public static T FindRootObjectOrCreateInActiveScene<T>(string name, int sibling = -1) where T : MonoBehaviour => FindRootObjectOrCreate<T>(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), name, sibling);
    public static T FindRootObjectOrCreate<T>(Scene scene, string name, int sibling = -1) where T : MonoBehaviour
    {
        GameObject go = scene.GetRootGameObjects().Where(s => s.GetComponent<T>() != null && s.name.ToUpper() == name.ToUpper()).FirstOrDefault();
        go = go ?? new GameObject(name);
        go.TryAddComponent<T>();
        if (sibling >= 0)
        {
            go.transform.SetSiblingIndex(sibling);
        }
        return go.GetComponent<T>();
    }
    #endregion


    #region Coroutines

    public static void TryStopRoutine(this Coroutine coroutine, MonoBehaviour target)
    {
        if (coroutine == null) return;
        target.StopCoroutine(coroutine);
        coroutine = null;
    }

    #endregion
}
