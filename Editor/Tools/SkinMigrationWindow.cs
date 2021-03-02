using ExSoftware.ExEditor;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class SkinMigrationWindow : ExWindow<SkinMigrationWindow>
{
    // Unity Menu item
    [MenuItem("Window/ExSoftware/Styles/Skin Migration", false, 1000)]
    static void OpenMainShaderGraph()
    {

        SkinMigrationWindow currentWindow = (SkinMigrationWindow)SkinMigrationWindow.GetWindow<SkinMigrationWindow>(false);
        currentWindow.minSize = new Vector2(400, 270);
        currentWindow.position = new Rect(new Vector2(200, 200), new Vector2(350, 600));
        currentWindow.wantsMouseMove = true;
        currentWindow.titleContent.text = "Migrate Skin";
        currentWindow.Show();
    }
    GUIDActions _currentAction = GUIDActions.Migrate;
    public enum GUIDActions
    {
        ManageSkin = 0,
        Migrate
    }

    GUISkin _skinOriginal = null, _skinTarget = null;

    public override void DoGUI()
    {

        EditorGUI.BeginChangeCheck();
        _skinOriginal = EditorGUILayout.ObjectField("Original", _skinOriginal, typeof(GUISkin), false) as GUISkin;
        if (EditorGUI.EndChangeCheck())
        {
            EnableList();
        }

        if (_skinOriginal == null)
        {
            ExGUI.HelpBoxWarning("Necesitar asignar un GUISkin");
            return;
        }

        _currentAction = (GUIDActions)GUILayout.Toolbar((int)_currentAction, System.Enum.GetNames(typeof(GUIDActions)));
        ExGUIx.ScrollView("Scroll", delegate
        {
            switch (_currentAction)
            {
                case GUIDActions.Migrate:
                    DrawMigration();
                    break;
                case GUIDActions.ManageSkin:
                    DrawManageStyles();
                    break;
                    //case GUIDActions.AddDefault:
                    //    DrawAddDefault();
                    //    break;


            }

        });
        Repaint();
    }


    #region Reorder
    UnityEditorInternal.ReorderableList roorderList = null;
    string _tempName;


    void EnableList()
    {
        if (_skinOriginal == null)
        {
            DisableList(); return;
        }
        _selectedStyleIndex = -1;
        _selectedStyle = null;

        roorderList = new UnityEditorInternal.ReorderableList(_skinOriginal.customStyles, typeof(GUIStyle));

        roorderList.drawElementCallback += delegate (Rect rect, int index, bool isActive, bool isFocused)
        {
            if (_skinOriginal.customStyles.Length <= index) return;
            Rect r = rect.Copy();
            r.width -= 20;

            EditorGUI.BeginChangeCheck();
            _tempName = EditorGUI.DelayedTextField(r, _skinOriginal.customStyles[index].name);
            if (EditorGUI.EndChangeCheck())
            {
                _skinOriginal.customStyles[index].name = _tempName;
            }

            r.x = r.xMax;
            r.width = 20;

            GUI.color = Color.red;
            if (GUI.Button(r, ""))
            {
                RemoveCustomStyle(_skinOriginal, index);
            }
            GUI.color = Color.white;
        };
        roorderList.onSelectCallback += delegate (UnityEditorInternal.ReorderableList list)
        {
            _selectedStyleIndex = list.index;
            _selectedStyle = _skinOriginal.customStyles[_selectedStyleIndex];
        };
    }
    void DisableList()
    {
        roorderList = null;
    }
    MonoScript script;
    void DrawManageStyles()
    {
        if (roorderList == null && _skinOriginal != null) EnableList();
        if (roorderList == null) return;

        ExGUI.Title("Add Buildt In Styles");
        DrawAddDefault();

        ExGUI.Title("Reorder");
        ExGUI.Horizontal(delegate ()
        {
            script = (MonoScript)EditorGUILayout.ObjectField(script, typeof(MonoScript), false);
            if (GUILayout.Button("ReOrder", EditorStyles.miniButtonLeft))
            {
                try
                {
                    ReorderByEnum(script.GetClass());
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex);
                }
            }
            if (GUILayout.Button("Add From Enum", EditorStyles.miniButtonRight))
            {
                try
                {
                    AddFromEnum(script.GetClass());
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        });


        EditorGUILayout.Space();
        roorderList.DoLayoutList();
    }
    //List<GUIStyle> _selectedStyles = new List<GUIStyle>();
    GUIStyle _selectedStyle = null;
    int _selectedStyleIndex = 0;
    void ReorderByEnum(System.Type enumType)
    {
        if (enumType == null) return;

        string[] names = System.Enum.GetNames(script.GetClass());
        for (int x = 0; x < names.Length; x++)
        {
            int index = FindIndex(_skinOriginal, names[x]);
            if (index > -1)
            {
                SwitchCustomStyle(_skinOriginal, x, index);
            }
        }

    }

    void AddFromEnum(System.Type enumType)
    {
        if (enumType == null) return;

        string[] names = System.Enum.GetNames(script.GetClass());
        for (int x = 0; x < names.Length; x++)
        {
            int index = FindIndex(_skinOriginal, names[x]);
            if (index == -1)
            {
                GUIStyle newstyle = new GUIStyle();
                newstyle.name = names[x];
                AddCopyCustomStyle(_skinOriginal, newstyle);
            }
        }

    }

    #endregion
    #region Add default
    string _styleTarget;
    string _styleEditorTarget;
    void DrawAddDefault()
    {
        ExGUI.Horizontal(delegate ()
        {
            EditorGUI.BeginChangeCheck();
            _styleTarget = EditorGUILayout.TextField("BuiltIn Style", _styleTarget);
            if (EditorGUI.EndChangeCheck())
            {
                _styleTarget = _styleTarget.Replace("(", "").Replace(")", "").Replace("GUIStyle", "");
            }
            if (GUILayout.Button("Paste", EditorStyles.miniButtonLeft))
            {
                _styleTarget = GUIUtility.systemCopyBuffer.Replace("\"", "").Replace("(GUIStyle)", "");
                GUI.FocusControl(null);
            }


            if (GUILayout.Button("Add", EditorStyles.miniButtonRight))
            {
                GUIStyle style = new GUIStyle((GUIStyle)_styleTarget);
                if (style != null && style.name != "")
                {
                    AddCopyCustomStyle(_skinOriginal, style);
                }
            }
        });
        ExGUI.Horizontal(delegate ()
        {
            _styleEditorTarget = EditorGUILayout.TextField("Editor Style", _styleEditorTarget);

            GUI.enabled = _styleEditorTarget != "";
            if (GUILayout.Button("Add", EditorStyles.miniButton))
            {
                System.Type editors = typeof(EditorStyles);
                PropertyInfo[] fs = typeof(EditorStyles).GetProperties(BindingFlags.Public | BindingFlags.Static);

                fs = typeof(EditorStyles).GetProperties(BindingFlags.Public | BindingFlags.Static).Where(s => s.Name == _styleEditorTarget).ToArray();
                PropertyInfo fields = typeof(EditorStyles).GetProperties(BindingFlags.Public | BindingFlags.Static).Where(s => s.Name == _styleEditorTarget).FirstOrDefault();
                if (fields == null) return;
                GUIStyle st = (GUIStyle)fields.GetValue(null, null);
                GUIStyle style = new GUIStyle(st);
                if (style != null)
                {
                    AddCopyCustomStyle(_skinOriginal, style);
                }
            }
            GUI.enabled = true;
        });

        ExGUI.Horizontal(delegate ()
        {
            GUI.enabled = _selectedStyle != null;
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(_selectedStyle != null ? "Replace " + _selectedStyle.name : "Replace", EditorStyles.miniButton))
            {
                _styleTarget = GUIUtility.systemCopyBuffer.Replace("\"", "").Replace("(GUIStyle)", "");
                GUIStyle style = new GUIStyle((GUIStyle)_styleTarget);
                CopyDataCustomStyle(_selectedStyleIndex, style);
            }
            GUI.enabled = true;
        });
    }
    #endregion
    #region Migration

    void DrawMigration()
    {
        _skinTarget = EditorGUILayout.ObjectField("Target", _skinTarget, typeof(GUISkin), false) as GUISkin;
        if (_skinOriginal == null || _skinTarget == null)
            return;
        ExGUI.Group("Styles", delegate ()
        {
            ExGUI.ButtonGo("Copy All:", delegate ()
            {
                CopyAllRegularStyles();
                SaveSkin(_skinTarget);
            });

            ExGUI.Horizontal(delegate ()
            {
                EditorGUILayout.LabelField("Copy styles names to Clipboard");
                GUILayout.FlexibleSpace();
                ExGUI.Button("Original", EditorStyles.miniButtonLeft, delegate ()
                {
                    CopyAllNamesToClipboar(_skinOriginal);
                });
                ExGUI.Button("Target", EditorStyles.miniButtonRight, delegate ()
                {
                    CopyAllNamesToClipboar(_skinTarget);
                });
            });

        });
        ExGUI.Group("Custom Styles", delegate ()
        {
            ExGUI.ButtonGo("Copy All:", delegate ()
            {
                _skinTarget.customStyles = _skinOriginal.customStyles;
                SaveSkin(_skinTarget);
            });
            ExGUI.ButtonGo("Update All:", delegate ()
            {
                ActionsToAll(true);
                SaveSkin(_skinTarget);
            });

            ExGUIx.ScrollView("CustomStyles", delegate ()
            {

                for (int x = 0; x < _skinOriginal.customStyles.Length; x++)
                {
                    ExGUI.Horizontal(delegate ()
                    {
                        ExGUI.Button("X", EditorStyles.miniButtonLeft, delegate ()
                        {
                            RemoveCustomStyle(_skinOriginal.customStyles[x].name);
                            SaveSkin(_skinTarget);
                        }, GUILayout.Width(30), GUILayout.Height(20));

                        if (_skinTarget.FindStyle(_skinOriginal.customStyles[x].name) == null)
                        {
                            ExGUI.Button(EditorGUIUtility.FindTexture("Animation.Play"), EditorStyles.miniButtonRight, delegate ()
                            {
                                AddCopyCustomStyle(_skinOriginal.customStyles[x].name);
                                SaveSkin(_skinTarget);
                            }, GUILayout.Width(30), GUILayout.Height(20));
                        }
                        else
                        {
                            ExGUI.Button(EditorGUIUtility.FindTexture("RotateTool"), EditorStyles.miniButtonRight, delegate ()
                            {
                                ReplaceCustomStyle(_skinOriginal.customStyles[x].name);
                                SaveSkin(_skinTarget);
                            }, GUILayout.Width(30), GUILayout.Height(20));
                        }

                        EditorGUILayout.LabelField(_skinOriginal.customStyles[x].name);
                    });
                }
            });
        });
    }
    #endregion
    GUIStyle[] _customSkins;

    void CopyAllRegularStyles()
    {
        _skinTarget.box = _skinOriginal.box;
        _skinTarget.button = _skinOriginal.button;
        _skinTarget.toggle = _skinOriginal.toggle;
        _skinTarget.label = _skinOriginal.label;
        _skinTarget.window = _skinOriginal.window;
        _skinTarget.textField = _skinOriginal.textField;
        _skinTarget.textArea = _skinOriginal.textArea;
        _skinTarget.horizontalSlider = _skinOriginal.horizontalSlider;
        _skinTarget.horizontalSliderThumb = _skinOriginal.horizontalSliderThumb;
        _skinTarget.verticalSlider = _skinOriginal.verticalSlider;
        _skinTarget.verticalSliderThumb = _skinOriginal.verticalSliderThumb;
        _skinTarget.horizontalScrollbar = _skinOriginal.horizontalScrollbar;
        _skinTarget.horizontalScrollbarLeftButton = _skinOriginal.horizontalScrollbarLeftButton;
        _skinTarget.horizontalScrollbarRightButton = _skinOriginal.horizontalScrollbarRightButton;
        _skinTarget.verticalScrollbar = _skinOriginal.verticalScrollbar;
        _skinTarget.verticalScrollbarDownButton = _skinOriginal.verticalScrollbarDownButton;
        _skinTarget.verticalScrollbarThumb = _skinOriginal.verticalScrollbarThumb;
        _skinTarget.verticalScrollbarUpButton = _skinOriginal.verticalScrollbarUpButton;
        _skinTarget.scrollView = _skinOriginal.scrollView;
        _skinTarget.font = _skinOriginal.font;
    }

    void RemoveCustomStyle(string name)
    {
        int index = ArrayUtility.FindIndex(_skinTarget.customStyles, s => s.name == name.ToString());
        if (index > -1)
        {
            _customSkins = _skinTarget.customStyles;
            RemoveCustomStyle(_skinTarget, index);
        }
    }
    void RemoveCustomStyle(GUISkin skin, int index)
    {
        if (index > -1)
        {
            _customSkins = skin.customStyles;
            ArrayUtility.RemoveAt(ref _customSkins, index);
            skin.customStyles = _customSkins;
        }
    }

    void AddCopyCustomStyle(string name)
    {
        GUIStyle style = ArrayUtility.Find(_skinOriginal.customStyles, s => s.name == name.ToString());
        AddCopyCustomStyle(_skinTarget, style);
    }
    void AddCopyCustomStyle(GUISkin skin, GUIStyle style)
    {
        if (style != null)
        {
            _customSkins = skin.customStyles;
            ArrayUtility.Add(ref _customSkins, style);
            skin.customStyles = _customSkins;
        }
    }
    void CopyDataCustomStyle(int index, GUIStyle newdata)
    {
        GUIStyle style = _skinOriginal.customStyles[index];
        _skinOriginal.customStyles[index] = new GUIStyle(newdata);
        _skinOriginal.customStyles[index].name = style.name;
    }
    void ReplaceCustomStyle(string name)
    {
        GUIStyle style = ArrayUtility.Find(_skinOriginal.customStyles, s => s.name == name.ToString());
        int index = ArrayUtility.FindIndex(_skinTarget.customStyles, s => s.name == name.ToString());
        if (style != null && index > -1)
        {
            _skinTarget.customStyles[index] = style;
        }
    }

    int FindIndex(GUISkin skin, string name)
    {
        return ArrayUtility.FindIndex(skin.customStyles, s => s.name == name.ToString());
    }
    void SwitchCustomStyle(GUISkin skin, int indexa, int indexb)
    {
        GUIStyle style = skin.customStyles[indexa];
        skin.customStyles[indexa] = skin.customStyles[indexb];
        skin.customStyles[indexb] = style;
    }
    void ActionsToAll(bool doReplace)
    {
        bool styleFound = false;
        for (int x = 0; x < _skinOriginal.customStyles.Length; x++)
        {
            styleFound = _skinTarget.FindStyle(_skinOriginal.customStyles[x].name) != null;
            if (styleFound && doReplace)
            {
                ReplaceCustomStyle(_skinOriginal.customStyles[x].name);
            }
            if (!styleFound && !doReplace)
            {
                AddCopyCustomStyle(_skinOriginal.customStyles[x].name);
            }
        }
        //      _skinTarget.a

        SaveSkin(_skinTarget);
    }

    void SaveSkin(GUISkin skin)
    {
        EditorUtility.SetDirty(skin);
        AssetDatabase.SaveAssets();
    }

    void CopyAllNamesToClipboar(GUISkin origin)
    {
        //      System.Windows.
        string s = "";
        for (int x = 0; x < origin.customStyles.Length; x++)
        {
            s += origin.customStyles[x].name + ",\n";
        }
        GUIUtility.systemCopyBuffer = s;
        Debug.Log("List of custom styles copied to clipboard");
        //      System.Windows.Forms.Clipboard.SetText (s);
    }
}
