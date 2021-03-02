using UnityEngine;

namespace ExSoftware.ExEditor
{
    public class xEvents
    {

        public static Event Current => Event.current;

        #region Event type

        public static Vector2 MousePosition => Current.mousePosition;
        public static bool MouseDown => Current.rawType == EventType.MouseDown;
        public static bool MouseUp => Current.rawType == EventType.MouseUp;
        public static bool MouseDrag => Current.rawType == EventType.MouseDrag;
        public static bool DragExited => Current.rawType == EventType.DragExited;
        public static bool DragUpdated => Current.rawType == EventType.DragUpdated;
        public static bool KeyDown => Current.rawType == EventType.KeyDown;
        public static bool KeyUp => Current.rawType == EventType.KeyUp;
        public static bool Layout => Current.rawType == EventType.Layout;
        public static bool Repaint => Current.rawType == EventType.Repaint;
        public static bool ScrollWheel => Current.rawType == EventType.ScrollWheel;

        #endregion


        #region Event mouse

        public static bool MouseLeft => Current.button == 0;
        public static bool MouseRight => Current.button == 1;
        public static bool MouseMid => Current.button == 2;
        public static bool MouseSingleClick => Current.clickCount == 1;
        public static bool MouseDobleClick => Current.clickCount == 2;

        #endregion

        #region Keyboard

        public static bool Shift => Current.shift;
        public static bool Control => Current.control;
        public static bool Alt => Current.alt;
        public static Vector2 Delta { get => Current.delta; set => Current.delta = value; }
        public static bool Numeric => Current.numeric;
        public static bool IsKey => Current.isKey;
        public static bool IsMouse => Current.isMouse;
        public static KeyCode KeyCode => Current.keyCode;

        #endregion

    }

}
