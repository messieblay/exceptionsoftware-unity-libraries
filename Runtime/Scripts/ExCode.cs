using System;
using UnityEngine;

public static class ExCode
{


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

    #region Prim types

    /// <summary>
    /// Round float. 3 decimas .
    /// </summary>
    /// <param name="v">V.</param>
    public static float Round(this float v) => Mathf.Round(v * 1000) / 1000;

    public static float Round(this float v, int decimals)
    {
        if (decimals < 0)
        {
            return v;
        }
        float round = Mathf.Pow(10, decimals);
        return Mathf.Round(v * round) / round;
    }

    /// <summary>
    /// true  = 1
    /// false = 0
    /// </summary>
    /// <param name="boolean"></param>
    /// <returns></returns>
    public static int ToInt(this bool boolean) => boolean ? 1 : 0;

    /// <summary>
    /// false < 1 <= true
    /// </summary>
    /// <param name="boolean"></param>
    /// <returns></returns>
    public static bool ToBool(this int boolean) => boolean >= 1 ? true : false;


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
