using UnityEngine;

namespace Assets.Plugins.ExSoftware.Editor.Tools
{
    public static class Test
    {

        //
        // Resumen:
        //     Disposable helper class for managing BeginScrollView / EndScrollView.
        public class ColorScope : GUI.Scope
        {
            Color prev;
            public ColorScope(Color color)
            {
                prev = GUI.color;
                GUI.color = color;
            }
            protected override void CloseScope()
            {
                GUI.color = prev;
            }
        }
    }
}
