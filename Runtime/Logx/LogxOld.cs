using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class LogxOld
{
    public static string LINE_SEPARATOR = "-----------------------------------";
    public static string INTRO = "\n";
    public static string TAB = "\t";

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

    public static void Log(string Msg, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Log, Msg, flags);
    }

    public static void Log(string msg, Color color, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Log, msg, color, flags);
    }

    public static void Log(string[] msgs, Color[] colors, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Log, msgs, colors, flags);
    }

    #endregion

    #region Warning

    public static void LogWarning(string Msg, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Warning, Msg, flags);
    }

    public static void LogWarning(string msg, Color color, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Warning, msg, color, flags);
    }

    public static void LogWarning(string[] msgs, Color[] colors, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Warning, msgs, colors, flags);
    }

    #endregion

    #region Error

    public static void LogError(string Msg, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Error, Msg, flags);
    }

    public static void LogError(string msg, Color color, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Error, msg, color, flags);
    }

    public static void LogError(string[] msgs, Color[] colors, LogOptions flags = LogOptions.None)
    {
        Log(LogType.Error, msgs, colors, flags);
    }

    #endregion


    #region Basic

    static string _msgInternal = "", _msgFinal = "";
    static string _logClassPattern = "{0}:{1}";
    static string _logNoClassPattern = "{0}";

    public static void Log(LogType logtype, string[] msgs, Color[] colors, LogOptions flags = LogOptions.None)
    {
        for (int x = 0; x < msgs.Length; x++)
        {
            if (x < msgs.Length)
            {
                Log(logtype, msgs[x], colors[x], flags.SetFlags(flags, true));
            }
            else
            {
                Log(logtype, msgs[x], flags.SetFlags(flags, true));
            }
        }
    }

    public static void Log(LogType logtype, string msg, LogOptions flags = LogOptions.None)
    {
        Log(logtype, msg, SkinColor, flags);
    }

    public static void Log(LogType logtype, string msg, Color color, LogOptions flags = LogOptions.None)
    {
        if (UnityEngine.Debug.isDebugBuild)
        {
            if (flags.IsFlagSet(LogOptions.Bold))
            {
                msg = RichBold(msg);
            }
            if (_currentIndent > 0)
            {
                msg = Indent + msg;
            }
            if (flags.IsFlagSet(LogOptions.ShowClass))
            {
                _msgFinal = _msgInternal = string.Format(_logClassPattern, CurrentClass, RichColor(msg, color));
            }
            else
            {
                _msgFinal = _msgInternal = string.Format(_logNoClassPattern, RichColor(msg, color));
            }

            if (flags.IsFlagSet(LogOptions.Delayed))
            {

                _delayLog += _msgInternal;
                _msgFinal = _delayLog;

                if (!flags.IsFlagSet(LogOptions.SameRow))
                {
                    _delayLog += "\n";
                    _msgFinal = _delayLog;
                }
                return;
            }

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

    #endregion

    #region Delay

    static string _delayLog = "";

    public static void LogDelay(string msg)
    {
        _delayLog += msg + "\n";
    }


    public static void ClearDelayed()
    {
        _delayLog = "";
    }

    public static void LogDelayed()
    {
        if (UnityEngine.Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log(_delayLog + "\n" + LINE_SEPARATOR);
            ClearDelayed();
        }
    }

    public static void LogDelayedWarning()
    {
        if (UnityEngine.Debug.isDebugBuild)
        {
            UnityEngine.Debug.LogWarning(_delayLog + "\n" + LINE_SEPARATOR);
            ClearDelayed();
        }
    }

    public static void LogDelayedError()
    {
        if (UnityEngine.Debug.isDebugBuild)
        {
            UnityEngine.Debug.LogError(_delayLog + "\n" + LINE_SEPARATOR);
            ClearDelayed();
        }
    }

    #endregion

    #region Extra lines
    static Color SkinColor
    {
        get
        {

#if UNITY_EDITOR
            return EditorStyles.label.normal.textColor;
#else
            return Color.white;
#endif

        }
    }
    public static void LogSeparator(LogOptions options = LogOptions.None)
    {
        Log(LogType.Log, LINE_SEPARATOR, SkinColor, options);
    }

    public static void LogSeparator(Color color, LogOptions options = LogOptions.None)
    {
        Log(LogType.Log, LINE_SEPARATOR, color, options);
    }


    public static void LogIntro(LogOptions options = LogOptions.None)
    {
        Log(LogType.Log, INTRO, SkinColor, options);
    }

    public static void LogTab(Color color, LogOptions options = LogOptions.None)
    {
        Log(LogType.Log, TAB, color, options);
    }


    public static void LogTitle(string msg, Color color, LogOptions options = LogOptions.None)
    {
        Log(LogType.Log, LINE_SEPARATOR, color, options);
        Log(LogType.Log, msg, color, options);
        Log(LogType.Log, LINE_SEPARATOR, color, options);
    }

    public static void LogTitle(string msg, LogOptions options = LogOptions.None)
    {
        LogTitle(msg, SkinColor, options);
    }


    public static void LogHeader(string msg, Color color, LogOptions options = LogOptions.None)
    {
        Log(LogType.Log, msg, color, options);
        Log(LogType.Log, LINE_SEPARATOR, color, options);
    }

    public static void LogHeader(string msg, LogOptions options = LogOptions.None)
    {
        LogHeader(msg, SkinColor, options);
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

}
