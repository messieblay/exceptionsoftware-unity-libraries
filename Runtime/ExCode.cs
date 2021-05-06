using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;
using System.Linq;

public static class ExCode
{
    #region Dictionarys

    public static bool TryAdd<T, K>(this Dictionary<T, K> dic, T key, K value)
    {
        if (dic.ContainsKey(key))
        {
            return false;
        }

        dic.Add(key, value);
        return true;
    }

    public static void TryAdd<T, K>(this Dictionary<T, K> dic, Dictionary<T, K> otherDic)
    {
        foreach (KeyValuePair<T, K> kp in otherDic)
        {
            TryAdd(dic, kp.Key, kp.Value);
        }
    }

    public static void TryRemoveByKey<T, K>(this Dictionary<T, K> dic, Dictionary<T, K> otherDic)
    {
        foreach (T kp in otherDic.Keys)
        {
            dic.Remove(kp);
        }
    }

    public static Dictionary<T, K> Copy<T, K>(this Dictionary<T, K> dic)
    {
        Dictionary<T, K> otherDic = new Dictionary<T, K>();
        foreach (KeyValuePair<T, K> kp in dic)
        {
            otherDic.Add(kp.Key, kp.Value);
        }
        return otherDic;
    }

    public static void TryAdd<T, K>(this Dictionary<T, K> dic, KeyValuePair<T, K> kp)
    {
        TryAdd(dic, kp.Key, kp.Value);
    }

    #endregion

    #region List

    /// <summary>
    /// Removes all elements of the list and the add one element.
    /// </summary>
    /// <returns>The add.</returns>
    /// <param name="list">List.</param>
    /// <param name="item">Item.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static List<T> ClearAdd<T>(this List<T> list, T item)
    {
        list.Clear();
        list.Add(item);
        return list;
    }

    /// <summary>
    /// Removes all elements of the list and later the add range.
    /// </summary>
    /// <returns>The add range.</returns>
    /// <param name="list">List.</param>
    /// <param name="items">Items.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static List<T> ClearAddRange<T>(this List<T> list, List<T> items)
    {
        list.Clear();
        list.AddRange(items);
        return list;
    }

    /// <summary>
    /// Clone the specified list and Objects
    /// </summary> 
    /// <returns>The list.</returns> 
    public static List<K> Cast<T, K>(this List<T> list)
    {
        List<K> res = new List<K>();
        list.ForEach(x => res.Add((K)Convert.ChangeType(x, typeof(K))));
        return res;
    }

    /// <summary>
    /// Clone the specified list and Objects
    /// </summary> 
    /// <returns>The list.</returns> 
    public static List<T> Clone<T>(this List<T> list) where T : ICloneable
    {
        List<T> res = new List<T>();
        list.ForEach(x => res.Add((T)x.Clone()));
        return res;
    }

    /// <summary>
    /// Clones the list. 
    /// </summary>
    /// <returns>The list.</returns> 
    public static List<T> CloneList<T>(this List<T> listToClone)
    {
        List<T> res = new List<T>();
        res.AddRange(listToClone);
        return res;
    }

    /// <summary>
    /// Determines if is null or empty the specified list.
    /// </summary>
    /// <returns><c>true</c> if is null or empty the specified list; otherwise, <c>false</c>.</returns> 
    public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;

    /// <summary>
    /// Empty the specified list.
    /// </summary> 
    public static bool Empty<T>(this List<T> list) => list.Count == 0;

    /// <summary>
    /// Counts the real.
    /// </summary>
    /// <returns>List.Count - 1</returns> 
    public static int CountReal<T>(this List<T> list)
    {
        return list.Count - 1;
    }

    /// <summary>
    /// Get Last item of the specified list.
    /// </summary>
    public static T Last<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            return default(T);
        }
        return list[list.Count - 1];
    }

    /// <summary>
    /// Get First item of the specified list.
    /// </summary>
    public static T First<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            return default(T);
        }
        return list[0];
    }

    /// <summary>
    /// Add item in index=0
    /// </summary>
    public static List<T> AddFirst<T>(this List<T> list, T item)
    {
        list.Insert(0, item);
        return list;
    }

    /// <summary>
    /// Get random item from list
    /// </summary>
    public static T Random<T>(this List<T> list)
    {
        if (list.Empty())
        {
            return default(T);
        }
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Removes all the nulls of the list
    /// </summary>
    /// <returns>List without nulls</returns>
    public static List<T> ClearNulls<T>(this List<T> list)
    {
        int x = 0;
        while (x < list.Count)
        {
            if (list[x] != null)
            {
                x++;
                continue;
            }
            list.RemoveAt(x);
        }
        return list;
    }
    /// <summary>
    /// Adds item if not repeated
    /// </summary>
    public static bool TryAddUnique<T>(this List<T> list, T item)
    {
        if (list.Contains(item))
        {
            return false;
        }
        list.Add(item);
        return true;
    }

    /// <summary>
    /// Adds item if not repeated
    /// </summary>
    public static List<T> AddUnique<T>(this List<T> list, T item)
    {
        if (list.Contains(item))
        {
            return list;
        }
        list.Add(item);
        return list;
    }


    /// <summary>
    /// Adds list if not repeated
    /// </summary>
    public static List<T> AddRangeUnique<T>(this List<T> list, List<T> item)
    {
        item.ForEach(i => list.AddUnique(i));
        return list;
    }

    /// <summary>
    /// Switch two items
    /// </summary>
    public static List<T> Switch<T>(this List<T> list, int a, int b)
    {
        if (list.ExistIndex(a) && list.ExistIndex(b))
        {
            T c = list[a];
            list[a] = list[b];
            list[b] = c;
        }
        return list;
    }

    /// <summary>
    /// Exist index in list
    /// </summary>
    public static bool ExistIndex<T>(this List<T> list, int a) => 0 <= a && a < list.Count;


    /// <summary>
    /// Convert listo to string array
    /// </summary>
    public static string[] ToStringArray<T>(this List<T> list)
    {
        string[] newList = new string[list.Count];
        for (int x = 0; x < list.Count; x++)
        {
            newList[x] = list[x].ToString();
        }
        return newList;
    }

    /// <summary>
    /// Convert listo to string array
    /// </summary>
    public static List<string> ToStringList<T>(this List<T> list)
    {
        List<string> newList = new List<string>();
        for (int x = 0; x < list.Count; x++)
        {
            newList.Add(list[x].ToString());
        }
        return newList;
    }

    /// <summary>
    /// Add the specified list and values.
    /// </summary>
    /// <param name="list">List.</param>
    /// <param name="values">Values.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static List<T> Add<T>(this List<T> list, params T[] values)
    {
        foreach (T t in values)
        {
            list.Add(t);
        }
        return list;
    }

    public static List<T> AddParam<T>(this List<T> list, params T[] values)
    {
        foreach (T t in values)
        {
            list.Add(t);
        }
        return list;
    }

    #endregion

    #region Arrays


    public static T[] Clear<T>(this T[] array)
    {
#if UNITY_EDITOR
        UnityEditor.ArrayUtility.Clear(ref array);
        return array;
#else
        List<T> l = array.ToList();
        l.Clear();
        return l.ToArray();
#endif
    }

    public static T[] Add<T>(this T[] array, T item)
    {
#if UNITY_EDITOR
        UnityEditor.ArrayUtility.Add(ref array, item);
        return array;
#else
        //      List<T> l = array.ToList ();
        //      l.Clear ();
        //      return l.ToArray ();

        T[] array2 = new T[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
        {
            array2[i] = array[i];
        }
        array2[array.Length] = item;
        return array2;
#endif
    }

    public static T[] AddRange<T>(this T[] array, T[] item)
    {
#if UNITY_EDITOR
        UnityEditor.ArrayUtility.AddRange(ref array, item);
        return array;
#else
        List<T> l = array.ToList();
        l.AddRange(item);
        return l.ToArray();
#endif
    }

    public static T[] Remove<T>(this T[] array, T item)
    {
#if UNITY_EDITOR
        UnityEditor.ArrayUtility.Remove(ref array, item);
        return array;
#else
        List<T> l = array.ToList();
        l.Remove(item);
        return l.ToArray();
#endif
    }

    public static T[] RemoveAt<T>(this T[] array, int index)
    {
#if UNITY_EDITOR
        UnityEditor.ArrayUtility.RemoveAt(ref array, index);
        return array;
#else

        T[] array2 = new T[array.Length - 1];
        for (int i = 0; i < index; i++)
        {
            array2[i] = array[i];
        }
        for (int i = index; i < array2.Length; i++)
        {
            array2[i] = array[i + 1];
        }
        return array2;
#endif
    }

    public static T[] Insert<T>(this T[] array, int index, T item)
    {
#if UNITY_EDITOR
        UnityEditor.ArrayUtility.Insert(ref array, index, item);
        return array;
#else
        List<T> l = array.ToList();
        l.Insert(index, item);
        return l.ToArray();
#endif
    }

    public static List<T> ToList<T>(this T[] arrayToClone)
    {
        List<T> res = new List<T>();
        res.AddRange(arrayToClone);

        return res;
    }

    public static T Last<T>(this T[] list)
    {
        if (list == null || list.Length == 0)
        {
            return default(T);
        }
        return list[list.Length - 1];
    }

    public static T First<T>(this T[] list)
    {
        if (list == null || list.Length == 0)
        {
            return default(T);
        }
        return list[0];
    }

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
        }
    }

    public static int IndexOf<T>(this T[] array, T value) => Array.IndexOf<T>(array, value);
    public static void MoveBack<T>(this T[] array, T value)
    {
        int num = Array.IndexOf<T>(array, value);
        if (num > 0)
        {
            T t = array[num - 1];
            array[num - 1] = value;
            array[num] = t;
        }
    }

    public static void MoveForw<T>(this T[] array, T value)
    {
        int num = Array.IndexOf<T>(array, value);
        if (num < array.Length - 1 && num >= 0)
        {
            T t = array[num + 1];
            array[num + 1] = value;
            array[num] = t;
        }
    }

    public static bool CanMoveBack<T>(this T[] array, T value) => Array.IndexOf<T>(array, value) > 0;

    public static bool CanMoveForw<T>(this T[] array, T value)
    {
        int num = Array.IndexOf<T>(array, value);
        return num < array.Length - 1 && num >= 0;
    }

    /// <summary>
    /// Switch two items
    /// </summary>
    public static T[] Switch<T>(this T[] list, int a, int b)
    {
        if (list.ExistIndex(a) && list.ExistIndex(b))
        {
            T c = list[a];
            list[a] = list[b];
            list[b] = c;
        }
        return list;
    }

    /// <summary>
    /// Exist index in list
    /// </summary>
    public static bool ExistIndex<T>(this T[] list, int a) => 0 <= a && a < list.Length;

    /// <summary>
    /// Convert listo to string array
    /// </summary>
    public static string[] ToStringArray<T>(this T[] list)
    {
        string[] newList = new string[list.Length];
        for (int x = 0; x < list.Length; x++)
        {
            newList[x] = list[x].ToString();
        }
        return newList;
    }

    /// <summary>
    /// Convert listo to string array
    /// </summary>
    public static List<string> ToStringList<T>(this T[] list)
    {
        List<string> newList = new List<string>();
        for (int x = 0; x < list.Length; x++)
        {
            newList.Add(list[x].ToString());
        }
        return newList;
    }


    /// <summary>
    /// Convert ArrayList to GUIContent
    /// </summary>
    public static GUIContent[] ToGUIContent(this string[] list)
    {
        GUIContent[] newList = new GUIContent[list.Length];
        for (int x = 0; x < list.Length; x++)
        {
            newList[x] = new GUIContent(list[x]);
        }
        return newList;
    }

    /// <summary>
    /// Convert ArrayList to GUIContent
    /// </summary>
    public static GUIContent[] ToGUIContent<T>()
    {
        try
        {
            string[] enumNames = System.Enum.GetNames(typeof(T));
            GUIContent[] newList = new GUIContent[enumNames.Length];
            for (int x = 0; x < enumNames.Length; x++)
            {
                newList[x] = new GUIContent(enumNames[x]);
            }
            return newList;
        }
        catch
        {
            return null;
        }
    }


    //  static UnityEngine.Random _rng = new UnityEngine.Random ();

    /// <summary>
    /// Shuffle the specified list.
    /// </summary>
    /// <param name="list">List.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Resize the specified list, sz and c.
    /// </summary>
    /// <param name="list">List.</param>
    /// <param name="sz">Size.</param>
    /// <param name="c">C.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static void Resize<T>(this List<T> list, int sz, T c)
    {
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = sz;
            list.AddRange(Enumerable.Repeat(c, sz - cur));
        }
    }

    public static void Resize<T>(this List<T> list, int sz) where T : new()
    {
        Resize(list, sz, new T());
    }

    /// <summary>
    /// Get a Sublist
    /// </summary>
    /// <returns>The list.</returns>
    /// <param name="list">List.</param>
    /// <param name="begin">Begin.</param>
    /// <param name="end">End.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static List<T> SubList<T>(this List<T> list, int begin, int end)
    {
        List<T> newList = new List<T>();
        //      if (0 <= begin && begin <= end && end < list.Count) {
        if (0 <= begin && begin <= end)
        {
            if (begin == end)
            {
                newList.Add(list[begin]);
                return newList;
            }

            for (int x = begin; x < Mathf.Min(end, list.Count); x++)
            {
                newList.Add(list[x]);
            }
        }
        return newList;

    }

    #endregion

    #region GameObject

    /// <summary>
    /// Set the static the gameobject and all childrens
    /// </summary>
    /// <param name="go">Go.</param>
    /// <param name="isStatic">If set to <c>true</c> is static.</param>
    public static void SetStatic(this GameObject go, bool isStatic)
    {
        go.isStatic = isStatic;
        if (go.transform.childCount > 0)
        {
            for (int x = 0; x < go.transform.childCount; x++)
            {
                go.transform.GetChild(x).gameObject.SetStatic(isStatic);
            }
        }
    }


    /// <summary>
    /// Set the static the gameobject and all childrens
    /// </summary>
    /// <param name="go">Go.</param>
    /// <param name="isStatic">If set to <c>true</c> is static.</param>
    public static void SetActiveRec(this GameObject go, bool isActive)
    {
        go.SetActive(isActive);
        if (go.transform.childCount > 0)
        {
            for (int x = 0; x < go.transform.childCount; x++)
            {
                go.transform.GetChild(x).gameObject.SetActiveRec(isActive);
            }
        }
    }

    #endregion

    #region Transform

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

    #endregion

    #region Min Max

    public static int Max(this int g, int max) => Mathf.Max(g, max);

    public static float Max(this float g, float max) => Mathf.Max(g, max);

    public static int Min(this int g, int min) => Mathf.Min(g, min);

    public static float Min(this float g, float min) => Mathf.Min(g, min);

    /// <summary>
    /// Set value between [min,max]
    /// Mathf.Min (max, Mathf.Max (min, g))
    /// </summary> 
    public static int Clamp(this int g, int min, int max) => Mathf.Clamp(g, min, max);

    /// <summary>
    /// Set value between [min,max]
    /// Mathf.Min (max, Mathf.Max (min, g))
    /// </summary> 
    public static float Clamp(this float g, float min, float max) => Mathf.Clamp(g, min, max);
    public static Vector3 Max(this Vector3 g, Vector3 max) => new Vector3(Mathf.Max(g.x, max.x), Mathf.Max(g.y, max.y), Mathf.Max(g.z, max.z));
    public static Vector2 Max(this Vector2 g, Vector2 max) => new Vector2(Mathf.Max(g.x, max.x), Mathf.Max(g.y, max.y));
    public static Vector3 Min(this Vector3 g, Vector3 max) => new Vector3(Mathf.Min(g.x, max.x), Mathf.Min(g.y, max.y), Mathf.Min(g.z, max.z));
    public static Vector2 Min(this Vector2 g, Vector2 max) => new Vector2(Mathf.Min(g.x, max.x), Mathf.Min(g.y, max.y));
    public static Vector3 Max(this Vector3 g, float x, float y, float z) => new Vector3(Mathf.Max(g.x, x), Mathf.Max(g.y, y), Mathf.Max(g.z, z));
    public static Vector2 Max(this Vector2 g, float x, float y) => new Vector2(Mathf.Max(g.x, x), Mathf.Max(g.y, y));
    public static Vector3 Min(this Vector3 g, float x, float y, float z) => new Vector3(Mathf.Min(g.x, x), Mathf.Min(g.y, y), Mathf.Min(g.z, z));
    public static Vector2 Min(this Vector2 g, float x, float y) => new Vector2(Mathf.Min(g.x, x), Mathf.Min(g.y, y));

    #endregion

    #region Vector

    /// <summary>
    /// Convert Vector2 to Vector3
    /// </summary>
    /// <param name="v">V.</param>
    public static Vector3 To3(this Vector2 v) => new Vector3(v.x, v.y, 0);
    /// <summary>
    /// Convert Vector2 to Vector3 (X,Y) => (X,0,Z)
    /// </summary>
    /// <param name="v">V.</param>
    public static Vector3 ToForward3D(this Vector2 v) => new Vector3(v.x, 0, v.y);
    /// <summary>
    /// Convert Vector2 to Vector3 (X,Y) => (X,0,Z)
    /// </summary>
    /// <param name="v">V.</param>
    public static Vector2 ToForward2D(this Vector3 v) => new Vector2(v.x, v.z);

    /// <summary>
    /// Move the specified vector in x, y and z.
    /// </summary>
    /// <param name="x">The x coordinate. v.x+=x;</param>
    /// <param name="y">The y coordinate. v.y+=y;</param>
    /// <param name="z">The z coordinate. v.z+=z;</param>
    public static Vector3 Move(this Vector3 v, float x, float y, float z)
    {
        v.x += x;
        v.y += y;
        v.z += z;
        return v;
    }

    /// <summary>
    /// Move the specified vector in vMove
    /// </summary>
    /// <param name="vMove">The x coordinate. v+=vMove;</param>
    public static Vector3 Move(this Vector3 v, Vector3 vMove)
    {
        v.x += vMove.x;
        v.y += vMove.y;
        v.z += vMove.z;
        return v;
    }

    /// <summary>
    /// Move the specified vector in x and y.
    /// </summary>
    /// <param name="x">The x coordinate. v.x+=x;</param>
    /// <param name="y">The y coordinate. v.y+=y;</param>
    public static Vector2 Move(this Vector2 v, float x, float y)
    {
        v.x += x;
        v.y += y;
        return v;
    }

    /// <summary>
    /// Move the specified vector in vMove
    /// </summary>
    /// <param name="vMove">The x coordinate. v+=vMove;</param>
    public static Vector2 Move(this Vector2 v, Vector2 vMove)
    {
        v.x += vMove.x;
        v.y += vMove.y;
        return v;
    }

    public static Vector2 SetX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 SetX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 SetY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 SetY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 SetZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    /// <summary>
    /// To key. x,y,z,w
    /// </summary> 
    public static string ToKey(this Vector4 v) => $"{v.x},{v.y},{v.z},{v.w}";

    /// <summary>
    /// To key. x,y,z
    /// </summary> 
    public static string ToKey(this Vector3 v) => $"{v.x},{v.y},{v.z}";

    /// <summary>
    /// To key. x,y
    /// </summary> 
    public static string ToKey(this Vector2 v) => $"{v.x},{v.y}";

    /// <summary>
    /// Determines if the specified v is Vector3.zero
    /// </summary>
    /// <returns><c>true</c> if is zero the specified v; otherwise, <c>false</c>.</returns>
    public static bool IsZero(this Vector3 v) => v.x == 0 && v.y == 0 && v.z == 0;

    /// <summary>
    /// Determines if the specified v is Vector2.zero
    /// </summary>
    /// <returns><c>true</c> if is zero the specified v; otherwise, <c>false</c>.</returns>
    public static bool IsZero(this Vector2 v) => v.x == 0 && v.y == 0;

    /// <summary>
    /// Round the specified vector with decimals.
    /// </summary>
    /// <param name="decimals">Round Decimals.</param>
    public static Vector3 Round(this Vector3 v, int decimals)
    {
        if (decimals < 0)
        {
            return v;
        }
        float round = Mathf.Pow(10, decimals);
        v.x = Mathf.Round(v.x * round) / round;
        v.y = Mathf.Round(v.y * round) / round;
        v.z = Mathf.Round(v.z * round) / round;
        return v;
    }

    /// <summary>
    /// Copy the specified v.
    /// </summary>
    public static Vector3 Copy(this Vector3 v) => new Vector3(v.x, v.y, v.z);
    public static int CompareTo(this Vector3 v, Vector3 other)
    {
        int x = v.x.CompareTo(other.x);
        int y = v.y.CompareTo(other.y);
        if (x == 0)
        {
            if (y == 0)
            {
                return v.z.CompareTo(other.z);
            }
            else
            {
                return y;
            }
        }
        else
        {
            return x;
        }
    }

    static int Vector3Compare(Vector3 v0, Vector3 v1)
    {
        int x = v0.x.CompareTo(v1.x);
        int y = v0.y.CompareTo(v1.y);

        if (x == 0)
        {
            if (y == 0)
            {
                return v0.z.CompareTo(v1.z);
            }
            else
            {
                return y;
            }
        }
        else
        {
            return x;
        }
    }

    public static Vector2 Create(int x, int y) => new Vector2(x, y);
    public static Vector3 Create(int x, int y, int z) => new Vector3(x, y, z);
    public static Vector4 Create(int x, int y, int z, int w) => new Vector4(x, y, z, w);
    public static Vector2 Create(float x, float y) => new Vector2((int)x, (int)y);
    public static Vector3 Create(float x, float y, float z) => new Vector3((int)x, (int)y, (int)z);
    public static Vector4 Create(float x, float y, float z, float w) => new Vector4((int)x, (int)y, (int)z, (int)w);

    #endregion

    #region Prim types

    /// <summary>
    /// Round float. 3 decimas .
    /// </summary>
    /// <param name="v">V.</param>
    public static float Round(this float v)
    {
        return Mathf.Round(v * 1000) / 1000;
    }

    public static float Round(this float v, int decimals)
    {
        if (decimals < 0)
        {
            return v;
        }
        float round = Mathf.Pow(10, decimals);
        return Mathf.Round(v * round) / round;
    }

    public static int ToInt(this bool boolean) => boolean ? 1 : 0;
    public static bool ToBool(this int boolean) => boolean >= 1 ? true : false;
    public static float Pow(this float value, float pow) => Mathf.Pow(value, pow);

    #endregion

    #region Color

    public static Color SetRGB(this Color c, float r, float g, float b, float a)
    {
        c.r = r;
        c.g = g;
        c.b = b;
        c.a = a;
        return c;
    }


    public static Color SetR(this Color c, float r)
    {
        c.r = r;
        return c;
    }

    public static Color SetG(this Color c, float g)
    {
        c.g = g;
        return c;
    }

    public static Color SetB(this Color c, float b)
    {
        c.b = b;
        return c;
    }

    public static Color SetA(this Color c, float a)
    {
        c.a = a;
        return c;
    }

    #endregion

    #region Distance

    public static int Distance(this int a, int b) => Mathf.Abs(b - a);
    public static float Distance(this float a, float b) => Mathf.Abs(b - a);
    public static float Distance(this Vector2 a, Vector2 b) => Vector2.Distance(a, b);
    public static float Distance(this Vector3 a, Vector3 b) => Vector3.Distance(a, b);

    #endregion

    #region UI

    public static UnityEngine.UI.Image Fade(this UnityEngine.UI.Image a, float fade)
    {
        a.color.SetA(fade);
        return a;
    }

    #endregion

    #region CopyComponent

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType())
            return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch
                {
                } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component => go.AddComponent<T>().GetCopyOf(toAdd) as T;

    #endregion

    #region String
    public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

    /// <summary>
    /// Parses the sentence.
    /// RecordadQueArturoEsUnPipa -> Recordad que arturo es un pipa
    /// </summary>
    /// <returns>The sentence.</returns>
    /// <param name="text">Text.</param>
    public static string ParseSentence(this string text)
    {

        if (string.IsNullOrEmpty(text))
            return string.Empty;
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }

    /// <summary>
    /// Tos the sentence.
    /// Recordad que arturo es un pipa -> RecordadQueArturoEsUnPipa
    /// </summary>
    /// <returns>The sentence.</returns>
    /// <param name="text">Text.</param>
    public static string ToSentence(this string text, bool forzeLower = false)
    {
        string finalString = string.Empty;

        foreach (var s in text.Trim().Split(' '))
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (i == 0)
                {
                    finalString += char.ToUpper(s[i]);
                }
                else
                {
                    if (forzeLower)
                    {
                        finalString += char.ToLower(s[i]);
                    }
                    else
                    {
                        finalString += s[i];
                    }
                }
            }
        }
        return finalString;
    }

    public static string ToSentenceOld(this string text)
    {
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (text[i - 1] == ' ' && char.IsLower(text[i]))
            {
                newText.Append(char.ToUpper(text[i]));
                continue;
            }
            newText.Append(text[i]);
        }
        return newText.ToString();
    }
    /// <summary>
    /// Normaliza una cadena de texto para guardar el fichero
    /// Quita todos los acentos y Ñ de un string
    /// </summary>
    /// <returns>The file string.</returns>
    /// <param name="text">Text.</param>
    public static string ToFileString(this string text) => QuitAccentsAndN(text);

    public static string QuitAccentsAndN(string texto)
    {
        string con = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜÑçÇ";
        string sin = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUNcC";
        for (int i = 0; i < con.Length; i++)
        {
            texto = texto.Replace(con[i], sin[i]);
        }
        return texto;
    }

    #endregion

    #region Delegate & Actions

    public static void TryInvoke(this System.Action action)
    {
        if (action == null)
        {
            return;
        }
        action();
    }

    #endregion

    #region Inputs

    static string JOY_KEY = "Joystick";

    public static KeyCode ToJoyNumber(this KeyCode code, int index)
    {
        if (index < 0)
            return code;
        string namecode = code.ToString();
        namecode = ToJoyNumber(namecode, index);
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), namecode);
    }

    public static int GetJoyNumber(this KeyCode code)
    {
        string namecode = code.ToString();
        if (namecode.StartsWith(JOY_KEY))
        {
            string part = namecode.Substring(8);
            char[] cpart = part.ToCharArray();
            if (Char.IsDigit(cpart[0]))
            {
                return int.Parse(cpart[0].ToString());
            }
        }
        return 0;
    }

    public static string ToJoyNumber(string namecode, int index)
    {
        if (namecode.StartsWith(JOY_KEY))
        {
            string part = namecode.Substring(8);
            char[] cpart = part.ToCharArray();
            if (Char.IsDigit(cpart[0]))
            {
                cpart[0] = index.ToString().ToCharArray()[0];
                part = new string(cpart);
            }
            else
            {
                part = index.ToString() + new string(cpart);
            }
            return JOY_KEY + part;
        }
        else
        {
            return namecode;
        }
    }

    #endregion

    #region Layers

    public static int GetLayer(this System.Enum layer) => UnityEngine.LayerMask.NameToLayer(layer.ToString());

    public static void SetLayerChilds(this Transform trans, System.Enum layer)
    {
        trans.gameObject.layer = GetLayer(layer);
        foreach (Transform child in trans)
            child.SetLayerChilds(layer);
    }

    #endregion

    /// <summary>
    /// Gets the path in hierarchy of GameObject.
    /// </summary>
    /// <returns>The path.</returns>
    public static string GetPath(this GameObject gameObject)
    {
        string path = "/" + gameObject.name;
        Transform transform = gameObject.transform;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = "/" + transform.gameObject.name + path;
        }
        return path;
    }

    /// <summary>
    /// Gets the path in hierarchy of Component.
    /// </summary>
    /// <returns>The path.</returns>
    public static string GetPath(this Component comp) => comp.gameObject.GetPath();

    /// <summary>
    /// Finds GameObject from Hierarchy path.
    /// </summary>
    public static GameObject FindFromHID(this string hid) => GameObject.Find(hid);

    /// <summary>
    /// Finds Component from Hierarchy path.
    /// </summary>
    public static T FindFromHID<T>(this string hid)
    {
        GameObject g = GameObject.Find(hid);
        if (g != null)
        {
            return g.GetComponent<T>();
        }
        return default(T);
    }


}
