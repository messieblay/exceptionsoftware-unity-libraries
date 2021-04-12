using System;
using UnityEditor;
using UnityEngine;

namespace ExceptionSoftware.ExEditor
{
    public class ExWindow<T> : ExWindow where T : ExWindow
    {

        public static T TryOpenWindow()
        {
            T mw = UnityEditor.EditorWindow.GetWindow<T>();
            if (mw.IsWindowOpened)
            {
                EditorWindow.FocusWindowIfItsOpen<T>();
            }
            else
            {
                mw.Open();
            }
            return mw;
        }

        public static T TryOpenUtility()
        {
            T mw = UnityEditor.EditorWindow.GetWindow<T>();
            if (mw.IsWindowOpened)
            {
                EditorWindow.FocusWindowIfItsOpen<T>();
            }
            else
            {
                mw.Open(null);
            }
            return mw;
        }
    }

    public class ExWindow : EditorWindow
    {
        public bool IsWindowOpened => ExWindowService.IsWindowOpened(this);

        /// <summary>
        /// Quick open. Is preferible write a custom open. Every window is diferent
        /// </summary>
        /// <param name="title"></param>
        public void Open(string title = null)
        {
            Open(title, new Vector2(200, 200), new Vector2(1000, 700));
        }

        public void Open(string title, Vector2 pos, Vector2 size)
        {
            base.titleContent = new GUIContent((title == null || title == string.Empty) ? GetTitle() : title);
            position = new Rect(pos, size);
            DoEnable();
            Show();
        }

        protected Event e { get { return Event.current; } }

        [NonSerialized] protected ExGUI ExGUIx = new ExGUI();
        [NonSerialized] protected bool _recompiled = false;
        [NonSerialized] protected static bool _needsLayout = false;
        [NonSerialized] protected bool _isOpened = true;
        [NonSerialized] protected bool _forceRepaint = false;
        [NonSerialized] protected Vector2 _windowSize = Vector2.one;


        internal void SetRecompile() => _recompiled = true;

        protected virtual void OnEnable()
        {
            ExWindowService.AddWindow(this);
            DoEnable();
        }

        protected virtual void DoEnable() { }
        protected virtual void OnDisable()
        {
            ExWindowService.RemoveWindow(this);
            DoDisable();
        }

        protected virtual void DoDisable() { }
        protected virtual void DoResize() { }
        protected virtual void DoRecompile() { }
        protected virtual void DoLayout() { }
        #region OnInspector

        public virtual void DoPreGUI() { }
        public virtual void DoGUI() { }
        public virtual void DoPostGUI() { }
        public virtual void DoLoadStyles() { }
        protected virtual void DoPreRepaint() { }
        public virtual string GetTitle() => GetType().Name;

        protected void OnGUI()
        {
            DoLoadStyles();

            if (_windowSize != base.position.size)
            {
                _windowSize = base.position.size;
                _needsLayout = true;
                DoResize();
            }

            if (_needsLayout)
            {
                _needsLayout = false;
                //CalcWindowSizes();
                DoLayout();
            }

            if (_recompiled)
            {
                _recompiled = false;
                DoRecompile();
            }

            DoPreGUI();
            DoGUI();
            DoPostGUI();

            CheckForceRepaint();
        }


        protected void CheckForceRepaint()
        {
            if (_forceRepaint)
            {
                _forceRepaint = false;
                DoPreRepaint();
                Repaint();
            }
        }
        #endregion

    }
}
