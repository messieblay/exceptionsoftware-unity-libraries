using UnityEditor;
using UnityEngine;

namespace ExSoftware.ExEditor
{
    public class ExWindowScene<T> : ExWindow<T> where T : ExWindow
    {

        #region OnScene

        protected virtual void BeginScene() { }
        protected virtual void EndScene()
        {
            if (Event.current.type != EventType.Repaint)
            {
                Event.current.Use();
            }
            HandleUtility.Repaint();
        }

        protected virtual void DoSceneGUI() { }
        protected void OnSceneGUI()
        {
            BeginScene();
            DoSceneGUI();
            EndScene();
        }

        #endregion
    }
}
