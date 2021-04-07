using UnityEditor;
using UnityEngine;
namespace ExSoftware.ExEditor
{
    public static class ExPreferences
    {

        public static void TryGetColor(string key, out Color color)
        {
            if (!EditorPrefs.HasKey(key)) SetColor(key, Color.white.SetA(.5f));
            ColorUtility.TryParseHtmlString("#" + EditorPrefs.GetString(key), out color);
        }

        public static void TryGetFloat(string key, out float val)
        {
            if (!EditorPrefs.HasKey(key)) SetFloat(key, 0);
            val = EditorPrefs.GetFloat(key);
        }

        public static void TryGetInt(string key, out int val)
        {
            if (!EditorPrefs.HasKey(key)) SetInt(key, 0);
            val = EditorPrefs.GetInt(key);
        }

        public static void TryGetBool(string key, out bool val)
        {
            if (!EditorPrefs.HasKey(key)) SetBool(key, false);
            val = EditorPrefs.GetBool(key);
        }

        public static void TryGetString(string key, out string val)
        {
            if (!EditorPrefs.HasKey(key)) SetString(key, "");
            val = EditorPrefs.GetString(key);
        }

        public static string GetString(string key) => EditorPrefs.GetString(key);
        public static void SetColor(string key, Color color) => EditorPrefs.SetString(key, ColorUtility.ToHtmlStringRGBA(color));
        public static void SetFloat(string key, float val) => EditorPrefs.SetFloat(key, val);
        public static void SetInt(string key, int val) => EditorPrefs.SetInt(key, val);
        public static void SetBool(string key, bool val) => EditorPrefs.SetBool(key, val);
        public static void SetString(string key, string val) => EditorPrefs.SetString(key, val);
        public static void DeleteKey(string key) => EditorPrefs.DeleteKey(key);
        public static bool ExistKey(string key) => EditorPrefs.HasKey(key);
    }
}
