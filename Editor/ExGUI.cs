using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ExceptionSoftware.ExEditor
{
    //using System.IO;
    public class ExGUI
    {

        Dictionary<string, bool> foldsStack = new Dictionary<string, bool>();

        bool TryGetFadeStack(string key, bool autoOpen)
        {
            if (!foldsStack.ContainsKey(key))
            {
                foldsStack.Add(key, autoOpen);
            }
            return foldsStack[key];
        }

        void RecordValue(string key, bool value)
        {
            foldsStack[key] = value;
        }

        public void FadeGroup(string title, FadeDelegate fadeFunc)
        {
            FadeGroup(title, title, true, UnityEditor.EditorStyles.helpBox, fadeFunc);
        }

        public void FadeGroup(string title, bool autoOpen, FadeDelegate fadeFunc)
        {
            FadeGroup(title, title, autoOpen, UnityEditor.EditorStyles.helpBox, fadeFunc);
        }

        public void FadeGroup(string title, GUIStyle style, FadeDelegate fadeFunc)
        {
            FadeGroup(title, title, true, style, fadeFunc);
        }

        public void FadeGroup(string title, bool autoOpen, GUIStyle style, FadeDelegate fadeFunc)
        {
            FadeGroup(title, title, autoOpen, style, fadeFunc);
        }

        public void FadeGroup(string title, string id, bool autoOpen, GUIStyle style, FadeDelegate fadeFunc)
        {
            bool open = TryGetFadeStack(id, autoOpen);
            ExGUI.ColorGUI(Color.white, delegate
            {
                ExGUI.FadeGroup(title, ref open, style, fadeFunc);
            });
            RecordValue(id, open);
        }

        public void ToggleGroup(string title, FadeDelegate fadeFunc)
        {
            ToggleGroup(title, title, true, fadeFunc);
        }

        public void ToggleGroup(string title, bool autoOpen, FadeDelegate fadeFunc)
        {
            ToggleGroup(title, title, autoOpen, fadeFunc);
        }

        public void ToggleGroup(string title, string id, bool autoOpen, FadeDelegate fadeFunc)
        {
            bool open = TryGetFadeStack(id, autoOpen);
            ExGUI.ToggleGroup(title, ref open, fadeFunc);
            RecordValue(id, open);
        }

        public void FadeGroup(string title, bool autoOpen, FadeDelegate preTitle, FadeDelegate postTitle, FadeDelegate fadeFunc)
        {
            bool open = TryGetFadeStack(title, autoOpen);
            ExGUI.FadeGroup(title, ref open, preTitle, postTitle, fadeFunc);
            RecordValue(title, open);
        }

        #region Layout

        public delegate void LayoutDelegate();

        public static void Horizontal(LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal(options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }

        public static void Horizontal(bool styled, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            if (styled)
            {
                Horizontal(UnityEditor.EditorStyles.helpBox, layoutFunc, options);
                return;
            }
            Horizontal(layoutFunc, options);
        }

        public static void Horizontal(GUIStyle style, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal(style, options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }

        public static void Vertical(LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.BeginVertical(options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndVertical();
        }

        public static void Vertical(bool styled, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            if (styled)
            {
                Vertical(UnityEditor.EditorStyles.helpBox, layoutFunc, options);
                return;
            }
            Vertical(layoutFunc, options);
        }

        public static void Vertical(GUIStyle style, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.BeginVertical(style, options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndVertical();
        }

        public delegate void FadeDelegate();

        public static void FadeGroup(string title, ref bool fade, FadeDelegate fadeFunc)
        {
            FadeGroup(title, ref fade, UnityEditor.EditorStyles.helpBox, fadeFunc);
        }

        public static void FadeGroup(string title, ref bool fade, GUIStyle style, FadeDelegate fadeFunc)
        {
            UnityEditor.EditorGUILayout.BeginVertical(style);
            UnityEditor.EditorGUILayout.BeginHorizontal();
            fade = UnityEditor.EditorGUILayout.Toggle(fade, UnityEditor.EditorStyles.foldout, UnityEngine.GUILayout.Width(15));
            UnityEditor.EditorGUILayout.LabelField(title, UnityEditor.EditorStyles.boldLabel);
            UnityEditor.EditorGUILayout.EndHorizontal();
            if (UnityEditor.EditorGUILayout.BeginFadeGroup(System.Convert.ToInt32(fade)))
            {
                if (fadeFunc != null)
                    fadeFunc();
            }
            UnityEditor.EditorGUILayout.EndFadeGroup();
            UnityEditor.EditorGUILayout.EndVertical();
        }

        public static void FadeGroup(string title, ref bool fade, FadeDelegate preTitle, FadeDelegate postTitle, FadeDelegate fadeFunc)
        {
            float minw, maxw;
            UnityEditor.EditorGUILayout.BeginVertical(UnityEditor.EditorStyles.helpBox);
            UnityEditor.EditorGUILayout.BeginHorizontal();
            fade = UnityEditor.EditorGUILayout.Toggle(fade, UnityEditor.EditorStyles.foldout, UnityEngine.GUILayout.Width(15));
            if (preTitle != null)
                preTitle();
            UnityEditor.EditorStyles.boldLabel.CalcMinMaxWidth(new GUIContent(title), out minw, out maxw);
            UnityEditor.EditorGUILayout.LabelField(title, UnityEditor.EditorStyles.boldLabel, GUILayout.Width(maxw));
            if (postTitle != null)
                postTitle();
            UnityEditor.EditorGUILayout.EndHorizontal();
            if (UnityEditor.EditorGUILayout.BeginFadeGroup(System.Convert.ToInt32(fade)))
            {
                if (fadeFunc != null)
                {
                    fadeFunc();
                }
            }
            UnityEditor.EditorGUILayout.EndFadeGroup();
            UnityEditor.EditorGUILayout.EndVertical();
        }



        public static void FadeToggle(string title, ref bool toggle, FadeDelegate fadeFunc)
        {
            UnityEditor.EditorGUILayout.BeginVertical(UnityEditor.EditorStyles.helpBox);
            toggle = UnityEditor.EditorGUILayout.ToggleLeft(title, toggle, UnityEditor.EditorStyles.boldLabel);
            if (fadeFunc != null && toggle)
                fadeFunc();
            UnityEditor.EditorGUILayout.EndVertical();
        }

        public static void ToggleGroup(string title, ref bool toggle, FadeDelegate fadeFunc)
        {
            toggle = UnityEditor.EditorGUILayout.BeginToggleGroup(title, toggle);
            if (fadeFunc != null)
                fadeFunc();
            UnityEditor.EditorGUILayout.EndToggleGroup();
        }

        public static void Group(string title, LayoutDelegate layoutFunc)
        {
            Title(title);
            layoutFunc();
        }

        public static void Group(string title, bool space, LayoutDelegate layoutFunc)
        {
            Title(title);
            layoutFunc();
            if (space)
            {
                UnityEditor.EditorGUILayout.Space();
            }
        }
        public static void Group(LayoutDelegate headerDraw, LayoutDelegate contentDraw)
        {
            if (headerDraw != null) headerDraw();
            if (contentDraw != null) contentDraw();
        }

        public static void Group(string title, bool space, GUIStyle style, LayoutDelegate layoutFunc)
        {
            Vertical(style, delegate
            {
                Group(title, space, layoutFunc);
            });
        }

        public static void Group(GUIStyle style, LayoutDelegate headerDraw, LayoutDelegate layoutFunc)
        {
            if (style == null)
            {
                EditorGUILayout.BeginVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical(style);
            }
            Group(headerDraw, layoutFunc);
            EditorGUILayout.EndVertical();
        }

        public static void ChangeCheck(System.Action check, System.Action onChange)
        {
            UnityEditor.EditorGUI.BeginChangeCheck();
            if (check != null)
            {
                check();
            }
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                if (onChange != null)
                {
                    onChange();
                }
            }
        }

        #endregion

        #region Foldout Group
        public void FoldoutGroupIndent(string id, string title, bool autoOpen, LayoutDelegate layoutFunc)
        {
            bool open = TryGetFadeStack(id, autoOpen);
            ExGUI.FoldoutGroupIndent(ref open, title, layoutFunc);
            RecordValue(id, open);
        }
        public void FoldoutGroup(string id, string title, bool autoOpen, LayoutDelegate layoutFunc)
        {
            bool open = TryGetFadeStack(id, autoOpen);
            ExGUI.FoldoutGroup(ref open, title, layoutFunc);
            RecordValue(id, open);
        }
        public void FoldoutGroup(string title, bool autoOpen, LayoutDelegate layoutFunc)
        {
            bool open = TryGetFadeStack(title, autoOpen);
            ExGUI.FoldoutGroup(ref open, title, layoutFunc);
            RecordValue(title, open);
        }

        public void FoldoutGroup(string title, bool autoOpen, LayoutDelegate layoutFunc, LayoutDelegate headerLayoutFunc)
        {
            bool open = TryGetFadeStack(title, autoOpen);
            ExGUI.FoldoutGroup(ref open, title, layoutFunc, headerLayoutFunc);
            RecordValue(title, open);
        }

        public static void FoldoutGroup(string title, LayoutDelegate layoutFunc)
        {
            Color cachedColor = GUI.color;
            GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, 0.5f);
            EditorGUILayout.BeginHorizontal(ExStyles.MenuItemToolbarStyle);
            GUI.color = cachedColor;

            GUILayout.Label(title, ExStyles.MenuItemToggleStyle);

            EditorGUILayout.EndHorizontal();


            cachedColor = GUI.color;
            GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, (EditorGUIUtility.isProSkin ? 0.5f : 0.25f));
            EditorGUILayout.BeginVertical(ExStyles.MenuItemBackgroundStyle);
            GUI.color = cachedColor;

            EditorGUI.indentLevel++;
            layoutFunc();
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();

        }

        public static void FoldoutGroup(ref bool foldoutValue, string title, LayoutDelegate layoutFunc)
        {
            Color cachedColor = GUI.color;
            GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, 0.5f);
            GUILayout.Space(EditorGUI.indentLevel);
            EditorGUILayout.BeginHorizontal(ExStyles.MenuItemToolbarStyle);
            GUI.color = cachedColor;

            bool value = GUILayout.Toggle(foldoutValue, title, ExStyles.MenuItemToggleStyle);
            if (Event.current.button == ExStyles.FoldoutMouseId)
            {
                foldoutValue = value;
            }
            EditorGUILayout.EndHorizontal();

            if (foldoutValue)
            {
                cachedColor = GUI.color;
                GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, (EditorGUIUtility.isProSkin ? 0.5f : 0.25f));
                EditorGUILayout.BeginVertical(ExStyles.MenuItemBackgroundStyle);
                {
                    GUI.color = cachedColor;
                    EditorGUI.indentLevel++;
                    layoutFunc();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Separator();
                }
                EditorGUILayout.EndVertical();
            }
        }


        public static void FoldoutGroup(ref bool foldoutValue, string title, LayoutDelegate layoutFunc, LayoutDelegate headerLayoutFunc)
        {
            Color cachedColor = GUI.color;
            GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, 0.5f);
            EditorGUILayout.BeginHorizontal(ExStyles.MenuItemToolbarStyle);
            GUI.color = cachedColor;

            bool value = GUILayout.Toggle(foldoutValue, title, ExStyles.MenuItemToggleStyle);
            if (Event.current.button == ExStyles.FoldoutMouseId)
            {
                foldoutValue = value;
            }
            headerLayoutFunc();
            EditorGUILayout.EndHorizontal();

            if (foldoutValue)
            {
                cachedColor = GUI.color;
                GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, (EditorGUIUtility.isProSkin ? 0.5f : 0.25f));
                EditorGUILayout.BeginVertical(ExStyles.MenuItemBackgroundStyle);
                {
                    GUI.color = cachedColor;
                    EditorGUI.indentLevel++;
                    layoutFunc();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Separator();
                }
                EditorGUILayout.EndVertical();
            }
        }
        public static void FoldoutGroupIndent(ref bool foldoutValue, string title, LayoutDelegate layoutFunc)
        {
            Color cachedColor = GUI.color;
            GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, 0.5f);
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(EditorGUI.indentLevel * 20);
                EditorGUILayout.BeginHorizontal(ExStyles.MenuItemToolbarStyle);
                {
                    GUI.color = cachedColor;

                    bool value = GUILayout.Toggle(foldoutValue, title, ExStyles.MenuItemToggleStyle);
                    if (Event.current.button == ExStyles.FoldoutMouseId)
                    {
                        foldoutValue = value;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(EditorGUI.indentLevel * 20);
                if (foldoutValue)
                {
                    cachedColor = GUI.color;
                    GUI.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, (EditorGUIUtility.isProSkin ? 0.5f : 0.25f));
                    EditorGUILayout.BeginVertical(ExStyles.MenuItemBackgroundStyle);
                    {
                        GUI.color = cachedColor;
                        EditorGUI.indentLevel--;
                        layoutFunc();
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region ScrollView


        Dictionary<string, Vector2> scrollStack = new Dictionary<string, Vector2>();

        Vector2 TryGetScrollStack(string key)
        {
            if (!scrollStack.ContainsKey(key))
            {
                scrollStack.Add(key, Vector2.zero);
            }
            return scrollStack[key];
        }

        void RecordScrollValue(string key, Vector2 scroll)
        {
            scrollStack[key] = scroll;
        }

        public void ScrollView(string id, bool alwaysShowHorizontal, bool alwaysShowVertical, LayoutDelegate fadeFunc, params GUILayoutOption[] options)
        {
            Vector2 scroll = TryGetScrollStack(id);
            scroll = ExGUI.ScrollView(scroll, alwaysShowHorizontal, alwaysShowHorizontal, fadeFunc, options);
            RecordScrollValue(id, scroll);
        }

        public void ScrollView(string id, LayoutDelegate fadeFunc, params GUILayoutOption[] options)
        {
            Vector2 scroll = TryGetScrollStack(id);
            scroll = ExGUI.ScrollView(scroll, fadeFunc, options);
            RecordScrollValue(id, scroll);
        }



        //
        //
        //




        public static Vector2 ScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            Vector2 d = UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndScrollView();
            return d;
        }

        public static Vector2 ScrollView(Vector2 scrollPosition, GUIStyle style, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            Vector2 d = UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition, style, options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndScrollView();
            return d;
        }

        public static Vector2 ScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            Vector2 d = UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndScrollView();
            return d;
        }

        public static Vector2 ScrollView(Vector2 scrollPosition, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            Vector2 d = UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition, options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndScrollView();
            return d;
        }

        public static Vector2 ScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, LayoutDelegate layoutFunc, params GUILayoutOption[] options)
        {
            Vector2 d = UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options);
            layoutFunc();
            UnityEditor.EditorGUILayout.EndScrollView();
            return d;
        }

        public static void Indent(LayoutDelegate layoutFunc)
        {
            UnityEditor.EditorGUI.indentLevel = UnityEditor.EditorGUI.indentLevel + 1;
            layoutFunc();
            UnityEditor.EditorGUI.indentLevel = UnityEditor.EditorGUI.indentLevel - 1;
        }

        #endregion

        #region Buttons

        public delegate void BotonDelegate();

        public static bool ButtonGo(string label, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            return Button(label, "Go", botonFunc, GUILayout.Width(30));
        }

        public static bool ButtonGo(string label, Color c, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            return Button(label, "Go", c, botonFunc, GUILayout.Width(30));
        }

        public static bool Button(string label, string labelButton, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField(label);
            bool res = false;
            if (GUILayout.Button(labelButton, options))
            {
                res = true;
                botonFunc();
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            return res;
        }

        public static bool Button(string label, Texture2D iconButton, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField(label);
            bool res = false;
            if (GUILayout.Button(iconButton, options))
            {
                res = true;
                botonFunc();
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            return res;
        }

        public static bool Button(string label, string labelButton, Color c, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField(label);
            bool res = Button(labelButton, c, botonFunc, options);
            UnityEditor.EditorGUILayout.EndHorizontal();
            return res;
        }

        public static bool Button(string labelButton, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            bool res = false;
            if (GUILayout.Button(labelButton, options))
            {
                GUI.color = Color.white;
                res = true;
                botonFunc();
            }
            return res;
        }

        public static bool Button(string labelButton, Color c, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            GUI.color = c;
            bool res = Button(labelButton, botonFunc, options);
            GUI.color = Color.white;
            return res;
        }

        public static bool Button(string labelButton, Color c, GUIStyle style, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            GUI.color = c;
            bool res = Button(labelButton, style, botonFunc, options);
            GUI.color = Color.white;
            return res;
        }

        public static bool Button(string labelButton, GUIStyle style, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            bool res = false;
            if (GUILayout.Button(labelButton, style, options))
            {
                GUI.color = Color.white;
                res = true;
                botonFunc();
            }
            return res;
        }



        public static bool Button(Texture2D iconButton, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            bool res = false;
            if (GUILayout.Button(iconButton, options))
            {
                GUI.color = Color.white;
                res = true;
                botonFunc();
            }
            return res;
        }

        public static bool Button(Texture2D iconButton, Color c, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            GUI.color = c;
            bool res = Button(iconButton, botonFunc, options);
            GUI.color = Color.white;
            return res;
        }

        public static bool Button(Texture2D iconButton, Color c, GUIStyle style, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            GUI.color = c;
            bool res = Button(iconButton, style, botonFunc, options);
            GUI.color = Color.white;
            return res;
        }

        public static bool Button(Texture2D iconButton, GUIStyle style, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            bool res = false;
            if (GUILayout.Button(iconButton, style, options))
            {
                GUI.color = Color.white;
                res = true;
                botonFunc();
            }
            return res;
        }

        public static bool Button(GUIContent content, GUIStyle style, BotonDelegate botonFunc, params GUILayoutOption[] options)
        {
            bool res = false;
            if (GUILayout.Button(content, style, options))
            {
                GUI.color = Color.white;
                res = true;
                botonFunc();
            }
            return res;
        }

        #endregion

        #region COMPONENTES

        public static void Title(string title)
        {
            UnityEditor.EditorGUILayout.LabelField(title, UnityEditor.EditorStyles.boldLabel);
        }

        public static void TitleAutoMenu(string title)
        {
            GUILayout.Space(15);
            GUILayout.Label(title, ExStyles.Head2Style);
        }

        public static void TitleHead(string title)
        {
            GUILayout.Space(15);
            GUILayout.Label(title, ExStyles.Head1Style);
        }

        public static void HelpBox(string texto, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.LabelField(texto, UnityEditor.EditorStyles.helpBox);
        }

        public static void HelpBoxWarning(string texto, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.HelpBox(texto, UnityEditor.MessageType.Warning);
        }

        public static void HelpBoxError(string texto, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.HelpBox(texto, UnityEditor.MessageType.Error);
        }

        public static void HelpBoxInfo(string texto, params GUILayoutOption[] options)
        {
            UnityEditor.EditorGUILayout.HelpBox(texto, UnityEditor.MessageType.Info);
        }

        /** Draws a thin separator line */
        public static void Separator()
        {
            EditorGUILayout.LabelField("", (GUIStyle)"sv_iconselector_sep", GUILayout.Height(4));
        }


        public static bool ToggleButton(string s, ref bool selected, System.Action actions = null, params GUILayoutOption[] options)
        {
            GUIStyle st = new GUIStyle("Button");
            if (selected)
            {
                st.normal = st.active;
            }
            if (GUILayout.Button(s, options))
            {
                selected = !selected;
                try
                {
                    if (actions != null)
                        actions.Invoke();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            return selected;
        }

        public static bool ToggleButton(string s, ref bool selected, GUIStyle style, System.Action actions = null, params GUILayoutOption[] options)
        {
            GUIStyle st = new GUIStyle(style);
            if (selected)
            {
                st.normal = st.active;
            }
            if (GUILayout.Button(s, st, options))
            {
                selected = !selected;
                try
                {
                    if (actions != null)
                        actions.Invoke();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            return selected;
        }

        public static bool ToggleButton(Texture2D image, ref bool selected, System.Action actions = null, params GUILayoutOption[] options)
        {
            Texture2D fondo = null;
            fondo = GUI.skin.button.normal.background;
            bool res = false;
            if (selected)
            {
                fondo = GUI.skin.button.normal.background;
                GUI.skin.button.normal.background = GUI.skin.button.active.background;
            }
            if (GUILayout.Button(image, options))
            {
                selected = !selected;
                res = true;
                try
                {
                    if (actions != null)
                        actions.Invoke();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            GUI.skin.button.normal.background = fondo;
            return res;
        }

        public static bool ToggleButton(Texture2D image, ref bool selected, GUIStyle style, System.Action actions = null, params GUILayoutOption[] options)
        {
            Texture2D fondo = null;
            fondo = GUI.skin.button.normal.background;
            bool res = false;
            if (selected)
            {
                fondo = GUI.skin.button.normal.background;
                GUI.skin.button.normal.background = GUI.skin.button.active.background;
            }
            if (GUILayout.Button(image, style, options))
            {
                selected = !selected;
                res = true;
                try
                {
                    if (actions != null)
                        actions.Invoke();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            GUI.skin.button.normal.background = fondo;
            return res;
        }

        public static bool DoPlayButtonCommand(bool isEditing, System.Action startAction, System.Action stopAction)
        {
            GUI.changed = false;
            GUIContent contnt = isEditing ? EditorGUIUtility.IconContent("PlayButton On") : EditorGUIUtility.TrIconContent("PlayButton", "Play");
            Color color = GUI.color + new Color(0.01f, 0.01f, 0.01f, 0.01f);
            GUI.contentColor = new Color(1f / color.r, 1f / color.g, 1f / color.g, 1f / color.a);

            bool edit = GUILayout.Toggle(isEditing, contnt, "Command");
            GUI.backgroundColor = Color.white;
            if (GUI.changed)
            {
                if (edit)
                {
                    if (startAction != null) startAction();
                    //isEditing = edit;
                }
                else
                {
                    if (stopAction != null) stopAction();
                    //isEditing = edit;
                }
            }
            GUI.changed = false;
            return edit;
        }


        public static bool DoPlayButtonSmall(string title, bool isEditing, System.Action startAction, System.Action stopAction)
        {
            GUILayout.FlexibleSpace();
            ExGUI.Horizontal(delegate ()
            {
                GUIStyle style = new GUIStyle(EditorStyles.miniButton);
                if (isEditing)
                {
                    style.normal = style.active;
                    ExGUI.Button(new GUIContent(title, EditorGUIUtility.FindTexture("PlayButton On"), "Stop editing"), style, delegate
                    {
                        stopAction();
                        isEditing = false;
                    }, GUILayout.Height(15));
                }
                else
                {
                    //GUI.enabled = GUI.enabled && CallCanBeginEditingTool(compEditor.editor);
                    ExGUI.Button(new GUIContent(title, EditorGUIUtility.FindTexture("PlayButton"), "Start editing"), style, delegate
                    {
                        if (!isEditing)
                        {
                            stopAction();
                        }
                        startAction();
                        isEditing = true;
                    }, GUILayout.Height(15));
                }

            });
            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            return isEditing;
        }

        #endregion

        #region Colores

        public delegate void ColorDelegate();

        public static void ColorGUI(Color color, ColorDelegate colorFun)
        {
            GUI.color = color;
            colorFun();
            ResetColorGUI();
        }

        public static void ColorHandles(Color color, ColorDelegate colorFun)
        {
            UnityEditor.Handles.color = color;
            colorFun();
            ResetColorHandles();
        }

        public static void ColorGUI(Color color, bool use, ColorDelegate colorFun)
        {
            if (use)
                GUI.color = color;
            colorFun();
            ResetColorGUI();
        }

        public static void ColorHandles(Color color, bool use, ColorDelegate colorFun)
        {
            if (use)
                UnityEditor.Handles.color = color;
            colorFun();
            ResetColorHandles();
        }

        public static void ColorGUI(Color colorTrue, Color colorFalse, bool use, ColorDelegate colorFun)
        {
            GUI.color = use ? colorTrue : colorFalse;
            colorFun();
            ResetColorGUI();
        }

        public static void ColorHandles(Color colorTrue, Color colorFalse, bool use, ColorDelegate colorFun)
        {
            UnityEditor.Handles.color = use ? colorTrue : colorFalse;
            colorFun();
            ResetColorHandles();
        }

        //  static Color colorgui;
        //  static Color colorhandles;
        public static void SetColorGUI(Color c)
        {
            //      colorgui = GUI.color;
            GUI.color = c;
        }

        public static void ResetColorGUI()
        {
            GUI.color = Color.white;
        }

        public static void SetColorHandles(Color c)
        {
            //      colorhandles = UnityEditor.Handles.color;
            UnityEditor.Handles.color = c;
        }

        public static void ResetColorHandles()
        {
            //      UnityEditor.Handles.color = colorhandles;
            UnityEditor.Handles.color = Color.white;
        }

        #endregion

        #region Areas

        public static void Area(Rect r, System.Action action)
        {
            GUILayout.BeginArea(r);
            if (action != null)
            {
                action();
            }
            GUILayout.EndArea();
        }

        public static void Area(Rect r, GUIStyle style, System.Action action)
        {
            GUILayout.BeginArea(r, style);
            if (action != null)
            {
                action();
            }
            GUILayout.EndArea();
        }

        #endregion

        #region Line/Rect/Grid/etc drawing

        public static void DrawHorizontalLine(float thickness, Color color, float paddingTop = 0f, float paddingBottom = 0f, float width = 0f)
        {
            GUILayoutOption[] options = new GUILayoutOption[2] {
                GUILayout.ExpandHeight (false),
                (width > 0.0f ? GUILayout.Width (width) : GUILayout.ExpandWidth (true))
            };
            Color prevColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayoutUtility.GetRect(0f, (thickness + paddingTop + paddingBottom), options);
            Rect r = GUILayoutUtility.GetLastRect();
            r.y += paddingTop;
            r.height = thickness;
            GUI.Box(r, "", ExStyles.DividerStyle);
            GUI.backgroundColor = prevColor;
        }

        public static void DrawVerticalLine(float thickness, Color color, float paddingLeft = 0f, float paddingRight = 0f, float height = 0f)
        {
            GUILayoutOption[] options = new GUILayoutOption[2] {
                GUILayout.ExpandWidth (false),
                (height > 0.0f ? GUILayout.Height (height) : GUILayout.ExpandHeight (true))
            };
            Color prevColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayoutUtility.GetRect((thickness + paddingLeft + paddingRight), 0f, options);
            Rect r = GUILayoutUtility.GetLastRect();
            r.x += paddingLeft;
            r.width = thickness;
            GUI.Box(r, "", ExStyles.DividerStyle);
            GUI.backgroundColor = prevColor;
        }

        #endregion

        #region Draw Rect

        static Texture2D _staticRectTexture = null;
        static GUIStyle _staticRectStyle = null;
        static GUIStyle _staticTextStyle = null;

        public static void GUIDrawRect(Rect position, Color color, string text)
        {
            GUIDrawRect(position, color, text, false);
        }

        public static void GUIDrawRect(Rect position, Color color, string text, bool mid)
        {
            if (_staticRectTexture == null)
            {
                _staticRectTexture = new Texture2D(1, 1);
            }

            if (_staticRectStyle == null)
            {
                _staticRectStyle = new GUIStyle();
            }
            if (_staticTextStyle == null)
            {
                _staticTextStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
                _staticTextStyle.alignment = TextAnchor.MiddleCenter;
            }
            _staticRectTexture.SetPixel(0, 0, color);
            _staticRectTexture.Apply();

            _staticRectStyle.normal.background = _staticRectTexture;

            GUI.Box(position, GUIContent.none, _staticRectStyle);
            //      GUI.Label (position, text, EditorStyles.whiteLabel);
            position.width = 50;
            position.x -= 25;
            if (text != "")
                EditorGUI.LabelField(position, text, mid ? _staticTextStyle : EditorStyles.whiteBoldLabel);
        }

        public static void GUIDrawRectOutline(Rect position, Color color, int size)
        {
            if (_staticRectTexture == null)
            {
                _staticRectTexture = new Texture2D(1, 1);
            }

            if (_staticRectStyle == null)
            {
                _staticRectStyle = new GUIStyle();
            }
            if (_staticTextStyle == null)
            {
                _staticTextStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
                _staticTextStyle.alignment = TextAnchor.MiddleCenter;
            }
            _staticRectTexture.SetPixel(0, 0, color);
            _staticRectTexture.Apply();

            _staticRectStyle.normal.background = _staticRectTexture;
            Rect rt = new Rect(position);
            rt.yMin = rt.yMax - size;

            Rect rl = new Rect(position);
            rl.xMax = rl.xMin + size;
            Rect rr = new Rect(position);
            rr.xMin = rr.xMax - size;
            Rect rd = new Rect(position);
            rd.yMax = rd.yMin + size;

            GUI.Box(rt, GUIContent.none, _staticRectStyle);
            GUI.Box(rd, GUIContent.none, _staticRectStyle);
            GUI.Box(rl, GUIContent.none, _staticRectStyle);
            GUI.Box(rr, GUIContent.none, _staticRectStyle);
        }

        #endregion

        #region Draw Array
        [System.Obsolete]
        public static void DrawSerializedField(SerializedObject so, string propertyKey)
        {
            SerializedProperty property = so.FindProperty(propertyKey);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(property, true);
            if (EditorGUI.EndChangeCheck())
            {
                so.ApplyModifiedProperties();
            }
        }

        #endregion


    }

}
