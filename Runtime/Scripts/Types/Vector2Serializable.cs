using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Vector2Serializable
{
    [XmlAttribute] public float X;
    [XmlAttribute] public float Y;

    public Vector2 Vector2 => new Vector2((float)X, (float)Y);
    public Vector2Serializable() { }
    public Vector2Serializable(Vector2 vector)
    {
        X = vector.x;
        Y = vector.y;
    }

    public Vector2Serializable(float x, float y)
    {
        X = x;
        Y = y;
    }


    /// <summary>
    /// Automatic conversion from SerializableVector2 to Vector2
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector2(Vector2Serializable rValue) => new Vector2(rValue.X, rValue.Y);

    /// <summary>
    /// Automatic conversion from Vector2 to SerializableVector2
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector2Serializable(Vector2 rValue) => new Vector2Serializable(rValue.x, rValue.y);

}
