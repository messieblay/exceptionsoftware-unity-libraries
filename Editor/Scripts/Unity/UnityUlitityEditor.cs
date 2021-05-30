using com.hololabs;
using UnityEditor;
using UnityEngine;

public static class UnityUlitityEditor
{
    const string FUNCTIONS_MENUITEM = "Tools/Functions/";

    #region Remove Missing components
    static int go_count = 0, components_count = 0, missing_count = 0;

    [MenuItem(FUNCTIONS_MENUITEM + "Find and delete missing scripts")]
    public static void FindAndDeleteMissingComponents()
    {
        go_count = 0;
        components_count = 0;
        missing_count = 0;

        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            GameObject[] go = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects();
            foreach (GameObject g in go)
            {
                FindInGO(g);
            }
        }

        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing and deleted", go_count, components_count, missing_count));
    }

    static void FindInGO(GameObject g)
    {

        go_count++;
        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            components_count++;
            if (components[i] == null)
            {
                missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null)
                {
                    s = t.parent.name + "/" + s;
                    t = t.parent;
                }
                Debug.Log(s + " has an empty script attached in position: " + i, g);

                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
                Unityx.SetSceneDirty();
            }
        }

        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in g.transform)
        {
            FindInGO(childT.gameObject);
        }
    }
    #endregion


    #region Do Find By Guid Menu
    [MenuItem(FUNCTIONS_MENUITEM + "Find Asset by Guid %&g")]
    public static void DoFindByGuidMenu()
    {
        TextInputDialog.Prompt("GUID", "Find asset by Guid:", FindAssetByGuid);

        void FindAssetByGuid(string searchGuid)
        {
            string path = AssetDatabase.GUIDToAssetPath(searchGuid);
            if (string.IsNullOrEmpty(path)) return;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (obj == null) return;

            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
    }
    #endregion



    [MenuItem("Tools/Force recompile", false, 9000)]
    public static void ForceRecompile()
    {
        AssetDatabase.StartAssetEditing();
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssetPaths)
        {
            MonoScript script = AssetDatabase.LoadAssetAtPath(assetPath, typeof(MonoScript)) as MonoScript;
            if (script != null)
            {
                AssetDatabase.ImportAsset(assetPath);
                break;
            }
        }
        AssetDatabase.StopAssetEditing();
    }


}
