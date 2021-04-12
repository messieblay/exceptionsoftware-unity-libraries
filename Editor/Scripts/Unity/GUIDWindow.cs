//  06e687f68dd96f0448c6d8217bbcf608
using UnityEditor;
using UnityEngine;
namespace ExceptionSoftware.ExEditor
{
    public class GUIDWindow : ExWindow<GUIDWindow>
    {
        // Unity Menu item
        [MenuItem("Tools/Ex Software/Styles/GUID Tools", false, 1000)]
        static void OpenMainShaderGraph()
        {

            GUIDWindow currentWindow = (GUIDWindow)GUIDWindow.GetWindow<GUIDWindow>(false);
            currentWindow.minSize = new Vector2(400, 270);
            currentWindow.position = new Rect(new Vector2(200, 200), new Vector2(350, 600));
            currentWindow.wantsMouseMove = true;
            currentWindow.titleContent.text = "GUID Window";
            currentWindow.Show();
        }

        string[] _path = new string[3];
        string[] _guid = new string[3];
        UnityEngine.Object _obj;
        public override void DoGUI()
        {

            ExGUI.Title("GUID -> Path");
            EditorGUI.BeginChangeCheck();
            _guid[0] = EditorGUILayout.TextField("GUID", _guid[0]);
            if (EditorGUI.EndChangeCheck())
            {
                _path[0] = AssetDatabase.GUIDToAssetPath(_guid[0]);
            }
            EditorGUILayout.TextField("Path", _path[0]);



            ExGUI.Title("Path -> GUID");
            EditorGUI.BeginChangeCheck();
            _path[1] = EditorGUILayout.TextField("GUID", _path[1]);
            if (EditorGUI.EndChangeCheck())
            {
                _guid[1] = AssetDatabase.AssetPathToGUID(_path[1]);
            }

            EditorGUILayout.TextField("GUID", _guid[1]);


            ExGUI.Title("Object -> GUID");

            EditorGUI.BeginChangeCheck();
            _obj = (UnityEngine.Object)EditorGUILayout.ObjectField("Obj", _obj, typeof(UnityEngine.Object), false);
            if (EditorGUI.EndChangeCheck())
            {
                _guid[2] = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_obj));
            }
            EditorGUILayout.TextField("GUID", _guid[2]);

        }

    }
}
