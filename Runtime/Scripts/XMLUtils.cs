using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public static class XMLUtils
{
    static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");

    //NO BORRAR: Es para xbox
    public static string SerializeObjectToString<T>(T toSerialize)
    {
        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
        var settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.OmitXmlDeclaration = true;
        using (var stream = new StringWriter())
        using (var writer = XmlWriter.Create(stream, settings))
        {
            xmlSerializer.Serialize(writer, toSerialize, emptyNamepsaces);
            return stream.ToString();
        }
    }


    public static void SerializeObjectToFile<T>(T toSerialize, string relativePath, string filename, bool encrypted = true) => SerializeObjectToFile<T>(toSerialize, relativePath + filename, encrypted);
    public static void SerializeObjectToFile<T>(T toSerialize, string path, bool encrypted = true)
    {
        if (encrypted)
        {
            EncryptAndSerialize(path, toSerialize);
        }
        else
        {
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                xmlSerializer.Serialize(writer, toSerialize, emptyNamepsaces);
            }
        }

    }
    public static T DeserializeToObject<T>(string relativePath, string filename, bool encrypted = true) where T : class => DeserializeToObject<T>(relativePath + filename, encrypted);
    public static T DeserializeToObject<T>(string path, bool encrypted = true) where T : class
    {
        if (encrypted)
        {
            return DecryptAndDeserialize<T>(path);
        }
        else
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (StreamReader sr = new StreamReader(path))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }

    public static T DeserializqToObjectFromResourcesPath<T>(string resourcesPath) where T : class
    {
        System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
        TextAsset txasset = Resources.Load<TextAsset>(resourcesPath);

        byte[] byteArray = Encoding.UTF8.GetBytes(txasset.text);
        MemoryStream stream = new MemoryStream(byteArray);

        using (TextReader sr = new StreamReader(stream))
        {
            return (T)ser.Deserialize(sr);
        }
    }

    #region encrypt

    public static void EncryptAndSerialize<T>(string path, T obj)
    {
        SerializeUniqueParam<T>(obj, path);
        StreamReader theReader = new StreamReader(path, Encoding.Default);
        string textToEncrypt = theReader.ReadToEnd();
        theReader.Close();
        File.WriteAllText(path, Encrypt(textToEncrypt));
    }
    public static T DecryptAndDeserialize<T>(string path)
    {
        StreamReader theReader = null;
        StringReader reader = null;

        if (File.Exists(path)) theReader = new StreamReader(path, Encoding.UTF8);
        else
        {
            Debug.LogError("Error loading non existent file");
            return default(T);
        }

        string textToWrite = Decrypt(theReader.ReadToEnd());
        theReader.Close();
        reader = new StringReader(textToWrite);

        XmlSerializer serializer = new XmlSerializer(typeof(T));
        T result = (T)serializer.Deserialize(reader);

        reader.Close();
        return result;
    }


    static void SerializeUniqueParam<T>(T param, string filename)
    {
        XmlSerializer serializer = new XmlSerializer(param.GetType());
        StreamWriter writer = new StreamWriter(filename, false);
        serializer.Serialize(writer, param);
        writer.Close();
    }

    static string Encrypt(string text)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(keyD);
        // 256-AES key
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        // better lang support
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return System.Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    static string keyD = "f396348bc2f035584b36a0dd6b7f92e1";

    static string Decrypt(string text)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(keyD);
        // AES-256 key
        //byte[] toEncryptArray = System.Convert.FromBase64String(text);
        byte[] toEncryptArray = System.Convert.FromBase64String(text);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        // better lang support
        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    #endregion

    #region PS


    public const int kSaveDataMaxSize = 1024 * 1024; //1MB

    public static byte[] WriteToBuffer<T>(T savegame)
    {
        System.IO.MemoryStream output = new MemoryStream(kSaveDataMaxSize);
        System.IO.BinaryWriter writer = new BinaryWriter(output);
        XMLSerialize<T>(savegame, writer);
        writer.Close();
        return output.GetBuffer();
    }

    public static T ReadFromBuffer<T>(byte[] buffer)
    {
        System.IO.MemoryStream input = new MemoryStream(buffer);
        System.IO.BinaryReader reader = new BinaryReader(input);
        //Debugx.Log($"XMLDeserialize BEGIN");
        T newSaveGame = XMLDeserialize<T>(reader);
        //Debugx.Log($"XMLDeserialize END");
        reader.Close();
        return newSaveGame;
    }

    static void XMLSerialize<T>(T param, BinaryWriter writer)
    {
        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        var settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.OmitXmlDeclaration = true;

        XmlSerializer serializer = new XmlSerializer(param.GetType());

        serializer.Serialize(writer.BaseStream, param, emptyNamepsaces);
    }
    static T XMLDeserialize<T>(BinaryReader reader)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        T result = (T)serializer.Deserialize(reader.BaseStream);
        return result;
    }

    public static T Clone<T>(T savegameToClone) => ReadFromBuffer<T>(WriteToBuffer(savegameToClone));

    #endregion
}
