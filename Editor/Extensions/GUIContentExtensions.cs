
using UnityEngine;

public static class GUIContentExtensions
{
    public static GUIContent SetTooltip(this GUIContent value, string tooltip)
    {
        value.tooltip = tooltip;
        return value;
    }

    public static GUIContent SetImage(this GUIContent value, Texture image)
    {
        value.image = image;
        return value;
    }

    public static GUIContent SetText(this GUIContent value, string text)
    {
        value.text = text;
        return value;
    }

}
