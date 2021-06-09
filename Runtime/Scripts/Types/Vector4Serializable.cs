using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Vector4Serializable
{
    [XmlAttribute] public float X;
    [XmlAttribute] public float Y;
    [XmlAttribute] public float Z;
    [XmlAttribute] public float W;

    public Vector4 Vector4
    {
        get => new Vector4((float)X, (float)Y, (float)Z, (float)W);
    }
    public Vector4Serializable() { }
    public Vector4Serializable(Vector4 vector)
    {
        X = vector.x;
        Y = vector.y;
        Z = vector.z;
        W = vector.w;
    }

    public Vector4Serializable(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }


    /// <summary>
    /// Automatic conversion from SerializableVector4 to Vector4
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector4(Vector4Serializable rValue) => new Vector4(rValue.X, rValue.Y, rValue.Z, rValue.W);

    /// <summary>
    /// Automatic conversion from Vector4 to SerializableVector4
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector4Serializable(Vector4 rValue) => new Vector4Serializable(rValue.x, rValue.y, rValue.z, rValue.w);
}
