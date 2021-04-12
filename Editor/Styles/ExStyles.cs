using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ExceptionSoftware.ExEditor
{
    public class ExStyles
    {
        //[UnityEditor.MenuItem("Window/ExSoftware/Styles/Reload ExStyles")]
        public static void ReloadStyles()
        {
            stylesLoaded = false;
            TryLoadStyles();
        }

        public const string EditorResourcePath = "Assets/ExSoftware/ExEditor/Res/";
        //      public const string EditorResourcePath = "Res/";

        public static string DllPath { get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("\\", "/"); } }
        //
        //      public static string DllPathRelative {
        //          get {
        //
        //              if (DllPath.EndsWith ("/Assets")) {
        //                  return DllPath.Remove (DllPath.Length - ("Assets".Length));
        //              }
        //
        //              return System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location).Replace ("\\", "/");
        //          }
        //      }

        public static string editorAssets = "Assets/ExSoftware/ExEditor";
        public static string editorSkinName = "ExEditorSkin.guiskin";
        public static string editorSkinLightName = "ExEditorSkin.guiskin";
        public static bool useDarkSkin = false;

        public static GUISkin Skin;
        public static GUISkin InspectorSkin;
        public static bool stylesLoaded = false;

        public static Color DividerColor = new Color(0f, 0f, 0f, 0.25f);
        public static Color InspectorDividerColor = new Color(0.35f, 0.35f, 0.35f, 1f);
        public static Color ToolbarDividerColor = new Color(0f, 0.5f, 0.8f, 1f);
        public static Color ButtonOnColor = new Color(0.24f, 0.5f, 0.87f);

        //Dark
        public static GUIStyle DSeparador;
        public static GUIStyle DBox;
        public static GUIStyle DBoxRound;
        public static GUIStyle DBoxSides;

        //Mid
        public static GUIStyle MSeparador;
        public static GUIStyle MBox;
        public static GUIStyle MBoxRound;
        public static GUIStyle MBoxSides;

        //Light
        public static GUIStyle LSeparador;
        public static GUIStyle LBoxRound;
        public static GUIStyle LBox;
        public static GUIStyle LBoxSides;

        //Empty
        public static GUIStyle EBox;



        public static GUIStyle SelectionGrid;

        public static GUIStyle thinHelpBox;

        static GUIStyle _helpBox = null;

        public static GUIStyle HelpBox
        {
            get
            {
                if (_helpBox == null)
                {
                    _helpBox = UnityEditor.EditorStyles.helpBox;
                }
                return _helpBox;
            }
        }

        static GUIStyle[] customStyles;

        // text & labels
        public static GUIStyle WarningLabelStyle;
        // yellow text (in dark theme, else red)
        public static GUIStyle BoldLabelStyle;
        // bold label
        public static GUIStyle RichLabelStyle;
        // label that can parse html tags
        public static GUIStyle CenterLabelStyle;
        // a centered label
        public static GUIStyle Head1Style;
        // big heading style
        public static GUIStyle Head2Style;
        // heading style
        public static GUIStyle Head3Style;
        // heading style
        public static GUIStyle Head4Style;
        // heading style
        public static GUIStyle LabelRightStyle;
        // right-aligned label
        public static GUIStyle InspectorHeadStyle;
        // heading used in the inspector (similar in size to head3 but with different padding)
        public static GUIStyle InspectorHeadBoxStyle;
        // a seperator like heading used in the inspector
        public static GUIStyle InspectorHeadFoldStyle;
        // similar to InspectorHeadStyle, but has foldout icon
        public static GUIStyle HeadingFoldoutStyle;
        // a foldout that has big text (like a heading style)

        public static GUIStyle BoldLabelRed;
        public static GUIStyle BoldLabelYellow;
        public static GUIStyle BoldLabelGreen;

        // boxes & frames
        public static GUIStyle DividerStyle;
        public static GUIStyle BoxStyle;
        public static GUIStyle ScrollViewStyle;
        public static GUIStyle ScrollViewNoTLMarginStyle;
        public static GUIStyle ScrollViewNoMarginPaddingStyle;


        // resources - skin
        public static Texture2D Texture_Logo;
        public static Texture2D Texture_LogoIcon;
        public static Texture2D Texture_Box;
        public static Texture2D Texture_FlatBox;
        public static Texture2D Texture_NoPreview;
        public static Texture2D Texture_BoxSides;
        public static Texture2D Texture_BoxSubSides;
        public static Texture2D Texture_Selected;
        public static Texture2D Texture_FlatDarker;
        public static Texture2D Texture_FlatLighter;
        public static Texture2D Texture_Plus;
        public static Texture2D Texture_Minus;
        public static Texture2D Texture_Save;
        public static Texture2D Texture_Trash;

        public static Texture2D Texture_WindowOff;
        public static Texture2D Texture_WindowOn;

        // misc
        public static GUIStyle MenuBoxStyle;
        public static GUIStyle MenuBoxSubStyle;
        public static GUIStyle MenuItemStyle;
        public static GUIStyle MenuMiniItemStyle;
        public static GUIStyle MenuHeadStyle;
        public static GUIStyle ListItemBackDarkStyle;
        public static GUIStyle ListItemBackLightStyle;
        public static GUIStyle ListItemSelectedStyle;
        public static GUIStyle AboutLogoAreaStyle;

        // List
        public static GUIStyle ListTitle;
        public static GUIStyle ListBox;
        public static GUIStyle ListItem;
        public static GUIStyle ListItemSelected;

        public static GUIStyle greyBorder;
        public static GUIStyle curveEditor;


        public static GUIStyle MenuItemToolbarStyle = new GUIStyle((GUIStyle)"toolbarbutton") { fixedHeight = 20 };
        public static GUIStyle MenuItemToggleStyle = new GUIStyle((GUIStyle)"foldout");
        public static GUIStyle MenuItemBackgroundStyle = new GUIStyle((GUIStyle)"TE NodeBackground");
        public static GUIStyle MenuItemEnableStyle;





        public readonly static int FoldoutMouseId = 0;

        public static readonly float toolbarHeight = 17f;
        public static readonly float border = -2f;

        public static void TryLoadStyles()
        {
            if (!stylesLoaded)
            {
                LoadStyles();
            }
        }

        public static bool LoadStyles()
        {
            //Correct paths if necessary
            string projectPath = Application.dataPath;
            //          string projectPath = DllPath;
            //          Debug.Log ("Normal - " + DllPath);
            //          Debug.Log ("projectPath - " + projectPath);
            if (projectPath.EndsWith("/Assets"))
            {
                projectPath = projectPath.Remove(projectPath.Length - ("Assets".Length));
            }
            //          Debug.Log ("projectPath2 - " + projectPath);
            //          Debug.Log ("CodeBase: " +   System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().CodeBase).Replace ("\\", "/"));

            if (!System.IO.File.Exists(projectPath + "/" + editorSkinName))
            {
                //Initiate search

                System.IO.DirectoryInfo sdir = new System.IO.DirectoryInfo(Application.dataPath);
                //              System.IO.DirectoryInfo sdir = new System.IO.DirectoryInfo (DllPath);

                Queue<System.IO.DirectoryInfo> dirQueue = new Queue<System.IO.DirectoryInfo>();
                dirQueue.Enqueue(sdir);

                bool found = false;
                while (dirQueue.Count > 0)
                {
                    System.IO.DirectoryInfo dir = dirQueue.Dequeue();
                    if (System.IO.File.Exists(dir.FullName + "/" + editorSkinName))
                    {
                        string path = dir.FullName.Replace('\\', '/');
                        found = true;
                        //Remove data path from string to make it relative
                        path = path.Replace(projectPath, "");

                        if (path.StartsWith("/"))
                        {
                            path = path.Remove(0, 1);
                        }

                        editorAssets = path;
                        //                      Debug.Log ("Localizado directorio de assets de editor en '" + editorAssets + "'");
                        break;
                    }
                    System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        dirQueue.Enqueue(dirs[i]);
                    }
                }

                if (!found)
                {
                    Debug.LogWarning("No se han podido encontrar los Asset de estilo");
                    return false;
                }
            }

            //End checks
            Skin = AssetDatabase.LoadAssetAtPath(editorAssets + "/" + editorSkinName, typeof(GUISkin)) as GUISkin;
            //      inspectorSkin = AssetDatabase.LoadAssetAtPath (editorAssets + "/InspectorSkin.guiskin", typeof(GUISkin)) as GUISkin;
            GUISkin inspectorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            if (Skin != null)
            {
                Skin.button = inspectorSkin.button;
            }
            else
            {
                //Load skin at old path
                Skin = AssetDatabase.LoadAssetAtPath(editorAssets + "/" + editorSkinName, typeof(GUISkin)) as GUISkin;
                if (Skin != null)
                {
                    AssetDatabase.RenameAsset(editorAssets + "/" + editorSkinName, editorSkinLightName);
                }
                else
                {
                    return false;
                }
                //Error is shown in the inspector instead
                //Debug.LogWarning ("Couldn't find 'AstarEditorSkin' at '"+editorAssets + "/AstarEditorSkin.guiskin"+"'");
            }
            DSeparador = Skin.FindStyle("DSeparator");
            MSeparador = Skin.FindStyle("MSeparator");
            LSeparador = Skin.FindStyle("LSeparator");


            DBox = Skin.FindStyle("DBox");
            DBoxRound = Skin.FindStyle("DBox_Round");
            DBoxSides = Skin.FindStyle("DBox_Sides");

            MBox = Skin.FindStyle("MBox");
            MBoxRound = Skin.FindStyle("MBox_Round");
            MBoxSides = Skin.FindStyle("MBox_Sides");

            LBox = Skin.FindStyle("LBox");
            LBoxRound = Skin.FindStyle("LBox_Round");
            LBoxSides = Skin.FindStyle("LBox_Sides");

            EBox = Skin.FindStyle("EBox");


            SelectionGrid = Skin.FindStyle("SelectionGrid");

            try
            {
                greyBorder = (GUIStyle)"grey_border";
                curveEditor = (GUIStyle)"CurveEditorBackground";
            }
            catch (System.Exception)
            {
            }
            try
            {
                _helpBox = EditorStyles.helpBox;
            }
            catch (System.Exception)
            {
            }
            try
            {
                thinHelpBox = new GUIStyle(_helpBox);
                thinHelpBox.contentOffset = new Vector2(0, -2);
                thinHelpBox.stretchWidth = false;
                thinHelpBox.clipping = TextClipping.Overflow;
                thinHelpBox.overflow.top += 1;
            }
            catch (System.Exception)
            {
            }

            //Load foldOut Textures
            MenuItemToolbarStyle = new GUIStyle((GUIStyle)"toolbarbutton") { fixedHeight = 20 };
            MenuItemToggleStyle = new GUIStyle((GUIStyle)"foldout");
            MenuItemBackgroundStyle = new GUIStyle((GUIStyle)"TE NodeBackground");
            MenuItemEnableStyle = useDarkSkin ? new GUIStyle((GUIStyle)"OL ToggleWhite") : new GUIStyle((GUIStyle)"OL Toggle");




            LoadSkinTextures();
            try
            {
                //      Skin = GUI.skin;
                //EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
                customStyles = Skin.customStyles;
                WarningLabelStyle = AddCustomStyle(ref customStyles, "ExGSWarningLabel", new GUIStyle(EditorStyles.label)
                {
                    name = "ExGSWarningLabel",
                    richText = false,
                    normal = { textColor = (EditorGUIUtility.isProSkin ? Color.yellow : Color.red) },
                });
                BoldLabelStyle = AddCustomStyle(ref customStyles, "ExGSBoldLabel", new GUIStyle(EditorStyles.boldLabel)
                {
                    name = "ExGSBoldLabel",
                    richText = false,
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(3, 3, 0, 3),
                });
                RichLabelStyle = AddCustomStyle(ref customStyles, "ExGSRichLabel", new GUIStyle(EditorStyles.label)
                {
                    name = "ExGSRichLabel",
                    richText = true,
                });
                CenterLabelStyle = AddCustomStyle(ref customStyles, "ExGSCenterLabel", new GUIStyle(EditorStyles.label)
                {
                    name = "ExGSCenterLabel",
                    richText = false,
                    alignment = TextAnchor.MiddleCenter
                });
                Head1Style = AddCustomStyle(ref customStyles, "ExGSHead1", new GUIStyle(EditorStyles.boldLabel)
                {
                    name = "ExGSHead1",
                    richText = false,
                    fontSize = 20,
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 3, 3),
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f) : new Color(0.5f, 0.5f, 0.5f)) },
                });
                Head2Style = AddCustomStyle(ref customStyles, "ExGSHead2", new GUIStyle(EditorStyles.boldLabel)
                {
                    name = "ExGSHead2",
                    richText = false,
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 10),
                    normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
                });
                Head3Style = AddCustomStyle(ref customStyles, "ExGSHead3", new GUIStyle(EditorStyles.boldLabel)
                {
                    name = "ExGSHead3",
                    richText = false,
                    fontSize = 15,
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 5),
                    normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
                });
                Head4Style = AddCustomStyle(ref customStyles, "ExGSHead4", new GUIStyle(Head3Style)
                {
                    name = "ExGSHead4",
                    richText = false,
                    fontSize = 12,
                    normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
                });
                LabelRightStyle = AddCustomStyle(ref customStyles, "ExGSLabelRight", new GUIStyle(Skin.label)
                {
                    name = "ExGSLabelRight",
                    richText = false,
                    alignment = TextAnchor.MiddleRight,
                });
                InspectorHeadStyle = AddCustomStyle(ref customStyles, "ExGSInspectorHead", new GUIStyle(Head3Style)
                {
                    name = "ExGSInspectorHead",
                    padding = new RectOffset(15, 0, 3, 3),
                });
                InspectorHeadBoxStyle = AddCustomStyle(ref customStyles, "ExGSInspectorHeadBox", new GUIStyle(Skin.box)
                {
                    name = "ExGSInspectorHeadBox",
                    richText = false,
                    fontSize = 13,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(25, 0, 3, 3),
                    margin = new RectOffset(0, 0, 20, 10),
                    stretchWidth = true,
                    normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
                });
                InspectorHeadFoldStyle = AddCustomStyle(ref customStyles, "ExGSInspectorHeadFold", new GUIStyle(EditorStyles.foldout)
                {
                    name = "ExGSInspectorHeadFold",
                    richText = false,
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(17, 0, 0, 0),
                    margin = new RectOffset(10, 10, 2, 0),
                    normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
                });
                AboutLogoAreaStyle = AddCustomStyle(ref customStyles, "ExGSHeadingFoldout", new GUIStyle(EditorStyles.foldout)
                {
                    name = "ExGSHeadingFoldout",
                    richText = false,
                    fontSize = 15,
                    padding = new RectOffset(17, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 5),
                    normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
                });
                DividerStyle = AddCustomStyle(ref customStyles, "ExGSDivider", new GUIStyle()
                {
                    name = "ExGSDivider",
                    border = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    normal = { background = EditorGUIUtility.whiteTexture },
                });
                BoxStyle = AddCustomStyle(ref customStyles, "ExGSBox", new GUIStyle(Skin.box)
                {
                    name = "ExGSBox",
                    padding = new RectOffset(10, 10, 10, 10),
                    margin = new RectOffset(5, 0, 5, 0),
                    normal = { background = (EditorGUIUtility.isProSkin ? Texture_Box : Skin.box.normal.background) },
                });
                ScrollViewStyle = AddCustomStyle(ref customStyles, "ExGSScrollView", new GUIStyle(Skin.box)
                {
                    name = "ExGSScrollView",
                    padding = new RectOffset(10, 10, 10, 10),
                    margin = new RectOffset(5, 5, 5, 5),
                });
                ScrollViewNoTLMarginStyle = AddCustomStyle(ref customStyles, "ExGSScrollViewNoTLMargin", new GUIStyle(ScrollViewStyle)
                {
                    name = "ExGSScrollViewNoTLMargin",
                    margin = new RectOffset(0, 5, 0, 5),
                });
                ScrollViewNoMarginPaddingStyle = AddCustomStyle(ref customStyles, "ExGSScrollViewNoTLMarginNoPadding", new GUIStyle(ScrollViewStyle)
                {
                    name = "ExGSScrollViewNoTLMarginNoPadding",
                    padding = new RectOffset(0, 0, 3, 3),
                    margin = new RectOffset(0, 0, 0, 0),
                });
                MenuBoxStyle = AddCustomStyle(ref customStyles, "ExGSMenuBox", new GUIStyle(BoxStyle)
                {
                    name = "ExGSMenuBox",
                    margin = new RectOffset(1, 0, 0, 0),
                    padding = new RectOffset(1, 1, 0, 0),
                    normal = { background = Texture_BoxSides },
                });
                MenuBoxSubStyle = AddCustomStyle(ref customStyles, "ExGSMenuSubBox", new GUIStyle(BoxStyle)
                {
                    name = "ExGSMenuSubBox",
                    margin = new RectOffset(1, 10, 0, 0),
                    padding = new RectOffset(1, 1, 0, 0),
                    normal = { background = Texture_BoxSides },
                });
                MenuItemStyle = AddCustomStyle(ref customStyles, "ExGSMenuItem", new GUIStyle(EditorStyles.toggle)
                {
                    name = "ExGSMenuItem",
                    richText = false,
                    fontSize = 14,
                    alignment = TextAnchor.MiddleRight,
                    border = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 10, 11, 7),
                    normal = { background = null },
                    hover = { background = null },
                    active = { background = null },
                    focused = { background = null },
                    onNormal = { background = Texture_Selected, textColor = Color.white },
                    onHover = { background = null },
                    onActive = { background = Texture_Selected, textColor = Color.white },
                    onFocused = { background = null },
                });
                MenuMiniItemStyle = AddCustomStyle(ref customStyles, "ExGSMenuMiniItem", new GUIStyle(EditorStyles.toggle)
                {
                    name = "ExGSMenuMiniItem",
                    richText = false,
                    fontSize = 10,
                    alignment = TextAnchor.MiddleRight,
                    border = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 10, 3, 3),
                    normal = { background = null },
                    hover = { background = null },
                    active = { background = null },
                    focused = { background = null },
                    onNormal = { background = Texture_Selected, textColor = Color.white },
                    onHover = { background = null },
                    onActive = { background = Texture_Selected, textColor = Color.white },
                    onFocused = { background = null },
                });
                MenuHeadStyle = AddCustomStyle(ref customStyles, "ExGSMenuHead", new GUIStyle(EditorStyles.label)
                {
                    name = "ExGSMenuHead",
                    richText = false,
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleRight,
                    border = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 10, 20, 3),
                });
                ListItemBackDarkStyle = AddCustomStyle(ref customStyles, "ExGSListItemBackDark", new GUIStyle(Skin.button)
                {
                    name = "ExGSListItemBackDark",
                    richText = false,
                    fontSize = 11,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Clip,
                    stretchWidth = false,
                    wordWrap = false,
                    overflow = new RectOffset(0, 0, 0, 0),
                    border = new RectOffset(1, 1, 1, 1),
                    margin = new RectOffset(0, 0, 1, 0),
                    hover = { background = Texture_FlatDarker },
                    onHover = { background = Texture_FlatDarker },
                    normal = { background = Texture_FlatDarker },
                    active = { background = Texture_FlatDarker },
                    onNormal = { background = Texture_FlatDarker },
                    onActive = { background = Texture_FlatDarker },
                });
                ListItemSelectedStyle = AddCustomStyle(ref customStyles, "ExGSListItemSelected", new GUIStyle(ListItemBackDarkStyle)
                {
                    name = "ExGSListItemSelected",
                    hover = { background = Texture_Selected, textColor = Color.white },
                    onHover = { background = Texture_Selected, textColor = Color.white },
                    normal = { background = Texture_Selected, textColor = Color.white },
                    active = { background = Texture_Selected, textColor = Color.white },
                    onNormal = { background = Texture_Selected, textColor = Color.white },
                    onActive = { background = Texture_Selected, textColor = Color.white },
                });
                AboutLogoAreaStyle = AddCustomStyle(ref customStyles, "ExGSAboutLogoArea", new GUIStyle(Skin.box)
                {
                    name = "ExGSAboutLogoArea",
                    stretchWidth = true,
                    margin = new RectOffset(0, 0, 0, 10),
                    padding = new RectOffset(10, 10, 10, 10),
                    normal = { background = EditorGUIUtility.whiteTexture },
                });
                ListItemBackLightStyle = AddCustomStyle(ref customStyles, "ExGSListItemBackLight", new GUIStyle(ListItemBackDarkStyle)
                {
                    name = "ExGSListItemBackLight",
                    hover = { background = Texture_FlatLighter },
                    onHover = { background = Texture_FlatLighter },
                    normal = { background = Texture_FlatLighter },
                    active = { background = Texture_FlatLighter },
                    onNormal = { background = Texture_FlatLighter },
                    onActive = { background = Texture_FlatLighter },
                });
                BoldLabelRed = AddCustomStyle(ref customStyles, "ExGSBoldRed", new GUIStyle(EditorStyles.boldLabel)
                {
                    name = "ExGSBoldRed",
                    richText = false,
                    normal = { textColor = Color.red },
                });
                BoldLabelYellow = AddCustomStyle(ref customStyles, "ExGSBoldYellow", new GUIStyle(EditorStyles.boldLabel)
                {
                    name = "ExGSBoldYellow",
                    richText = false,
                    normal = { textColor = Color.yellow },
                });
                BoldLabelGreen = AddCustomStyle(ref customStyles, "ExGSBoldGreen", new GUIStyle(EditorStyles.boldLabel)
                {
                    name = "ExGSBoldGreen",
                    richText = false,
                    normal = { textColor = Color.green },
                });

                ListTitle = AddCustomStyle(ref customStyles, "ExListTitle", new GUIStyle((GUIStyle)"OL Title")
                {
                    name = "ExListTitle",
                    richText = false,
                    margin = new RectOffset(5, 0, 0, 0),
                    normal = { textColor = Color.green },
                });
                ListBox = AddCustomStyle(ref customStyles, "ExListBox", new GUIStyle((GUIStyle)"PreferencesSectionBox")
                {
                    name = "ExListBox",
                    margin = new RectOffset(5, 0, 0, 0),
                });
                ListItem = AddCustomStyle(ref customStyles, "ExListItem", new GUIStyle((GUIStyle)"PreferencesKeysElement")
                {
                    name = "ExListItem",
                    margin = new RectOffset(0, 0, 0, 0),
                });
                ListItemSelected = AddCustomStyle(ref customStyles, "ExListItemSelected", new GUIStyle((GUIStyle)"PreferencesKeysElement")
                {
                    name = "ExListItemSelected",
                    hover = { background = Texture_Selected, textColor = Color.white },
                    onHover = { background = Texture_Selected, textColor = Color.white },
                    normal = { background = Texture_Selected, textColor = Color.white },
                    active = { background = Texture_Selected, textColor = Color.white },
                    onNormal = { background = Texture_Selected, textColor = Color.white },
                    onActive = { background = Texture_Selected, textColor = Color.white },
                    margin = new RectOffset(0, 0, 0, 0),
                });

                // text
                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSWarningLabel") < 0) {
                //          WarningLabelStyle = new GUIStyle (EditorStyles.label) {
                //              name = "ExGSWarningLabel",
                //              richText = false,
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? Color.yellow : Color.red) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, WarningLabelStyle);
                //      }  

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSBoldLabel") < 0) {
                //          BoldLabelStyle = new GUIStyle (EditorStyles.boldLabel) {
                //              name = "ExGSBoldLabel",
                //              richText = false,
                //              padding = new RectOffset (0, 0, 0, 0),
                //              margin = new RectOffset (3, 3, 0, 3),
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, BoldLabelStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSRichLabel") < 0) {
                //
                //          RichLabelStyle = new GUIStyle (EditorStyles.label) {
                //              name = "ExGSRichLabel",
                //              richText = true,
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, RichLabelStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSCenterLabel") < 0) {
                //          CenterLabelStyle = new GUIStyle (EditorStyles.label) {
                //              name = "ExGSCenterLabel",
                //              richText = false,
                //              alignment = TextAnchor.MiddleCenter
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, CenterLabelStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSHead1") < 0) {
                //          Head1Style = new GUIStyle (EditorStyles.boldLabel) {
                //              name = "ExGSHead1",
                //              richText = false,
                //              fontSize = 20,
                //              padding = new RectOffset (0, 0, 0, 0),
                //              margin = new RectOffset (12, 0, 3, 3),
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? new Color (0.6f, 0.6f, 0.6f) : new Color (0.5f, 0.5f, 0.5f)) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, Head1Style);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSHead2") < 0) {
                //
                //          Head2Style = new GUIStyle (EditorStyles.boldLabel) {
                //              name = "ExGSHead2",
                //              richText = false,
                //              fontSize = 16,
                //              fontStyle = FontStyle.Bold,
                //              padding = new RectOffset (0, 0, 0, 0),
                //              margin = new RectOffset (0, 0, 0, 10),
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? new Color (0.7f, 0.7f, 0.7f) : new Color (0.35f, 0.35f, 0.35f)) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, Head2Style);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSHead3") < 0) {
                //          Head3Style = new GUIStyle (EditorStyles.boldLabel) {
                //              name = "ExGSHead3",
                //              richText = false,
                //              fontSize = 15,
                //              padding = new RectOffset (0, 0, 0, 0),
                //              margin = new RectOffset (0, 0, 0, 5),
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? new Color (0.7f, 0.7f, 0.7f) : new Color (0.35f, 0.35f, 0.35f)) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, Head3Style);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSHead4") < 0) {
                //          Head4Style = new GUIStyle (Head3Style) {
                //              name = "ExGSHead4",
                //              richText = false,
                //              fontSize = 12,
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? new Color (0.7f, 0.7f, 0.7f) : new Color (0.35f, 0.35f, 0.35f)) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, Head4Style);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSLabelRight") < 0) {
                //          LabelRightStyle = new GUIStyle (Skin.label) {
                //              name = "ExGSLabelRight",
                //              richText = false,
                //              alignment = TextAnchor.MiddleRight,
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, LabelRightStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSInspectorHead") < 0) {
                //          InspectorHeadStyle = new GUIStyle (Head3Style) {
                //              name = "ExGSInspectorHead",
                //              padding = new RectOffset (15, 0, 3, 3),
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, InspectorHeadStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSInspectorHeadBox") < 0) {
                //          InspectorHeadBoxStyle = new GUIStyle (Skin.box) {
                //              name = "ExGSInspectorHeadBox",
                //              richText = false,
                //              fontSize = 13,
                //              fontStyle = FontStyle.Bold,
                //              alignment = TextAnchor.MiddleLeft,
                //              padding = new RectOffset (25, 0, 3, 3),
                //              margin = new RectOffset (0, 0, 20, 10),
                //              stretchWidth = true,
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? new Color (0.7f, 0.7f, 0.7f) : new Color (0.35f, 0.35f, 0.35f)) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, InspectorHeadBoxStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSInspectorHeadFold") < 0) {
                //          InspectorHeadFoldStyle = new GUIStyle (EditorStyles.foldout) {
                //              name = "ExGSInspectorHeadFold",
                //              richText = false,
                //              fontSize = 14,
                //              fontStyle = FontStyle.Bold,
                //              padding = new RectOffset (17, 0, 0, 0),
                //              margin = new RectOffset (10, 10, 2, 0),
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? new Color (0.7f, 0.7f, 0.7f) : new Color (0.35f, 0.35f, 0.35f)) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, InspectorHeadFoldStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSHeadingFoldout") < 0) {
                //          HeadingFoldoutStyle = new GUIStyle (EditorStyles.foldout) {
                //              name = "ExGSHeadingFoldout",
                //              richText = false,
                //              fontSize = 15,
                //              padding = new RectOffset (17, 0, 0, 0),
                //              margin = new RectOffset (0, 0, 0, 5),
                //              normal = { textColor = (EditorGUIUtility.isProSkin ? new Color (0.7f, 0.7f, 0.7f) : new Color (0.35f, 0.35f, 0.35f)) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, HeadingFoldoutStyle);
                //
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSDivider") < 0) {
                //          // boxes and such
                //          DividerStyle = new GUIStyle () {
                //              name = "ExGSDivider",
                //              border = new RectOffset (0, 0, 0, 0),
                //              padding = new RectOffset (0, 0, 0, 0),
                //              margin = new RectOffset (0, 0, 0, 0),
                //              normal = { background = EditorGUIUtility.whiteTexture },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, DividerStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSBox") < 0) {
                //          BoxStyle = new GUIStyle (Skin.box) {
                //              name = "ExGSBox",
                //              padding = new RectOffset (10, 10, 10, 10),
                //              margin = new RectOffset (5, 0, 5, 0),
                //              normal = { background = (EditorGUIUtility.isProSkin ? Texture_Box : Skin.box.normal.background) },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, BoxStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSScrollView") < 0) {
                //          ScrollViewStyle = new GUIStyle (Skin.box) {
                //              name = "ExGSScrollView",
                //              padding = new RectOffset (10, 10, 10, 10),
                //              margin = new RectOffset (5, 5, 5, 5),
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, ScrollViewStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSScrollViewNoTLMargin") < 0) {
                //          ScrollViewNoTLMarginStyle = new GUIStyle (ScrollViewStyle) {
                //              name = "ExGSScrollViewNoTLMargin",
                //              margin = new RectOffset (0, 5, 0, 5),
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, ScrollViewNoTLMarginStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSScrollViewNoTLMarginNoPadding") < 0) {
                //          ScrollViewNoMarginPaddingStyle = new GUIStyle (ScrollViewStyle) {
                //              name = "ExGSScrollViewNoTLMarginNoPadding",
                //              padding = new RectOffset (0, 0, 3, 3),
                //              margin = new RectOffset (0, 0, 0, 0),
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, ScrollViewNoMarginPaddingStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSMenuBox") < 0) {
                //
                //          // misc
                //          MenuBoxStyle = new GUIStyle (BoxStyle) {
                //              name = "ExGSMenuBox",
                //              margin = new RectOffset (1, 10, 0, 0),
                //              padding = new RectOffset (1, 1, 0, 0),
                //              normal = { background = Texture_BoxSides },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, MenuBoxStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSMenuItem") < 0) {
                //
                //          MenuItemStyle = new GUIStyle (EditorStyles.toggle) {
                //              name = "ExGSMenuItem",
                //              richText = false,
                //              fontSize = 14,
                //              alignment = TextAnchor.MiddleRight,
                //              border = new RectOffset (0, 0, 0, 0),
                //              margin = new RectOffset (0, 0, 0, 0),
                //              padding = new RectOffset (0, 10, 11, 7),
                //              normal = { background = null },
                //              hover = { background = null },
                //              active = { background = null },
                //              focused = { background = null },
                //              onNormal = { background = Texture_Selected, textColor = Color.white },
                //              onHover = { background = null },
                //              onActive = { background = Texture_Selected, textColor = Color.white },
                //              onFocused = { background = null },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, MenuItemStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSMenuHead") < 0) {
                //          MenuHeadStyle = new GUIStyle (EditorStyles.label) {
                //              name = "ExGSMenuHead",
                //              richText = false,
                //              fontSize = 14,
                //              fontStyle = FontStyle.Bold,
                //              alignment = TextAnchor.MiddleRight,
                //              border = new RectOffset (0, 0, 0, 0),
                //              margin = new RectOffset (0, 0, 0, 0),
                //              padding = new RectOffset (0, 10, 20, 3),
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, MenuHeadStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSListItemBackDark") < 0) {
                //          ListItemBackDarkStyle = new GUIStyle (Skin.button) {
                //              name = "ExGSListItemBackDark",
                //              richText = false,
                //              fontSize = 11,
                //              alignment = TextAnchor.MiddleLeft,
                //              clipping = TextClipping.Clip,
                //              stretchWidth = false,
                //              wordWrap = false,
                //              overflow = new RectOffset (0, 0, 0, 0),
                //              border = new RectOffset (1, 1, 1, 1),
                //              margin = new RectOffset (0, 0, 1, 0),
                //              hover = { background = Texture_FlatDarker },
                //              onHover = { background = Texture_FlatDarker },
                //              normal = { background = Texture_FlatDarker },
                //              active = { background = Texture_FlatDarker },
                //              onNormal = { background = Texture_FlatDarker },
                //              onActive = { background = Texture_FlatDarker },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, ListItemBackDarkStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSListItemBackLight") < 0) {
                //          ListItemBackLightStyle = new GUIStyle (ListItemBackDarkStyle) {
                //              name = "ExGSListItemBackLight",
                //              hover = { background = Texture_FlatLighter },
                //              onHover = { background = Texture_FlatLighter },
                //              normal = { background = Texture_FlatLighter },
                //              active = { background = Texture_FlatLighter },
                //              onNormal = { background = Texture_FlatLighter },
                //              onActive = { background = Texture_FlatLighter },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, ListItemBackLightStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSListItemSelected") < 0) {
                //          ListItemSelectedStyle = new GUIStyle (ListItemBackDarkStyle) {
                //              name = "ExGSListItemSelected",
                //              hover = { background = Texture_Selected, textColor = Color.white },
                //              onHover = { background = Texture_Selected, textColor = Color.white },
                //              normal = { background = Texture_Selected, textColor = Color.white },
                //              active = { background = Texture_Selected, textColor = Color.white },
                //              onNormal = { background = Texture_Selected, textColor = Color.white },
                //              onActive = { background = Texture_Selected, textColor = Color.white },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, ListItemSelectedStyle);
                //      }

                //      if (ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == "ExGSAboutLogoArea") < 0) {
                //          AboutLogoAreaStyle = new GUIStyle (Skin.box) {
                //              name = "ExGSAboutLogoArea",
                //              stretchWidth = true,
                //              margin = new RectOffset (0, 0, 0, 10),
                //              padding = new RectOffset (10, 10, 10, 10),
                //              normal = { background = EditorGUIUtility.whiteTexture },
                //          };
                //          ArrayUtility.Add<GUIStyle> (ref customStyles, AboutLogoAreaStyle);
                //      }

                Skin.customStyles = customStyles;
                //      GUI.skin = Skin;
                stylesLoaded = true;
                //              MonoBehaviour.print ("Ex: ExStyles Loaded");
            }
            catch (System.NullReferenceException)
            {

            }
            return true;
        }


        static GUIStyle AddCustomStyle(ref GUIStyle[] customStyles, string styleName, GUIStyle style)
        {
            int indexStyle = ArrayUtility.FindIndex<GUIStyle>(customStyles, s => s.name == styleName);
            if (indexStyle < 0)
            {
                ArrayUtility.Add<GUIStyle>(ref customStyles, style);
                return style;
            }
            customStyles[indexStyle] = style;
            //      return customStyles [indexStyle];
            return style;
        }

        static void LoadSkinTextures()
        {
            //      Texture_Logo = LoadEditorTexture (EditorResourcePath + "Skin/logo.png");
            //      Texture_LogoIcon = LoadEditorTexture (EditorResourcePath + "Skin/logo_icon" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            //      Texture_Box = LoadEditorTexture (EditorResourcePath + "Skin/box.png");
            //      Texture_FlatBox = LoadEditorTexture (EditorResourcePath + "Skin/flatbox" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            //      Texture_NoPreview = LoadEditorTexture (EditorResourcePath + "Skin/no_preview.png");
            //      Texture_BoxSides = LoadEditorTexture (EditorResourcePath + "Skin/boxsides" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            //      Texture_BoxSubSides = LoadEditorTexture (EditorResourcePath + "Skin/boxsides_sub" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            //      Texture_Selected = LoadEditorTexture (EditorResourcePath + "Skin/selected" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            //      Texture_FlatDarker = LoadEditorTexture (EditorResourcePath + "Skin/flat_darker" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            //      Texture_FlatLighter = LoadEditorTexture (EditorResourcePath + "Skin/flat_lighter" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            //      Texture_Plus = LoadEditorTexture (EditorResourcePath + "Icons/plus.png");
            //      Texture_Minus = LoadEditorTexture (EditorResourcePath + "Icons/minus.png");
            //      Texture_Save = LoadEditorTexture (EditorResourcePath + "Icons/save.png");
            //      Texture_Trash = LoadEditorTexture (EditorResourcePath + "Icons/trash.png"); 
        }

        #region resource loading

        public static Texture2D LoadEditorTexture(string fn)
        {
            Texture2D tx = AssetDatabase.LoadAssetAtPath(fn, typeof(Texture2D)) as Texture2D;
            if (tx == null)
            {
                Debug.LogWarning("Failed to load texture: " + fn);
            }
            else if (tx.wrapMode != TextureWrapMode.Clamp)
            {
                string path = AssetDatabase.GetAssetPath(tx);
                TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                tImporter.textureType = TextureImporterType.GUI;
                tImporter.npotScale = TextureImporterNPOTScale.None;
                tImporter.filterMode = FilterMode.Point;
                tImporter.wrapMode = TextureWrapMode.Clamp;
                tImporter.maxTextureSize = 64;
                //              tImporter.for = TextureImporterFormat.AutomaticTruecolor;
                AssetDatabase.SaveAssets();
            }
            return tx;
        }

        #endregion
    }

}
