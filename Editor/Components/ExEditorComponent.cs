
public class ExEditorComponent
{
    protected bool _isDirty = true;
    protected bool _forceRepaint = true;

    public bool IsDirty { get { return _isDirty; } set { _isDirty = value; } }
    public bool ForceRepaint { get { return _forceRepaint; } set { _forceRepaint = value; } }
}
