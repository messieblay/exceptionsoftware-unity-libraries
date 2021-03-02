using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Editorx
{
    static Dictionary<Component, Editor> _cachedEditors = new Dictionary<Component, Editor>();
    public static Editor CreateEditor(Component comp)
    {
        Editor editor = null;
        if (_cachedEditors.TryGetValue(comp, out editor))
        {
            if (editor)
            {
                return editor;
            }
            _cachedEditors.Remove(comp);
        }

        editor = Editor.CreateEditor(comp);
        _cachedEditors.Add(comp, editor);
        return editor;
    }

}
