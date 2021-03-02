using UnityEditor;
using UnityEngine;

namespace ExSoftware.ExEditor
{
    public class ExEditor<T> : Editor
    {
        protected SerializedObject _serializedObject;

        public SerializedObject SerializedObject { get { return this._serializedObject; } }

        protected ExGUI ExGUIx = new ExGUI();

        protected Event e { get { return Event.current; } }

        protected static bool _recompiled = false;
        public static bool opened = true;

        static ExEditor()
        {
            _recompiled = true;
        }

        #region Serialization

        public SerializedProperty FindProperty(string prop) => _serializedObject.FindProperty(prop);

        #endregion

        #region Target

        protected T script = default(T);

        protected T GetTarget() => Target;

        protected T Target
        {
            get
            {
                if (script == null)
                {
                    script = (T)System.Convert.ChangeType(target, typeof(T));
                }
                _serializedObject = new SerializedObject(target);
                return script;
            }
        }

        protected Component Componentx => (Component)target;

        protected Transform Transformx => Componentx.transform;

        protected GameObject GameObjectx => Componentx.gameObject;

        #endregion

        protected virtual void OnEnable()
        {
            opened = true;
            if (target == null) { DestroyImmediate(this); return; }
            DoEnable();
        }

        protected virtual void DoEnable() { }

        protected virtual void OnDisable()
        {
            opened = false;
            if (target == null) { DestroyImmediate(this); return; }
            DoDisable();
        }

        protected virtual void DoDisable() { }
        protected virtual void DoRecompile() { }
        #region OnInspector

        protected virtual void BeginInspector() => GetTarget();

        protected virtual void EndInspector() { }
        protected virtual void DoInspector() { }
        public override void OnInspectorGUI()
        {
            BeginInspector();
            if (_recompiled)
            {
                _recompiled = false;
                DoRecompile();
                DoEnable();
            }

            DoInspector();
            EndInspector();
        }

        #endregion

        #region OnScene

        protected virtual void DoWindows() { }
        protected virtual void BeginScene() { }
        protected virtual void EndScene() => HandleUtility.Repaint();
        protected virtual void DoSceneGUI() { }

        public virtual void OnSceneGUI()
        {
            DoWindows();
            BeginScene();
            DoSceneGUI();
            EndScene();
        }

        protected void PreventClickSelection() => HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        public void DrawSerializedField(string fieldKey) => ExGUI.DrawSerializedField(_serializedObject, fieldKey);

        #endregion

        protected int _controlID;

        protected float handleSize = 0.04f;
        protected float pickSize = 0.06f;

    }
}
