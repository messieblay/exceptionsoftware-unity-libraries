using System.Collections.Generic;
using System.Linq;

namespace ExSoftware.ExEditor
{
    public static class ExWindowService
    {
        #region Recompile

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void DidReloadScripts()
        {
            _allOpenedWindows.Values.ForEach(s => s.SetRecompile());
        }

        #endregion
        #region Window management
        static Dictionary<System.Type, ExWindow> _allOpenedWindows = new Dictionary<System.Type, ExWindow>();
        public static void AddWindow(ExWindow window)
        {
            _allOpenedWindows.TryAdd(window.GetType(), window);
        }

        public static void RemoveWindow(ExWindow window)
        {
            _allOpenedWindows.Remove(window.GetType());
        }

        public static T GetWindow<T>() where T : ExWindow
        {
            if (_allOpenedWindows.TryGetValue(typeof(T), out ExWindow win))
            {
                return (T)win;
            }
            return default(T);
        }

        public static void ForceRepaintAll() => _allOpenedWindows.Values.ForEach(s => s.Repaint());

        public static void GetWindow<T>(System.Action<T> action) where T : ExWindow
        {
            if (_allOpenedWindows.TryGetValue(typeof(T), out ExWindow win))
            {
                if (action != null)
                {
                    action((T)win);
                }
            }
        }
        public static bool IsWindowOpened<T>(T win) where T : UnityEditor.EditorWindow => _allOpenedWindows.Values.Where(s => s == win).Count() > 0;
        public static bool IsWindowOpened(UnityEditor.EditorWindow win) => _allOpenedWindows.Values.Where(s => s == win).Count() > 0;

        #endregion
    }
}
