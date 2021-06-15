using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ExceptionSoftware.ExEditor
{
    public class BuiltInStylesWindow : EditorWindow
    {
        [MenuItem("Tools/Styles/Built-in styles and icons", priority = ExConstants.MENU_ITEM_PRIORITY)]
        public static void ShowWindow()
        {
            BuiltInStylesWindow w = (BuiltInStylesWindow)EditorWindow.GetWindow<BuiltInStylesWindow>();
            w.Show();
        }

        class Drawing
        {
            public Rect Rect;
            public string title;
            public string copyText;
            public Action Draw = null;
        }

        enum BultInState
        {
            Styles, Icons, Editor, IconBundle
        }

        private List<Drawing> _drawings = new List<Drawing>();

        //Window State
        BultInState _state = BultInState.Styles;

        //Search
        string _search = "";
        bool m_focusOnSearch = false;
        protected string m_searchFilterControl = "builtInNAMETEXTFIELDCONTROLNAME";
        protected bool m_resizable = false;

        //Item Collections
        GUIContent inactiveText = new GUIContent("inactive");
        GUIContent activeText = new GUIContent("active");
        private List<UnityEngine.Texture2D> _textures;
        bool _isDataDirty = false;

        //GUI
        Vector2 _scroll;
        private float _maxY;
        private Rect _oldPosition;

        //static float TILE_MARGIN = 5f;
        //static float WINDOW_BOTTOM_MARGIN = 32f;
        //static float WINDOW_LEFT_MARGIN = 32f;
        //Styles
        bool _stylesLoaded = false;
        GUIStyle _boxStyle;
        void LoadStyles()
        {
            if (_stylesLoaded) return;
            _boxStyle = new GUIStyle(GUI.skin.box);
            _boxStyle.normal.textColor = Color.white;
            _boxStyle.clipping = TextClipping.Clip;
            //_boxStyle.fontStyle = FontStyle.Bold;
            _stylesLoaded = true;
        }
        void DrawSearch()
        {
            float width = EditorGUIUtility.labelWidth;
            //EditorGUIUtility.labelWidth = m_searchLabelSize;
            EditorGUI.BeginChangeCheck();
            {
                GUI.SetNextControlName(m_searchFilterControl + m_resizable);

                EditorGUILayout.BeginHorizontal();
                _search = EditorGUILayout.TextField(_search, (GUIStyle)"SearchTextField");
                {
                    if (GUILayout.Button("", (GUIStyle)"SearchCancelButton"))
                    {
                        EditorGUI.FocusTextInControl(null);
                        _search = "";
                        _isDataDirty = true;
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (m_focusOnSearch)
                {
                    m_focusOnSearch = false;
                    EditorGUI.FocusTextInControl(m_searchFilterControl + m_resizable);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                _isDataDirty = true;
                _search = _search.ToLower();
            }
        }

        void OnGUI()
        {
            if (position.width != _oldPosition.width && Event.current.type == EventType.Layout)
            {
                _isDataDirty = true;
                _oldPosition = position;
            }
            LoadStyles();

            DrawToolbar();
            DrawSearch();


            float top = 36;

            if (_isDataDirty)
            {
                _drawings.Clear();

                switch (_state)
                {
                    case BultInState.Styles:
                        CollectBuiltInStyles(GUI.skin.customStyles);
                        break;
                    case BultInState.Icons:
                        CollectBuiltInIcons(_search);
                        break;
                    case BultInState.Editor:
                        CollectFromEditorStyles();
                        break;
                    case BultInState.IconBundle:
                        CollectIconsBundle(_search);
                        break;


                }
                _isDataDirty = false;
                //_maxY = y;
            }

            Rect r = position;
            r.y = top;
            r.height -= r.y;
            r.x = r.width - 16;
            r.width = 16;

            areaHeight = position.height - top;
            Rect area = new Rect(0, top, position.width - 16.0f, _maxY);

            GUILayout.BeginVertical();
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            {
                //EditorGUILayout.BeginVertical();
                GUILayout.Label(GUIContent.none, new GUIStyle(), GUILayout.Height(_maxY));
                //EditorGUILayout.EndVertical();
                DrawItems();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            //Debug.Log(_scroll.y);
        }
        float areaHeight;
        void DrawItems()
        {
            foreach (Drawing draw in _drawings)
            {
                if (draw == null)
                {
                    continue;
                }
                if (draw.Draw == null)
                {
                    continue;
                }

                if (_scroll.y < draw.Rect.yMax && draw.Rect.yMin < _scroll.y + areaHeight)
                {
                    try
                    {
                        GUILayout.BeginArea(draw.Rect, _boxStyle);
                        if (GUILayout.Button(draw.title, EditorStyles.miniButton))
                        {
                            EditorGUI.FocusTextInControl(null);
                            CopyText(draw.copyText);
                        }

                        //GUILayout.Space(18);
                        draw.Draw();
                        GUILayout.EndArea();
                    }
                    catch /* (System.ArgumentException ex)*/
                    {
                        //string s = ex.Message;
                        //s += "";
                    }

                }
            }
        }

        //void Try
        void CollectFromEditorStyles(/*string search*/)
        {
            System.Type editors = typeof(EditorStyles);
            PropertyInfo[] fs = typeof(EditorStyles).GetProperties(BindingFlags.Public | BindingFlags.Static);
            fs = typeof(EditorStyles).GetProperties(BindingFlags.Public | BindingFlags.Static);

            if (fs == null) return;

            List<GUIStyle> styles = new List<GUIStyle>();
            foreach (PropertyInfo p in fs)
            {
                try
                {
                    GUIStyle st = (GUIStyle)p.GetValue(null, null);
                    styles.Add(st);
                }
                catch { }
            }
            CollectBuiltInStyles(styles.ToArray());
        }
        void CollectBuiltInIcons(string search)
        {
            _textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D)).Cast<Texture2D>().OrderBy(o => (o.width * o.height)).ThenBy(os => os.name).ToList();

            //Vector2 pos = Vector2.zero;
            float x = 10, y = 0;
            float rowHeight = 0.0f;

            foreach (UnityEngine.Object oo in _textures)
            {
                Texture2D texture = (Texture2D)oo;

                if (texture.name == "")
                    continue;

                if (search != "" && !texture.name.ToLower().Contains(search))
                    continue;

                if (texture.texelSize.x >= 512 || texture.texelSize.y >= 512)
                {
                    continue;
                }

                Drawing draw = new Drawing();

                float width = Mathf.Max(GUI.skin.button.CalcSize(new GUIContent(texture.name)).x, texture.width) + 8.0f;
                float height = texture.height + GUI.skin.button.CalcSize(new GUIContent(texture.name)).y + 8.0f;

                if (x + width > position.width - 32.0f)
                {
                    x = 5.0f;
                    y += rowHeight + 8.0f;
                    rowHeight = 0.0f;
                }
                draw.Rect = new Rect(x, y, width, height);

#if UNITY_2020_1_OR_NEWER
                draw.copyText = "EditorGUIUtility.IconContent( \"" + texture.name + "\" ).icon";
#else
                draw.copyText = "EditorGUIUtility.FindTexture( \"" + texture.name + "\" )\n" + AssetDatabase.GetAssetPath(texture);
#endif

                draw.title = texture.name;

                rowHeight = Mathf.Max(rowHeight, height);

                width -= 8.0f;

                draw.Draw = () =>
                {
                    Rect textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height, texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                    EditorGUI.DrawTextureTransparent(textureRect, texture);
                };

                x += width + 8.0f;
                _drawings.Add(draw);
            }

            _maxY = _drawings.Last().Rect.yMax;
        }


        void CollectBuiltInStyles(GUIStyle[] styles)
        {
            float x = 10, y = 0;

            foreach (GUIStyle ss in styles)
            {
                if (_search != "" && !ss.name.ToLower().Contains(_search))
                    continue;

                GUIStyle thisStyle = ss;

                Drawing draw = new Drawing();

                float width = Mathf.Max(100.0f, ss.CalcSize(new GUIContent(ss.name)).x, ss.CalcSize(inactiveText).x + ss.CalcSize(activeText).x) + 16.0f;
                float height = 60.0f;

                if (x + width > position.width - 32 && x > 5.0f)
                {
                    x = 5.0f;
                    y += height + 10.0f;
                }

                draw.Rect = new Rect(x, y, width, height);
                draw.copyText = "(GUIStyle)\"" + thisStyle.name + "\"";
                draw.title = thisStyle.name;

                width -= 8.0f;

                draw.Draw = () =>
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUI.BeginChangeCheck();
                        {
                            GUILayout.Toggle(false, "AAA", thisStyle, GUILayout.Width(width * 0.5f));
                            GUILayout.Toggle(true, "BBB", thisStyle, GUILayout.Width(width * 0.5f));
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            CopyText(draw.copyText);
                        }
                    }
                    GUILayout.EndHorizontal();
                };

                x += width + 18.0f;

                _drawings.Add(draw);
            }
            if (_drawings != null)
                _maxY = _drawings.Last().Rect.yMax;
        }

        void CopyText(string pText)
        {
            TextEditor editor = new TextEditor();
            editor.text = pText;
            editor.SelectAll();
            editor.Copy();
        }


        void DrawToolbar()
        {
            EditorGUI.BeginChangeCheck();
            _state = (BultInState)GUILayout.Toolbar((int)_state, System.Enum.GetNames(typeof(BultInState)));
            if (EditorGUI.EndChangeCheck())
            {
                _isDataDirty = true;
            }
        }











        #region IconsBundle
        void CollectIconsBundle(string search)
        {
            _textures = GetListIconsbundle(search);

            //Vector2 pos = Vector2.zero;
            float x = 10, y = 0;
            float rowHeight = 0.0f;

            foreach (UnityEngine.Object oo in _textures)
            {
                Texture2D texture = (Texture2D)oo;

                if (texture.name == "")
                    continue;

                if (search != "" && !texture.name.ToLower().Contains(search))
                    continue;

                if (texture.texelSize.x >= 512 || texture.texelSize.y >= 512)
                {
                    continue;
                }

                Drawing draw = new Drawing();

                float width = Mathf.Max(GUI.skin.button.CalcSize(new GUIContent(texture.name)).x, texture.width) + 8.0f;
                float height = texture.height + GUI.skin.button.CalcSize(new GUIContent(texture.name)).y + 8.0f;

                if (x + width > position.width - 32.0f)
                {
                    x = 5.0f;
                    y += rowHeight + 8.0f;
                    rowHeight = 0.0f;
                }
                draw.Rect = new Rect(x, y, width, height);

#if UNITY_2020_1_OR_NEWER
                draw.copyText = "EditorGUIUtility.IconContent( \"" + texture.name + "\" ).icon";
#else
                draw.copyText = "EditorGUIUtility.FindTexture( \"" + texture.name + "\" )\n" + AssetDatabase.GetAssetPath(texture);
#endif

                draw.title = texture.name;

                rowHeight = Mathf.Max(rowHeight, height);

                width -= 8.0f;

                draw.Draw = () =>
                {
                    Rect textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height, texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                    EditorGUI.DrawTextureTransparent(textureRect, texture);
                };

                x += width + 8.0f;
                _drawings.Add(draw);
            }

            _maxY = _drawings.Last().Rect.yMax;
        }

        private static List<Texture2D> GetListIconsbundle(string filter)
        {
            List<Texture2D> list = new List<Texture2D>();
            var editorAssetBundle = GetEditorAssetBundle();
            var iconsPath = GetIconsPath();
            var count = 0;
            foreach (var assetName in EnumerateIcons(editorAssetBundle, iconsPath, filter))
            {
                var icon = editorAssetBundle.LoadAsset<Texture2D>(assetName);
                if (icon == null)
                    continue;

                list.Add(icon);
            }
            return list;
        }
        private static IEnumerable<string> EnumerateIcons(AssetBundle editorAssetBundle, string iconsPath, string filter)
        {
            foreach (var assetName in editorAssetBundle.GetAllAssetNames())
            {

                if (assetName.StartsWith(iconsPath, StringComparison.OrdinalIgnoreCase) == false)
                    continue;
                if (assetName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) == false &&
                    assetName.EndsWith(".asset", StringComparison.OrdinalIgnoreCase) == false)
                    continue;
                if (!filter.IsNullOrEmpty() && !assetName.Contains(filter)) continue;
                yield return assetName;
            }
        }
        private static AssetBundle GetEditorAssetBundle()
        {
            var editorGUIUtility = typeof(EditorGUIUtility);
            var getEditorAssetBundle = editorGUIUtility.GetMethod(
                "GetEditorAssetBundle",
                BindingFlags.NonPublic | BindingFlags.Static);

            return (AssetBundle)getEditorAssetBundle.Invoke(null, new object[] { });
        }

        private static string GetIconsPath()
        {
#if UNITY_2018_3_OR_NEWER
            return UnityEditor.Experimental.EditorResources.iconsPath;
#else
            var assembly = typeof(EditorGUIUtility).Assembly;
            var editorResourcesUtility = assembly.GetType("UnityEditorInternal.EditorResourcesUtility");

            var iconsPathProperty = editorResourcesUtility.GetProperty(
                "iconsPath",
                BindingFlags.Static | BindingFlags.Public);

            return (string)iconsPathProperty.GetValue(null, new object[] { });
#endif
        }
        #endregion
    }
}
