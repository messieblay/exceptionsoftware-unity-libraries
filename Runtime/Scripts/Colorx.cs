using UnityEngine;

public static class Colorx
{
    /// <summary>
    /// Interpolacion de color. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Color Lerp(Color a, Color b, float t) => LerpHSV(a.ToHSV(), b.ToHSV(), t);
    public static Color LerpHSV(ColorHSV a, ColorHSV b, float t)
    {
        // Hue interpolation
        float h = 0;
        float d = b.h - a.h;
        if (a.h > b.h)
        {
            // Swap (a.h, b.h)
            var h3 = b.h;
            b.h = a.h;
            a.h = h3;

            d = -d;
            t = 1 - t;
        }

        if (d > 0.5) // 180deg
        {
            a.h = a.h + 1; // 360deg
            h = (a.h + t * (b.h - a.h)) % 1; // 360deg
        }
        if (d <= 0.5) // 180deg
        {
            h = a.h + t * d;
        }

        // Interpolates the rest
        return new ColorHSV
        (
        h, // H
        a.s + t * (b.s - a.s), // S
        a.v + t * (b.v - a.v), // V
        a.a + t * (b.a - a.a) // A
        );
    }
}
