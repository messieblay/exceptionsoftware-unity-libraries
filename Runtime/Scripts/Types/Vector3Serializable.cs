using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Vector3Serializable
{
    [XmlAttribute] public float X;
    [XmlAttribute] public float Y;
    [XmlAttribute] public float Z;

    public Vector3 Vector3
    {
        get => new Vector3((float)X, (float)Y, (float)Z);
    }
    public Vector3Serializable() { }
    public Vector3Serializable(Vector3 vector)
    {
        X = vector.x;
        Y = vector.y;
        Z = vector.z;
    }

    public Vector3Serializable(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }


    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(Vector3Serializable rValue)
    {
        return new Vector3(rValue.X, rValue.Y, rValue.Z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3Serializable(Vector3 rValue)
    {
        return new Vector3Serializable(rValue.x, rValue.y, rValue.z);
    }

}
