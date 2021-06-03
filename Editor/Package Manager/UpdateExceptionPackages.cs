using SimpleJSON;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UpdateExceptionPackages : MonoBehaviour
{
    // Start is called before the first frame update

    [MenuItem("Tools/Update exception software packages", false, 9000)]
    static void UpdateEx()
    {
        var dir = System.IO.Directory.GetParent(Application.dataPath);
        var filePath = dir.FullName.Replace("\\", "/") + "/Packages/packages-lock.json";
        var jsonString = File.ReadAllText(filePath);
        JSONNode data = JSON.Parse(jsonString);
        var clone = data["dependencies"].Clone();

        List<JSONNode> recordsToDelete = new List<JSONNode>();
        foreach (JSONNode record in data["dependencies"])
        {
            if (record.ToString().Contains("exceptionsoftware"))
            {
                recordsToDelete.Add(record);
            }
        }

        foreach (var r in recordsToDelete)
        {
            data["dependencies"].Remove(r);
        }
        File.WriteAllText(filePath, data.ToString());
        AssetDatabase.Refresh(ImportAssetOptions.Default);


    }

}
