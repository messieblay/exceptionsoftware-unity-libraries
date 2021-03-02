using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class SerializableVector3
{
    [XmlAttribute] public float X;
    [XmlAttribute] public float Y;
    [XmlAttribute] public float Z;

    public Vector3 Vector3
    {
        get => new Vector3((float)X, (float)Y, (float)Z);
    }
    public SerializableVector3() { }
    public SerializableVector3(Vector3 vector)
    {
        X = vector.x;
        Y = vector.y;
        Z = vector.z;
    }

    public SerializableVector3(float x, float y, float z)
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
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.X, rValue.Y, rValue.Z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }

}
