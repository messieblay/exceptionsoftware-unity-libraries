using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectionFlowGrid : ExEditorComponent
{
    static float _headerHeight = 18f;
    //static float _maxHeightComponent = 350f;
    static float _footerHeight = 1f;
    static float _margin = 2f;
    //static float _pixelMargin = 3f;

    public Vector2 panelPosition;

    string _title = "";
    List<string> _listContent = null;
    List<string> _filter = null;
    GUIStyle _style = null;

    #region Actions
    public System.Action<string> onClickAction = null;
    List<ActionEntry> _dropdownActions = new List<ActionEntry>();

    class ActionEntry
    {
        public string name;
        public System.Action action;
        public void DoAction()
        {
            action.TryInvoke();
        }

        public override string ToString()
        {
            return name;
        }
    }

    public void AddMenuAction(string title, System.Action action)
    {
        if (!_dropdownActions.Exists(s => s.name == title))
        {
            _dropdownActions.Add(new ActionEntry() { name = title, action = action });
        }
    }
    #endregion


    List<Rect> _rects = new List<Rect>();
    List<string> _filteredContent = new List<string>();


    //Debug Data
    int _rows = 1;

    //Temp data
    string _internalFilter;
    string _currentText;
    float _rowHeightWithMargins = 0;
    Rect currentRect = new Rect();
    Vector2 _currentSize = Vector2.zero;
    Event _e;
    Vector2 _componentSize;

    Rect _headerRect;
    Rect _actionDropRect;
    Rect _searchFieldRect;
    Rect _contentRect;
    Rect _footerRect;

    ExSearchField _searchField;

    //GUIStyle titleStyle = null;
    public bool boldTitle = false;

    public SelectionFlowGrid(string title, List<string> listContent, List<string> filter, GUIStyle buttonStyle)
    {
        this._title = title;
        this._listContent = listContent;
        this._filter = filter;
        this._style = buttonStyle;
        this._currentSize = _style.CalcSize(new GUIContent("wee"));
        this._rowHeightWithMargins = _currentSize.y + _margin;

        _searchField = new ExSearchField(false, true);
        _searchField.onSearchChanged += delegate (string searchFilter)
        {
            _internalFilter = searchFilter;
        };

        AddMenuAction("Action", null);
    }

    public void DoLayoutFlowGrid()
    {
        _e = Event.current;
        DoFilterContent();

        //if (_e.type == EventType.Repaint)
        {
            _headerRect = GUILayoutUtility.GetRect(0f, _headerHeight, new GUILayoutOption[]
            {
                    GUILayout.ExpandWidth(true)
            });

            //_headerRect.width -= (float)_style.margin.left + _style.fixedWidth + 2;
            if (_headerRect.width > 1)
            {
                _componentSize = new Vector2(_headerRect.width, 0);
                float height = CalculeComponentHeight();
                _componentSize.y = height;

                Rect[] rs = _headerRect.SplitSuperFixed(ExRect.RectBorder.Right, 200, 10, 50);
                _actionDropRect = rs[1];
                _searchFieldRect = rs[3];
                _searchFieldRect.y += 2;
            }

            _contentRect = GUILayoutUtility.GetRect(10f, _componentSize.y, new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinHeight(_rowHeightWithMargins)
            });

            _footerRect = GUILayoutUtility.GetRect(4f, _footerHeight, new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            });

            _footerRect.width = _contentRect.width = _headerRect.width;
        }

        DoDrawHeader();
        DoLayout();
        DoDraw();
    }

    void DoFlowGrid(Rect r)
    {
        _e = Event.current;
        DoFilterContent();

        _componentSize = r.size;

        Rect[] rs = r.SplitSuperFixed(ExRect.RectBorder.Up, _headerHeight);
        _headerRect = rs[0];
        _headerRect.width -= (float)_style.margin.left + _style.fixedWidth + 2;
        _searchFieldRect = _headerRect.SplitSuperFixed(ExRect.RectBorder.Left, EditorStyles.boldLabel.CalcSize(new GUIContent(_title)).x)[1];

        rs = rs[1].SplitSuperFixed(ExRect.RectBorder.Down, _footerHeight);

        _contentRect = rs[0];
        _footerRect = rs[1];

        _footerRect.width = _contentRect.width = _headerRect.width;

        DoDrawHeader();
        DoLayout();
        DoDraw();
    }
    void DoFilterContent()
    {
        _filteredContent.Clear();

        for (int x = 0; x < _listContent.Count; x++)
        {
            if (_filter != null && _filter.Count > 0 && ContainsLower(_filter, _listContent[x]))
            {
                continue;
            }
            if (_internalFilter != null && _internalFilter.Length > 0 && !_listContent[x].ToLower().Contains(_internalFilter.ToLower()))
            {
                continue;
            }
            _filteredContent.Add(_listContent[x]);
        }
    }

    float CalculeComponentHeight()
    {
        float curX = 0;
        float curWidth = 0;
        float totalHeight = 0;
        int rows = 1;
        //_rects.Clear();
        for (int x = 0; x < _filteredContent.Count; x++)
        {
            curWidth = _style.CalcSize(new GUIContent(_filteredContent[x])).x;
            curX += curWidth;
            if (_componentSize.x <= curX)
            {
                curX = curWidth;
                totalHeight += _rowHeightWithMargins;
                rows++;
            }
        }

        //if (/*_e.type == EventType.Layout &&*/ _headerRect.width <= 0)
        {
            _rows = rows;
            if (_rows * _rowHeightWithMargins != totalHeight)
            {
                totalHeight = _rows * _rowHeightWithMargins;
            }
        }
        return totalHeight;
    }

    void DoLayout()
    {
        Vector2 currentPos = new Vector2(0, _contentRect.y);

        //Este vector definira los limites del componente para despues reservar el espacio
        //Inicialmente comtemplara los 3 pixeles de margen del titulo y una row como minimo
        _rects.Clear();
        for (int x = 0; x < _filteredContent.Count; x++)
        {
            _currentText = _filteredContent[x];

            _currentSize = _style.CalcSize(new GUIContent(_currentText));
            currentRect = new Rect(new Vector2(currentPos.x, currentPos.y + 1), _currentSize);

            currentPos.x = currentRect.xMax + _margin;

            if (currentRect.width > 1 && currentRect.xMax >= _componentSize.x)
            {
                currentPos.y += _rowHeightWithMargins;
                currentPos.x = 0;
                currentRect = new Rect(currentPos, _currentSize);
                currentPos.x = currentRect.xMax + _margin;
            }

            _rects.Add(currentRect);
        }
    }

    void DoDrawHeader()
    {
        if (_e.type == EventType.Repaint)
        {
            GUI.Label(_headerRect, "", EditorStyles.toolbar);
            GUI.Label(_headerRect, _title, boldTitle ? EditorStyles.boldLabel : EditorStyles.label);
        }

        _searchField.DoSearch(_searchFieldRect);

        if (_dropdownActions.Count > 1)
        {
            int indexAction = EditorGUI.Popup(_actionDropRect, 0, _dropdownActions.ToStringArray(), EditorStyles.toolbarDropDown);
            if (indexAction > -1)
            {
                _dropdownActions[indexAction].DoAction();
            }
        }
    }

    void DoDraw()
    {

        if (_e.type == EventType.Repaint)
        {
            //GUI.Box(_contentRect, "", (GUIStyle)"CurveEditorBackground");
            //GUI.Box(_contentRect, "");
        }

        for (int x = 0; x < _filteredContent.Count; x++)
        {
            if (_e.type == EventType.Repaint)
                GUI.Label(_rects[x], _filteredContent[x], _style);

            if (_e.type == EventType.MouseDown && (_e.button == 0 && GUI.enabled))
            {
                if (_rects[x].Contains(_e.mousePosition))
                {
                    _e.Use();

                    if (onClickAction != null)
                    {
                        onClickAction(_filteredContent[x]);
                    }
                }
            }
        }

        if (_e.type == EventType.MouseDown && GUI.enabled)
        {
            GUI.FocusControl("");
            _isDirty = true;
        }
    }

    public bool ContainsLower(List<string> list, string text)
    {
        return list.Exists(s => s.ToLower().Contains(text.ToLower()));
    }

}
