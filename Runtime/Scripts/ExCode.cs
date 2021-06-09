using System;
using System.Reflection;
using System.Text;
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
