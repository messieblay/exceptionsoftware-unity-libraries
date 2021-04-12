using UnityEngine;
namespace ExceptionSoftware.ExEditor
{
    public static class ExGUIExtra
    {

        static GUIStyle foldoutStyle = null;
        public static bool TabFadeGroup(bool active, string title, System.Action toolbar, System.Action fadeFunc)
        {
            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(UnityEditor.EditorStyles.boldLabel)
                {
                    margin = new RectOffset(4, 4, 2, 4)
                };
            }
            UnityEditor.EditorGUILayout.BeginVertical();
            ExGUI.Separator();
            UnityEditor.EditorGUILayout.BeginHorizontal();
            bool fade = UnityEditor.EditorGUILayout.Toggle(active, UnityEditor.EditorStyles.foldout, UnityEngine.GUILayout.Width(15), GUILayout.Height(20));

            if (GUILayout.Button(title, foldoutStyle))
            {
                fade = !fade;
            }

            GUILayout.FlexibleSpace();

            toolbar.TryInvoke();

            UnityEditor.EditorGUILayout.EndHorizontal();
            if (UnityEditor.EditorGUILayout.BeginFadeGroup(System.Convert.ToInt32(fade)))
            {
                if (fadeFunc != null)
                    fadeFunc();
            }
            UnityEditor.EditorGUILayout.EndFadeGroup();
            UnityEditor.EditorGUILayout.EndVertical();
            UnityEditor.EditorGUILayout.Space();
            return fade;
        }
        public static bool TabFadeGroup(bool active, string title, System.Action fadeFunc)
        {
            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(UnityEditor.EditorStyles.boldLabel)
                {
                    margin = new RectOffset(4, 4, 2, 4)
                };
            }
            UnityEditor.EditorGUILayout.BeginVertical();
            ExGUI.Separator();
            UnityEditor.EditorGUILayout.BeginHorizontal();
            bool fade = UnityEditor.EditorGUILayout.Toggle(active, UnityEditor.EditorStyles.foldout, UnityEngine.GUILayout.Width(15), GUILayout.Height(20));

            if (GUILayout.Button(title, foldoutStyle))
            {
                fade = !fade;
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            if (UnityEditor.EditorGUILayout.BeginFadeGroup(System.Convert.ToInt32(fade)))
            {
                if (fadeFunc != null)
                    fadeFunc();
            }
            UnityEditor.EditorGUILayout.EndFadeGroup();
            UnityEditor.EditorGUILayout.EndVertical();
            UnityEditor.EditorGUILayout.Space();
            return fade;
        }

        public static bool ToggleFadeGroup(bool active, string title, System.Action fadeFunc, System.Action<bool> onchange = null)
        {
            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(UnityEditor.EditorStyles.boldLabel);
                //{
                //    margin = new RectOffset(40, 4, 2, 4),
                //    padding = new RectOffset(40, 4, 2, 4),
                //    clipOffset = new Vector2(20, 0)
                //};
            }
            //EditorGUILayout.(foldoutStyle, typeof(GUIStyle), false);
            UnityEditor.EditorGUILayout.BeginVertical();
            ExGUI.Separator();
            UnityEditor.EditorGUILayout.BeginHorizontal();
            bool fade = UnityEditor.EditorGUILayout.Toggle(active, UnityEngine.GUILayout.Width(15), GUILayout.Height(20));

            if (GUILayout.Button(title, foldoutStyle))
            {
                fade = !active;

            }
            if (fade != active)
            {
                if (onchange != null)
                {
                    onchange(fade);
                }
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            if (UnityEditor.EditorGUILayout.BeginFadeGroup(System.Convert.ToInt32(fade)))
            {
                if (fadeFunc != null)
                    fadeFunc();
            }
            UnityEditor.EditorGUILayout.EndFadeGroup();
            UnityEditor.EditorGUILayout.EndVertical();
            UnityEditor.EditorGUILayout.Space();
            return fade;
        }
    }
}
