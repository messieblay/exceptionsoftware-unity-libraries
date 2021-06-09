using UnityEngine;

public static class VectorExtensions
{

    /// <summary>
    /// Convert Vector2 to Vector3
    /// </summary>
    /// <param name="v">V.</param>
    public static Vector3 To3(this Vector2 v) => new Vector3(v.x, v.y, 0);
    /// <summary>
    /// Convert Vector2 to Vector3 (X,Y) => (X,0,Z)
    /// </summary>
    /// <param name="v">V.</param>
    public static Vector3 ToForward3D(this Vector2 v) => new Vector3(v.x, 0, v.y);
    /// <summary>
    /// Convert Vector2 to Vector3 (X,Y) => (X,0,Z)
    /// </summary>
    /// <param name="v">V.</param>
    public static Vector2 ToForward2D(this Vector3 v) => new Vector2(v.x, v.z);

    /// <summary>
    /// Move the specified vector in x, y and z.
    /// </summary>
    /// <param name="x">The x coordinate. v.x+=x;</param>
    /// <param name="y">The y coordinate. v.y+=y;</param>
    /// <param name="z">The z coordinate. v.z+=z;</param>
    public static Vector3 Move(this Vector3 v, float x, float y, float z)
    {
        v.x += x;
        v.y += y;
        v.z += z;
        return v;
    }

    /// <summary>
    /// Move the specified vector in vMove
    /// </summary>
    /// <param name="vMove">The x coordinate. v+=vMove;</param>
    public static Vector3 Move(this Vector3 v, Vector3 vMove)
    {
        v.x += vMove.x;
        v.y += vMove.y;
        v.z += vMove.z;
        return v;
    }

    /// <summary>
    /// Move the specified vector in x and y.
    /// </summary>
    /// <param name="x">The x coordinate. v.x+=x;</param>
    /// <param name="y">The y coordinate. v.y+=y;</param>
    public static Vector2 Move(this Vector2 v, float x, float y)
    {
        v.x += x;
        v.y += y;
        return v;
    }

    /// <summary>
    /// Move the specified vector in vMove
    /// </summary>
    /// <param name="vMove">The x coordinate. v+=vMove;</param>
    public static Vector2 Move(this Vector2 v, Vector2 vMove)
    {
        v.x += vMove.x;
        v.y += vMove.y;
        return v;
    }

    public static Vector2 SetX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 SetX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 SetY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 SetY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 SetZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    /// <summary>
    /// To key. x,y,z,w
    /// </summary> 
    public static string ToKey(this Vector4 v) => $"{v.x},{v.y},{v.z},{v.w}";

    /// <summary>
    /// To key. x,y,z
    /// </summary> 
    public static string ToKey(this Vector3 v) => $"{v.x},{v.y},{v.z}";

    /// <summary>
    /// To key. x,y
    /// </summary> 
    public static string ToKey(this Vector2 v) => $"{v.x},{v.y}";

    /// <summary>
    /// Determines if the specified v is Vector3.zero
    /// </summary>
    /// <returns><c>true</c> if is zero the specified v; otherwise, <c>false</c>.</returns>
    public static bool IsZero(this Vector3 v) => v.x == 0 && v.y == 0 && v.z == 0;

    /// <summary>
    /// Determines if the specified v is Vector2.zero
    /// </summary>
    /// <returns><c>true</c> if is zero the specified v; otherwise, <c>false</c>.</returns>
    public static bool IsZero(this Vector2 v) => v.x == 0 && v.y == 0;

    /// <summary>
    /// Round the specified vector with decimals.
    /// </summary>
    /// <param name="decimals">Round Decimals.</param>
    public static Vector3 Round(this Vector3 v, int decimals)
    {
        if (decimals < 0)
        {
            return v;
        }
        float round = Mathf.Pow(10, decimals);
        v.x = Mathf.Round(v.x * round) / round;
        v.y = Mathf.Round(v.y * round) / round;
        v.z = Mathf.Round(v.z * round) / round;
        return v;
    }

    /// <summary>
    /// Copy the specified v.
    /// </summary>
    public static Vector3 Copy(this Vector3 v) => new Vector3(v.x, v.y, v.z);
    public static int CompareTo(this Vector3 v, Vector3 other)
    {
        int x = v.x.CompareTo(other.x);
        int y = v.y.CompareTo(other.y);
        if (x == 0)
        {
            if (y == 0)
            {
                return v.z.CompareTo(other.z);
            }
            else
            {
                return y;
            }
        }
        else
        {
            return x;
        }
    }

    static int Vector3Compare(Vector3 v0, Vector3 v1)
    {
        int x = v0.x.CompareTo(v1.x);
        int y = v0.y.CompareTo(v1.y);

        if (x == 0)
        {
            if (y == 0)
            {
                return v0.z.CompareTo(v1.z);
            }
            else
            {
                return y;
            }
        }
        else
        {
            return x;
        }
    }

    public static Vector2 Create(int x, int y) => new Vector2(x, y);
    public static Vector3 Create(int x, int y, int z) => new Vector3(x, y, z);
    public static Vector4 Create(int x, int y, int z, int w) => new Vector4(x, y, z, w);
    public static Vector2 Create(float x, float y) => new Vector2((int)x, (int)y);
    public static Vector3 Create(float x, float y, float z) => new Vector3((int)x, (int)y, (int)z);
    public static Vector4 Create(float x, float y, float z, float w) => new Vector4((int)x, (int)y, (int)z, (int)w);

}
