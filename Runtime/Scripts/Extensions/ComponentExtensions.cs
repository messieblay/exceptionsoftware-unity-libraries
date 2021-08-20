using System;
using System.Reflection;
using UnityEngine;

public static class ComponentExtensions
{
    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component => go.AddComponent<T>().GetCopyOf(toAdd) as T;

    public static void TryRemoveComponent<T>(this Component component) where T : Component => component.gameObject.TryRemoveComponent<T>();
    public static void TryRemoveComponent<T>(this GameObject gameobject) where T : Component
    {
        T c = gameobject.GetComponent<T>();
        if (c != null)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(c);
            }
            else
            {
                GameObject.DestroyImmediate(c);
            }
        }
    }

    public static T TryAddComponent<T>(this GameObject gameobject) where T : Component => gameobject.GetComponent<T>() ?? gameobject.AddComponent<T>();
    public static T TryAddComponent<T>(this Component component) where T : Component => component.gameObject.TryAddComponent<T>();
    public static T GetOrAddComponent<T>(this GameObject gameobject) where T : Component => gameobject.GetComponent<T>() ?? gameobject.AddComponent<T>();
    public static T GetOrAddComponent<T>(this Component component) where T : Component => component.gameObject.GetOrAddComponent<T>();
    public static bool HasComponent<T>(this GameObject gameobject) where T : Component => gameobject.GetComponent<T>() != null;
    public static bool HasComponent<T>(this Component component) where T : Component => component.GetComponent<T>() != null;

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


}
