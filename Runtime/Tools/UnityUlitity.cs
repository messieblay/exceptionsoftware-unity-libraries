using UnityEngine;

public static class UnityUtility
{

    public static Transform TryFindChildOrCreate(Transform parent, string objname)
    {
        Transform t = parent.Find(objname);
        if (t == null)
        {
            t = (new GameObject(objname)).transform;
            t.parent = parent;
        }
        return t;
    }

}
