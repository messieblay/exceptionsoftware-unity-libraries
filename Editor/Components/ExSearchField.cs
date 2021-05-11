using UnityEditor;
using UnityEngine;
namespace ExceptionSoftware.ExEditor
{
    public class ExSearchField : ExEditorComponent
    {
        //Settings
        bool _delayed = true;
        bool _toolbar = false;

        protected string m_searchFilterControl = "SHADERNAMETEXTFIELDCONTROLNAME";
        protected string m_searchFilter = string.Empty;
        protected bool m_focusOnSearch = false;
        private float m_searchLabelSize = -1;
        private string m_searchFilterStr = "Search";

        public System.Action<string> onSearchChanged = null;

        float _seed;
        GUIStyle _styleField, _styleButton;

        public ExSearchField()
        {
            Init();
        }

        public ExSearchField(bool delayed, bool toolbar)
        {
            this._delayed = delayed;
            this._toolbar = toolbar;
            Init();
        }

        void Init()
        {
            this._seed = Random.Range(float.MinValue, float.MaxValue);

            if (_toolbar)
            {
                this._styleField = (GUIStyle)"ToolbarSeachTextField";
                this._styleButton = (GUIStyle)"ToolbarSeachCancelButton";
            }
            else
            {
                this._styleField = (GUIStyle)"SearchTextField";
                this._styleButton = (GUIStyle)"SearchCancelButton";
            }
        }
        void FireOnChangeSearch()
        {
            _isDirty = true;
            if (onSearchChanged != null) onSearchChanged(m_searchFilter);
        }
        public string DoLayoutSearch()
        {
            if (m_searchLabelSize < 0)
            {
                m_searchLabelSize = GUI.skin.label.CalcSize(new GUIContent(m_searchFilterStr)).x;
            }

            float width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = m_searchLabelSize;
            EditorGUI.BeginChangeCheck();
            {
                GUI.SetNextControlName(m_searchFilterControl + _seed);

                EditorGUILayout.BeginHorizontal();

                if (_delayed)
                {
                    m_searchFilter = EditorGUILayout.DelayedTextField(m_searchFilter, _styleField);
                }
                else
                {
                    m_searchFilter = EditorGUILayout.TextField(m_searchFilter, _styleField);
                }
                {
                    if (GUILayout.Button("", _styleButton))
                    {
                        m_searchFilter = "";
                        FireOnChangeSearch();
                        EditorGUI.FocusTextInControl(null);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (m_focusOnSearch)
                {
                    m_focusOnSearch = false;
                    EditorGUI.FocusTextInControl(m_searchFilterControl + _seed);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                FireOnChangeSearch();
            }

            EditorGUIUtility.labelWidth = width;
            return m_searchFilter;
        }

        public string DoSearch(Rect r)
        {
            r.width = Mathf.Max(30, r.width);

            Rect[] rs = r.Split(RectBorder.Right, _styleButton.fixedWidth);

            if (m_searchLabelSize < 0)
            {
                m_searchLabelSize = GUI.skin.label.CalcSize(new GUIContent(m_searchFilterStr)).x;
            }

            float width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = m_searchLabelSize;
            EditorGUI.BeginChangeCheck();
            {
                GUI.SetNextControlName(m_searchFilterControl + _seed);

                if (_delayed)
                {
                    m_searchFilter = EditorGUI.DelayedTextField(rs[0], m_searchFilter, _styleField);
                }
                else
                {
                    m_searchFilter = EditorGUI.TextField(rs[0], m_searchFilter, _styleField);
                }

                if (GUI.Button(rs[1], "", _styleButton))
                {
                    EditorGUI.FocusTextInControl(null);
                    m_searchFilter = "";
                    FireOnChangeSearch();

                }

                if (m_focusOnSearch)
                {
                    m_focusOnSearch = false;
                    EditorGUI.FocusTextInControl(m_searchFilterControl + _seed);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                FireOnChangeSearch();
            }

            EditorGUIUtility.labelWidth = width;

            return m_searchFilter;
        }
    }
}
