using UnityEngine;

public class LogEntry
{
    public string originalText;
    public string text;
    public int count;
    public GUIContent content;
    public Vector2 size = Vector2.one;
    //public GUIStyle style;
    public bool active = false;
    public string msgType;
    public static string TEXT_PATTERN = " [{0}] {1}";

    //public System.Diagnostics.StackTrace st;
    public string st;
    public string currentFile;
    public int currentLine;
    public LogType logType;

    public LogEntry(string text, int count, LogType logType)
    {
        this.text = this.originalText = text;
        this.count = count;
        this.content = new GUIContent(text);
        this.logType = logType;
    }
    public LogEntry(string text, int count, string msgType, LogType logType)
    {
        this.originalText = text;
        this.text = string.Format(TEXT_PATTERN, msgType, text);
        this.count = count;
        this.content = new GUIContent(this.text);
        this.msgType = msgType;
        this.logType = logType;
    }

}
