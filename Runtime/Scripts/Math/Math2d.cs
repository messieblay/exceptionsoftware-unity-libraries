using UnityEngine;
public static class Math2d
{
    public static bool LineSegmentsIntersection(out Vector2 intersection, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        intersection = Vector2.zero;

        float d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        float u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        float v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        intersection.x = p1.x + u * (p2.x - p1.x);
        intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }
}
