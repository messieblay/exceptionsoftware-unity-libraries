
using UnityEngine;

public static class ColorExtensions
{

    public static Color SetRGB(this Color c, float r, float g, float b, float a)
    {
        c.r = r;
        c.g = g;
        c.b = b;
        c.a = a;
        return c;
    }


    public static Color SetR(this Color c, float r)
    {
        c.r = r;
        return c;
    }

    public static Color SetG(this Color c, float g)
    {
        c.g = g;
        return c;
    }

    public static Color SetB(this Color c, float b)
    {
        c.b = b;
        return c;
    }

    public static Color SetA(this Color c, float a)
    {
        c.a = a;
        return c;
    }

    /// <summary>
    /// Interpolacion de color. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Color Lerp(Color a, Color b, float t) => ColorHSV.LerpHSV(a.ToHSV(), b.ToHSV(), t);

}
