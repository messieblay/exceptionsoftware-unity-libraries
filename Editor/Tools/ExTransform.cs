using UnityEditor;
using UnityEngine;

namespace ExSoftware.ExEditor
{
    [CanEditMultipleObjects, CustomEditor(typeof(Transform))]
    public class ExTransform : Editor
    {
        enum Axes
        {
            None,
            X,
            Y,
            Z = 4,
            All = 7
        }

        static ExTransform transform;

        SerializedProperty mPos;

        SerializedProperty mRot;
        Vector3 mRotEuler;

        SerializedProperty mScl;

        static bool _byColliders;

        static bool _alignAffectX = true;

        static bool _alignAffectY = true;

        static bool _alignAffectZ = true;

        static double _forceExpandTime;

        static GUIStyle _collapsedStyle;
        static float _buttonHeight = 18f;

        void OnEnable()
        {
            ExTransform.transform = this;
            this.mPos = base.serializedObject.FindProperty("m_LocalPosition");
            this.mRot = base.serializedObject.FindProperty("m_LocalRotation");
            this.mScl = base.serializedObject.FindProperty("m_LocalScale");
            this.mRotEuler = this.mRot.quaternionValue.eulerAngles;

            _background = GUI.backgroundColor;
            _foreground = GUI.contentColor;
        }

        static Color _background, _foreground;
        static readonly Color greenLt = Color.Lerp(Color.green, Color.white, 0.5f);
        static readonly Color redLt = Color.Lerp(Color.red, Color.white, 0.5f);
        static readonly Color blueLt = Color.Lerp(Color.blue, Color.white, 0.5f);

        void OnDestroy()
        {
            ExTransform.transform = null;
        }

        public static void SetBGColor(Color clr, float proportion = 0.5f)
        {
            GUI.backgroundColor = Color.Lerp(_background, clr, proportion);
        }

        public static void SetFGColor(Color clr, float proportion = 0.5f)
        {
            GUI.contentColor = Color.Lerp(_foreground, clr, proportion);
        }

        public static void SetColor(Color clr, float proportion = 0.5f)
        {
            GUI.backgroundColor = Color.Lerp(_background, clr, proportion);
            GUI.contentColor = Color.Lerp(_foreground, clr, proportion);
        }

        public static void ResetColors()
        {
            GUI.backgroundColor = _background;
            GUI.contentColor = _foreground;
        }

        public override void OnInspectorGUI()
        {
            //eGUI.RememberColors ();
            base.serializedObject.Update();
            EditorGUIUtility.labelWidth = 15f;
            this.DrawPosition();
            this.DrawRotation();
            this.DrawScale();

            ResetColors();
        }

        void DrawPosition()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            {
                Vector3 position = (base.serializedObject.targetObject as Transform).position;
                SetColor((position == Vector3.zero) ? Color.white : Color.gray, 0.5f);
                GUI.skin.button.padding = new RectOffset(0, 0, 0, 0);
                Texture2D image = (Texture2D)EditorGUIUtility.Load((position == Vector3.zero) ? "d_MoveTool" : "d_MoveTool On");

                if (GUILayout.Button(new GUIContent(image, "Reset Position"), GUILayout.Width(32f), GUILayout.Height(_buttonHeight)))
                {
                    Undo.RecordObject(ExTransform.transform, "Reset Position " + ExTransform.transform.name);
                    this.mPos.vector3Value = Vector3.zero;
                    base.serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.BeginChangeCheck();
                {
                    GUI.skin.button.padding = new RectOffset(6, 6, 2, 3);
                    SetColor(redLt, 0.5f);
                    EditorGUILayout.PropertyField(this.mPos.FindPropertyRelative("x"), new GUILayoutOption[0]);
                    SetColor(greenLt, 0.5f);
                    EditorGUILayout.PropertyField(this.mPos.FindPropertyRelative("y"), new GUILayoutOption[0]);
                    SetColor(blueLt, 0.5f);
                    EditorGUILayout.PropertyField(this.mPos.FindPropertyRelative("z"), new GUILayoutOption[0]);
                    ResetColors();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(ExTransform.transform, "Position Changed" + ExTransform.transform.name);
                    base.serializedObject.ApplyModifiedProperties();
                }
            }
            GUILayout.EndHorizontal();
        }

        void DrawScale()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            {
                Vector3 localScale = (base.serializedObject.targetObject as Transform).localScale;
                SetColor((localScale == Vector3.one) ? Color.white : Color.gray, 0.5f);
                GUI.skin.button.padding = new RectOffset(0, 0, 0, 0);
                Texture2D image = (Texture2D)EditorGUIUtility.Load((localScale == Vector3.one) ? "d_ScaleTool" : "d_ScaleTool On");


                if (GUILayout.Button(new GUIContent(image, "Reset Scale"), GUILayout.Width(32f), GUILayout.Height(_buttonHeight)))
                {
                    Undo.RecordObject(ExTransform.transform, "Reset Scale " + ExTransform.transform.name);
                    this.mScl.vector3Value = Vector3.one;
                    base.serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.BeginChangeCheck();
                {
                    GUI.skin.button.padding = new RectOffset(6, 6, 2, 3);
                    SetColor(redLt, 0.5f);
                    EditorGUILayout.PropertyField(this.mScl.FindPropertyRelative("x"), new GUILayoutOption[0]);
                    SetColor(greenLt, 0.5f);
                    EditorGUILayout.PropertyField(this.mScl.FindPropertyRelative("y"), new GUILayoutOption[0]);
                    SetColor(blueLt, 0.5f);
                    EditorGUILayout.PropertyField(this.mScl.FindPropertyRelative("z"), new GUILayoutOption[0]);
                    ResetColors();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(ExTransform.transform, "Scale Changed" + ExTransform.transform.name);
                    base.serializedObject.ApplyModifiedProperties();
                }
            }
            GUILayout.EndHorizontal();
        }

        void DrawRotation()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            {
                Vector3 eulerAngles = (base.serializedObject.targetObject as Transform).rotation.eulerAngles;
                SetColor((eulerAngles == Vector3.zero) ? Color.white : Color.gray, 0.5f);
                GUI.skin.button.padding = new RectOffset(0, 0, 0, 0);
                Texture2D image = (Texture2D)EditorGUIUtility.Load((eulerAngles == Vector3.zero) ? "d_RotateTool" : "d_RotateTool On");

                if (GUILayout.Button(new GUIContent(image, "Reset Rotation"), GUILayout.Width(32f), GUILayout.Height(_buttonHeight)))
                {
                    Undo.RecordObject(ExTransform.transform, "Reset Rotation " + ExTransform.transform.name);
                    this.mRot.quaternionValue = Quaternion.identity;
                    base.serializedObject.ApplyModifiedProperties();
                }

                if (this.mRot != null) this.mRotEuler = this.mRot.quaternionValue.eulerAngles;

                EditorGUI.BeginChangeCheck();
                {
                    GUI.skin.button.padding = new RectOffset(6, 6, 2, 3);
                    Vector3 localEulerAngles = (base.serializedObject.targetObject as Transform).localEulerAngles;
                    ExTransform.Axes axes = this.CheckDifference(this.mRot);
                    ExTransform.Axes axes2 = ExTransform.Axes.None;

                    GUILayoutOption opt = GUILayout.MinWidth(30f);
                    SetColor(redLt, 0.5f);
                    this.mRotEuler.x = EditorGUILayout.FloatField("X", this.mRotEuler.x, new GUILayoutOption[0]);

                    SetColor(greenLt, 0.5f);
                    this.mRotEuler.y = EditorGUILayout.FloatField("Y", this.mRotEuler.y, new GUILayoutOption[0]);

                    SetColor(blueLt, 0.5f);
                    this.mRotEuler.z = EditorGUILayout.FloatField("Z", this.mRotEuler.z, new GUILayoutOption[0]);

                    ResetColors();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(ExTransform.transform, "Rotation Changed" + ExTransform.transform.name);
                    this.mRot.quaternionValue = Quaternion.Euler(this.mRotEuler);
                    base.serializedObject.ApplyModifiedProperties();
                }
            }
            GUILayout.EndHorizontal();
        }

        static ExTransform.Axes CheckDifference(Transform t, Vector3 original)
        {
            Vector3 localEulerAngles = t.localEulerAngles;
            ExTransform.Axes axes = ExTransform.Axes.None;
            if (ExTransform.Differs(localEulerAngles.x, original.x))
            {
                axes |= ExTransform.Axes.X;
            }
            if (ExTransform.Differs(localEulerAngles.y, original.y))
            {
                axes |= ExTransform.Axes.Y;
            }
            if (ExTransform.Differs(localEulerAngles.z, original.z))
            {
                axes |= ExTransform.Axes.Z;
            }
            return axes;
        }

        ExTransform.Axes CheckDifference(SerializedProperty property)
        {
            ExTransform.Axes axes = ExTransform.Axes.None;
            if (property.hasMultipleDifferentValues)
            {
                Vector3 eulerAngles = property.quaternionValue.eulerAngles;
                UnityEngine.Object[] targetObjects = base.serializedObject.targetObjects;
                for (int i = 0; i < targetObjects.Length; i++)
                {
                    UnityEngine.Object @object = targetObjects[i];
                    axes |= ExTransform.CheckDifference(@object as Transform, eulerAngles);
                    if (axes == ExTransform.Axes.All)
                    {
                        break;
                    }
                }
            }
            return axes;
        }

        static bool FloatField(string name, ref float value, bool hidden, GUILayoutOption opt)
        {
            float num = value;
            GUI.changed = false;
            if (!hidden)
            {
                num = EditorGUILayout.FloatField(name, num, new GUILayoutOption[] {
                    opt
                });
            }
            else
            {
                float.TryParse(EditorGUILayout.TextField(name, "--", new GUILayoutOption[] {
                    opt
                }), out num);
            }
            bool result;
            if (GUI.changed && ExTransform.Differs(num, value))
            {
                value = num;
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        static bool Differs(float a, float b)
        {
            return Mathf.Abs(a - b) > 0.0001f;
        }

    }
}
