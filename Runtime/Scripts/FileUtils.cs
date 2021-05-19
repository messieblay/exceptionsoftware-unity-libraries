using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


public class FileUtils
{
    public static string BACKSLASH = "/";
    public static string ASSETS_FOLDER = "Assets/";
    public static string RESOURCES_FOLDER = "Resources/";

    #region Paths 
    public static string ConvertPathToRelative(string path)
    {
        var pathproject = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        var relative = ConvertBackslash(path).Replace(pathproject, string.Empty);
        return relative;
    }

    public static string ConvertRelativePathToAbsolute(string path)
    {
        if (path.StartsWith("Assets/"))
        {
            path = path.Substring(6);
        }
        return ConcatPaths(Application.dataPath, path);
    }


    public static string ConvertBackslash(string path)
    {
        path = path.Replace("\\", BACKSLASH);
        return path;
    }
    public static string AddEndingSlash(string path)
    {
        if (!path.EndsWith(BACKSLASH))
        {
            path += BACKSLASH;
        }
        return path;
    }

    public static string ConcatPaths(string pathA, string pathB)
    {
        pathA = ConvertBackslash(pathA.Trim());
        pathB = ConvertBackslash(pathB.Trim());

        while (pathA.EndsWith(BACKSLASH))
        {
            pathA = pathA.Remove(pathA.Length - 1);
        }
        while (pathB.StartsWith(BACKSLASH))
        {
            pathB = pathB.Substring(1);
        }
        return pathA + BACKSLASH + pathB;
    }

    #endregion
    #region File management

#if UNITY_EDITOR
    //[UnityEditor.MenuItem("Tools/File/LocateFiles")]
    static void LocateFile()
    {
        Debug.Log(ConvertPathToRelative(AddEndingSlash(Path.GetDirectoryName(LocateFile("Inputdb.cs")).Replace("\\", BACKSLASH))));
    }
#endif
    public static string LocateFile(string filename)
    {
        string[] res = System.IO.Directory.GetFiles(Application.dataPath, filename, SearchOption.AllDirectories);
        if (res.Length == 0)
        {
            Debug.LogError("error message ....");
            return null;
        }
        string path = ConvertBackslash(res[0]);
        return path;
    }
    public static string LocateFilePath(string filename)
    {
        return ConvertPathToRelative(AddEndingSlash(Path.GetDirectoryName(LocateFile(filename)).Replace("\\", BACKSLASH)));
    }

    #endregion



    #region File Encript
    static string keyD = "f396348bc2f035584b36a0dd6b7f92e1";
    static string Encrypt(string text)
    {
        //return text;
        //if (!instance.saveStats.DEBUG_ENCRYPT_SAVEGAME) return text;

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

    static string Decrypt(string text)
    {

        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(keyD);
        // AES-256 key
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
}
