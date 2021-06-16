using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CollectionsExtensions
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
    public static List<T> ClearAddRange<T>(this List<T> list, IEnumerable<T> items)
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
    public static bool Empty<T>(this List<T> list) => list == null || list.Count == 0;

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
        if (list == null) return null;
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
    public static List<T> AddRangeUnique<T>(this List<T> list, IEnumerable<T> item)
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
    public static bool IsNullOrEmpty<T>(this T[] array) => array == null || array.Length == 0;

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
    public static string[] ToStringArray<T>(this T[] list) => list.Select(s => s.ToString()).ToArray();

    /// <summary>
    /// Convert listo to string array
    /// </summary>
    public static List<string> ToStringList<T>(this T[] list) => list.Select(s => s.ToString()).ToList();


    /// <summary>
    /// Convert ArrayList to GUIContent
    /// </summary>
    public static GUIContent[] ToGUIContent(this string[] list) => list.Select(s => new GUIContent(s.ToString())).ToArray();

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

}
