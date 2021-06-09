using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Destroy all childrens
    /// </summary>
    public static void Clear(this Transform t)
    {
        while (t.childCount > 0)
        {
            GameObject.DestroyImmediate(t.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// Migrate childs.
    /// </summary>
    /// <param name="t">T.</param>
    /// <param name="target">Target.</param>
    public static void Migrate(this Transform t, Transform target)
    {
        while (t.childCount > 0)
        {
            t.GetChild(0).parent = t;
        }
    }

    /// <summary>
    /// Finds all childrens with name
    /// </summary>
    /// <returns>The all.</returns> 
    public static List<GameObject> FindAll(this Transform t, string name)
    {
        List<GameObject> lGos = new List<GameObject>();
        foreach (Transform tc in t)
        {
            if (tc.name == name)
            {
                lGos.Add(tc.gameObject);
            }
            else
            {
                List<GameObject> lGOsChild = tc.FindAll(name);
                if (lGOsChild != null && lGOsChild.Count > 0)
                {
                    lGos.AddRange(lGOsChild);
                }
            }
        }
        return lGos;
    }

    /// <summary>
    /// Gets the childs.
    /// </summary>
    public static List<Transform> GetChilds(this Transform t)
    {
        List<Transform> lGos = new List<Transform>();
        foreach (Transform tc in t)
        {
            lGos.Add(tc);
        }
        return lGos;
    }

    /// <summary>
    /// Gets the childs and sub childs.
    /// </summary>
    public static List<Transform> GetChildsAll(this Transform t)
    {
        List<Transform> lts = new List<Transform>();
        GetChildsAll(t, ref lts);
        return lts;
    }

    static void GetChildsAll(Transform transform, ref List<Transform> ltransform)
    {
        foreach (Transform t in transform)
        {
            ltransform.Add(t);
            GetChildsAll(t, ref ltransform);
        }
    }

    /// <summary>
    /// Gets all parents.
    /// </summary>
    public static List<Transform> GetParents(this Transform transform)
    {
        List<Transform> lcomp = new List<Transform>();
        Transform curTransform = transform.parent;

        while (curTransform != null)
        {
            lcomp.Add(curTransform);
            curTransform = curTransform.parent;
        }
        return lcomp;
    }

    public static Transform FindChildByTag(this Transform transform, string tag)
    {
        foreach (Transform t in GetRecursiveChilds(transform))
        {
            if (t.tag == tag)
            {
                return t;
            }
        }
        return null;
    }

    public static IEnumerable<Transform> FindChildsByTag(this Transform transform, string tag)
    {
        foreach (Transform t in GetRecursiveChilds(transform))
        {
            if (t.tag == tag)
            {
                yield return t;
            }
        }
    }

    public static bool ExistChildByTag(this Transform transform, string tag)
    {
        foreach (Transform t in GetRecursiveChilds(transform))
        {
            if (t.tag == tag)
            {
                return true;
            }
        }
        return false;
    }

    public static IEnumerable<Transform> FindChildsByTagStartsWith(this Transform transform, string tag)
    {
        foreach (Transform t in GetRecursiveChilds(transform))
        {
            if (t.tag.StartsWith(tag))
            {
                yield return t;
            }
        }
    }

    public static IEnumerable<Transform> GetRecursiveChilds(this Transform parent)
    {
        Transform t;
        for (int x = 0; x < parent.childCount; x++)
        {
            t = parent.GetChild(x);
            yield return t;
            if (t.childCount > 0)
            {
                foreach (Transform tchild in GetRecursiveChilds(t))
                    yield return tchild;
            }
        }
    }

    public static List<Transform> GetChildsByTag(this Transform transform, string tag)
    {
        List<Transform> lt = new List<Transform>();
        foreach (Transform t in GetRecursiveChilds(transform))
        {
            if (t.tag == tag)
            {
                lt.Add(t);
            }
        }
        return lt;
    }

    public static List<Transform> GetChildsByTagStartsWith(this Transform transform, string tag)
    {
        List<Transform> lt = new List<Transform>();
        foreach (Transform t in GetRecursiveChilds(transform))
        {
            if (t.tag == tag)
            {
                lt.Add(t);
            }
        }
        return lt;
    }

    public static Transform GetChildsByNameAndTag(this Transform transform, string name, string tag)
    {
        foreach (Transform t in GetRecursiveChilds(transform))
        {
            if (t.name == name && t.tag == tag)
            {
                return t;
            }
        }
        return null;
    }

}
