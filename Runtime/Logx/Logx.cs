using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class Logx
{
    public static string LINE_SEPARATOR = "-----------------------------------";
    public static string INTRO = "\n";
    public static string TAB = "\t";
    public static string PREFERENCES_KEY = "logx_labels";

    public static string CurrentClass
    {
        get
        {
            var st = new System.Diagnostics.StackTrace();

            var index = Mathf.Min(st.FrameCount - 1, 2);
            if (index < 0)
                return "{NoClass}";

            return st.GetFrame(index).GetMethod().DeclaringType.Name + " -> ";
        }
    }

    #region Normal

    public static void Log(string Msg, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Log, Msg, label, showInUnityConsole);
    }

    public static void Log(string msg, Color color, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Log, msg, color, label, showInUnityConsole);
    }

    public static void Log(string[] msgs, Color[] colors, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Log, msgs, colors, label, showInUnityConsole);
    }




    public static void Log(string Msg, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Log, Msg, label.ToString(), showInUnityConsole);
    }

    public static void Log(string msg, Color color, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Log, msg, color, label.ToString(), showInUnityConsole);
    }

    public static void Log(string[] msgs, Color[] colors, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Log, msgs, colors, label.ToString(), showInUnityConsole);
    }

    #endregion

    #region Warning

    public static void LogWarning(string Msg, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Warning, Msg, label, showInUnityConsole);
    }

    public static void LogWarning(string msg, Color color, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Warning, msg, color, label, showInUnityConsole);
    }

    public static void LogWarning(string[] msgs, Color[] colors, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Warning, msgs, colors, label, showInUnityConsole);
    }




    public static void LogWarning(string Msg, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Warning, Msg, label.ToString(), showInUnityConsole);
    }

    public static void LogWarning(string msg, Color color, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Warning, msg, color, label.ToString(), showInUnityConsole);
    }

    public static void LogWarning(string[] msgs, Color[] colors, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Warning, msgs, colors, label.ToString(), showInUnityConsole);
    }

    #endregion

    #region Error

    public static void LogError(string Msg, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Error, Msg, label, showInUnityConsole);
    }

    public static void LogError(string msg, Color color, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Error, msg, color, label, showInUnityConsole);
    }

    public static void LogError(string[] msgs, Color[] colors, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Error, msgs, colors, label, showInUnityConsole);
    }

    public static void LogError(string Msg, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Error, Msg, label.ToString(), showInUnityConsole);
    }

    public static void LogError(string msg, Color color, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Error, msg, color, label.ToString(), showInUnityConsole);
    }

    public static void LogError(string[] msgs, Color[] colors, Enum label, bool showInUnityConsole = false)
    {
        Log(LogType.Error, msgs, colors, label.ToString(), showInUnityConsole);
    }



    #endregion


    #region Basic

    static string _msgInternal = "", _msgFinal = "";
    static string _logClassPattern = "{0}:{1}";
    static string _logNoClassPattern = "{0}";

    public static void Log(LogType logtype, string[] msgs, Color[] colors, string label = "Default", bool showInUnityConsole = false)
    {
        for (int x = 0; x < msgs.Length; x++)
        {
            if (x < msgs.Length)
            {
                Log(logtype, msgs[x], colors[x], label, showInUnityConsole);
            }
            else
            {
                Log(logtype, msgs[x], label, showInUnityConsole);
            }
        }
    }

    public static void Log(LogType logtype, string msg, string label = "Default", bool showInUnityConsole = false)
    {
        Log(logtype, msg, SkinColor, label, showInUnityConsole);
    }

    public static void Log(LogType logtype, string msg, Color color, string label = "Default", bool showInUnityConsole = false)
    {
        if (UnityEngine.Debug.isDebugBuild)
        {

            if (_currentIndent > 0)
            {
                msg = Indent + msg;
            }

            _msgFinal = _msgInternal = string.Format(_logNoClassPattern, RichColor(msg, color));

            AddText(_msgFinal, label, logtype);

            if (showInUnityConsole || showInUnityConsoleForced)
            {
                switch (logtype)
                {
                    case LogType.Log:

                        UnityEngine.Debug.Log(_msgFinal);
                        break;
                    case LogType.Warning:
                        UnityEngine.Debug.LogWarning(_msgFinal);
                        break;
                    case LogType.Error:
                        UnityEngine.Debug.LogError(_msgFinal);
                        break;
                }
            }
        }
    }
    public static bool showInUnityConsoleForced = false;
    #endregion

    #region Extra lines
    static Color SkinColor
    {
        get
        {

            //return EditorStyles.label.normal.textColor;
#if UNITY_EDITOR
                                    return EditorStyles.label.normal.textColor;
#else
            return Color.grey;
#endif
        }
    }


    public static void LogSeparator(Enum label, bool showInUnityConsole = false)
    {
        LogSeparator(label.ToString(), showInUnityConsole);
    }
    public static void LogSeparator(string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Log, LINE_SEPARATOR, SkinColor, label, showInUnityConsole);
    }



    public static void LogSeparator(Color color, Enum label, bool showInUnityConsole = false)
    {
        LogSeparator(color, label.ToString(), showInUnityConsole);
    }
    public static void LogSeparator(Color color, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Log, LINE_SEPARATOR, color, label, showInUnityConsole);
    }


    public static void LogTitle(string msg, Color color, Enum label, bool showInUnityConsole = false)
    {
        LogTitle(msg, color, label.ToString(), showInUnityConsole);
    }

    public static void LogTitle(string msg, Color color, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Log, LINE_SEPARATOR, color, label, showInUnityConsole);
        Log(LogType.Log, LINE_SEPARATOR + "\n" + msg + "\n" + LINE_SEPARATOR, color, label, showInUnityConsole);
        Log(LogType.Log, LINE_SEPARATOR, color, label, showInUnityConsole);
    }



    public static void LogTitle(string msg, Enum label, bool showInUnityConsole = false)
    {
        LogTitle(msg, SkinColor, label, showInUnityConsole);
    }
    public static void LogTitle(string msg, string label = "Default", bool showInUnityConsole = false)
    {
        LogTitle(msg, SkinColor, label, showInUnityConsole);
    }



    public static void LogHeader(string msg, Color color, Enum label, bool showInUnityConsole = false)
    {
        LogHeader(msg, color, label.ToString(), showInUnityConsole);
    }
    public static void LogHeader(string msg, Enum label, bool showInUnityConsole = false)
    {
        LogHeader(msg, SkinColor, label, showInUnityConsole);
    }
    public static void LogHeader(string msg, string label = "Default", bool showInUnityConsole = false)
    {
        LogHeader(msg, SkinColor, label.ToString(), showInUnityConsole);
    }
    public static void LogHeader(string msg, Color color, string label = "Default", bool showInUnityConsole = false)
    {
        Log(LogType.Log, msg + "\n" + LINE_SEPARATOR, color, label, showInUnityConsole);
        //Log(LogType.Log, LINE_SEPARATOR, color, label, showInUnityConsole);
    }



    #endregion

    #region Indent

    static int _currentIndent = 0;

    public static void IncIndent()
    {
        _currentIndent++;
    }

    public static void DecIndent()
    {
        _currentIndent = Mathf.Max(0, _currentIndent - 1);
    }

    static string Indent
    {
        get
        {
            string msg = "";
            for (int x = 0; x < _currentIndent; x++)
            {
                msg = TAB + msg;
            }
            return msg;
        }
    }


    #endregion


    #region Patterns

    static string _logColorPattern = "<color=#{1}>{0}</color>";
    static string _logBoldPattern = "<b>{0}</b>";

    public static string RichColor(string text, Color color)
    {
        return string.Format(_logColorPattern, text, ColorUtility.ToHtmlStringRGBA(color));
    }

    public static string RichBold(string text)
    {
        return string.Format(_logBoldPattern, text);
    }

    #endregion

#if UNITY_EDITOR
    [MenuItem("Window/ExSoftware/Log/Clear console")]
#endif
    public static void ClearLogConsole()
    {
#if UNITY_EDITOR
        foreach (Type t in Assembly.GetAssembly(typeof(Editor)).GetTypes())
        {
            if (t.Name.Contains("LogEntries"))
            {
                try
                {
                    t.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
                }
                catch
                {
                }
                //                  t.GetMethod ("Clear", BindingFlags.Static | BindingFlags.Public).Invoke (null, null);
            }
        }
#endif

    }


    #region Console Window
    [SerializeField] static List<LogEntry> _lEntrys = new List<LogEntry>();
    public static List<LogEntry> LEntrys { get { return _lEntrys; } }
    public static System.Action<List<LogEntry>> onEntrysAdd = null;
    public static void AddText(string text, string label, LogType logType)
    {

        string currentFile = "";
        int currentLine = 0;
        string fullstack = "";
        //  ProcessStackTrace(out fullstack, out currentFile, out currentLine);

        //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
        //currentFile = st.GetFrame(0).GetFileName();
        //currentLine = st.GetFrame(0).GetFileLineNumber();


        //string txt = string.Format("{0} [{1}:{2}]\n{3}", text, currentFile, currentLine, UnityEngine.StackTraceUtility.ExtractStackTrace());


        LogEntry entry = new LogEntry(text, _lEntrys.Count, label, logType);

        entry.st = fullstack;
        entry.currentFile = currentFile;
        entry.currentLine = currentLine;

        _lEntrys.Add(entry);
        if (_lEntrys.Count > 300)
        {
            _lEntrys.RemoveAt(0);
        }
        if (onEntrysAdd != null) onEntrysAdd(_lEntrys);
    }
    static void ProcessStackTrace(out string fullstack, out string path, out int line)
    {
        fullstack = "";
        path = "";
        line = 0;
#if UNITY_EDITOR
        try
        {
            string stack = "";
            fullstack = stack = UnityEngine.StackTraceUtility.ExtractStackTrace();
            stack = stack.Substring(stack.IndexOf("(at "));
            stack = Application.dataPath + stack.Substring(0, stack.IndexOf(")")).Replace("(at ", "").Substring(6).Replace("\\", "/");
            string[] sp = stack.Split(':');
            path = sp[0] + ":" + sp[1];
            line = int.Parse(sp[2]);
        }
        catch { }
#endif
    }

    #endregion

}
