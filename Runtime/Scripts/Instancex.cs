using UnityEngine;
using System.Collections;

public static class Instancex
{

    public static GameObject Create(string name) => new GameObject(name);

    public static GameObject Create(string name, Transform parent)
    {
        GameObject go = Create(name);
        go.transform.parent = parent;
        go.transform.localPosition = Vector3.zero;
        return go;
    }

    public static Transform CreateTransform(string name) => Create(name).transform;

    public static Transform CreateTransform(string name, Transform parent)
    {
        Transform t = Create(name).transform;
        t.parent = parent;
        t.localPosition = Vector3.zero;
        return t;
    }

    public static T Create<T>(string name) where T : Component => Create(name).AddComponent<T>();

    public static T Create<T>(string name, Transform parent) where T : Component
    {
        T v = Create(name).AddComponent<T>();
        v.transform.SetParent(parent, false);
        return v;
    }

    public static GameObject TryCreate(string name, Transform parent)
    {
        Transform t = TryCreateTransform(name, parent);
        if (t == null)
        {
            return null;
        }
        return t.gameObject;
    }

    public static Transform TryCreateTransform(string name, Transform parent)
    {
        if (parent != null && parent.Find(name) == null)
        {
            return null;
        }
        Transform t = Create(name).transform;
        t.parent = parent;
        t.localPosition = Vector3.zero;
        return t;
    }

    public static T Singleton<T>() where T : Component => Singleton<T>(typeof(T).Name);

    public static T Singleton<T>(string name) where T : Component
    {
        T single = GameObject.FindObjectOfType<T>();
        if (single == null)
        {
            single = Create(name).AddComponent<T>();
        }
        return single;
    }

    public static T Copy<T>(T so) where T : ScriptableObject => GameObject.Instantiate(so) as T;


    #region Instantiate

    public static GameObject InstantiatePrefab(this GameObject g)
    {
        if (g == null)
        {
            return null;
        }

#if UNITY_EDITOR
        return UnityEditor.PrefabUtility.InstantiatePrefab(g) as GameObject;
#else
        return GameObject.Instantiate (g) as GameObject;
#endif
    }


    public static GameObject Instantiate(this GameObject g) => g == null ? null : GameObject.Instantiate(g) as GameObject;
    public static GameObject Name(this GameObject g, string name)
    {
        if (g == null)
        {
            return null;
        }
        g.name = name;
        return g;
    }

    public static GameObject InstantiatePrefab(this GameObject g, Transform parent)
    {
        GameObject prefab = g.InstantiatePrefab().Parent(parent);
        return prefab;
    }

    public static GameObject LocalPosition(this GameObject g, string name)
    {
        g.name = name;
        return g;
    }

    public static GameObject LocalPosition(this GameObject g, Vector3 localPosition)
    {
        g.transform.localPosition = localPosition;
        return g;
    }

    public static GameObject Position(this GameObject g, Vector3 position)
    {
        g.transform.position = position;
        return g;
    }

    public static GameObject LocalRotation(this GameObject g, Quaternion localRot)
    {
        g.transform.localRotation = localRot;
        return g;
    }

    public static GameObject Rotation(this GameObject g, Quaternion rot)
    {
        g.transform.rotation = rot;
        return g;
    }

    public static GameObject LocalRotation(this GameObject g, Vector3 localRot)
    {
        g.transform.localRotation = Quaternion.Euler(localRot);
        return g;
    }

    public static GameObject Rotation(this GameObject g, Vector3 rot)
    {
        g.transform.rotation = Quaternion.Euler(rot);
        return g;
    }

    public static GameObject LocalScale(this GameObject g, Vector3 localScale)
    {
        g.transform.localScale = localScale;
        return g;
    }

    public static GameObject Parent(this GameObject g, Transform parent, bool worldPositionStays = false)
    {
        g.transform.SetParent(parent, false);
        return g;
    }

    public static GameObject MoveLocalPosition(this GameObject g, Vector3 localPosition)
    {
        g.transform.localPosition = g.transform.localPosition.Move(localPosition);
        return g;
    }

    public static GameObject MovePosition(this GameObject g, Vector3 position)
    {
        g.transform.position = g.transform.position.Move(position);
        return g;
    }

    #endregion
}
