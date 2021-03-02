using System.Collections.Generic;
using UnityEngine;

public static class ExRect
{

    public enum RectBorder
    {
        Left,
        Up,
        Right,
        Down
    }

    public static Rect Next(this Rect r, RectBorder direction)
    {
        Rect rect = new Rect(r);
        switch (direction)
        {
            case RectBorder.Down:
                rect.y += r.height;
                break;
            case RectBorder.Up:
                rect.y -= r.height;
                break;
            case RectBorder.Left:
                rect.x -= r.width;
                break;
            case RectBorder.Right:
                rect.x += r.width;
                break;
        }
        return rect;
    }

    public static Rect CopyToZero(this Rect g)
    {
        return g.Copy().ToZero();
    }

    public static Rect ToZero(this Rect g)
    {
        g.position = Vector2.zero;
        return g;
    }

    public static Rect Copy(this Rect g)
    {
        return new Rect(g);
    }

    /// <summary>
    /// Expand a copy of the specified rect and pixels. 
    /// </summary>
    /// <param name="rect">Rect.</param>
    /// <param name="pixels">Pixels.</param>
    public static Rect Expand(this Rect rect, int pixels)
    {
        Rect r = rect.Copy();
        Vector2 c = r.center;
        r.width += pixels * 2;
        r.height += pixels * 2;
        r.center = c;
        return r;
    }
    /// <summary>
    /// Expand a copy of the specified rect and pixels. 
    /// </summary>
    /// <param name="rect">Rect.</param>
    /// <param name="pixels">Pixels.</param>
    public static Rect Expand(this Rect rect, float xpixels, float ypixels)
    {
        Rect r = rect.Copy();
        Vector2 c = r.center;
        r.width += xpixels * 2;
        r.height += ypixels * 2;
        r.center = c;
        return r;
    }

    public static Rect Padding(this Rect rect, float top, float left, float bottom, float right)
    {
        Rect r = rect.Copy();
        Vector2 c = r.center;
        r.yMin -= top;
        r.yMax += bottom;
        r.xMin += left;
        r.xMax -= right;

        return r;
    }
    //public static Rect Padding(this Rect rect, float top, float left, float bottom, float right)
    //{
    //    Rect r = rect.Copy();
    //    Vector2 c = r.center;
    //    r.yMin -= top;
    //    r.yMax += bottom;
    //    r.xMin += left;
    //    r.xMax -= right;

    //    return r;
    //}

    /// <summary>
    /// 
    /// Anchors
    ///  0 1 2
    ///  3 4 5
    ///  6 7 8
    /// 
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="corner"></param>
    /// <param name="anchor"></param>
    /// <returns></returns>
    public static Rect AnchorTo(this Rect rect, int hanchor, int vanchor, Rect anchor)
    {
        Vector2 finalPosition = rect.position;
        Vector2 center = rect.center;

        // Top Left
        if (hanchor == vanchor && vanchor == 0)
        {
            return rect;
        }

        // Center
        if (hanchor == vanchor && vanchor == 1)
        {
            rect.center = anchor.center;
            return rect;
        }

        // Horizontal
        switch (hanchor)
        {
            case 0:
                finalPosition.x = anchor.xMin;
                break;
            case 1:
                finalPosition.x = anchor.center.x - (rect.width * .5f);
                break;
            case 2:
                finalPosition.x = anchor.xMax - rect.width;
                break;
        }


        // Vertical
        switch (vanchor)
        {
            case 0:
                finalPosition.y = anchor.yMin;
                break;
            case 1:
                finalPosition.y = anchor.center.y - (rect.height * .5f);
                break;
            case 2:
                finalPosition.y = anchor.yMax - rect.height;
                break;
        }

        rect.position = finalPosition;
        return rect;
    }


    /// <summary>
    /// Splits the super fixed.
    /// 
    /// </summary>
    /// <returns>The super fixed.</returns>
    /// <param name="r">The rect component.</param>
    /// <param name="option">RectBorder Option Up/Left/Down/Right </param>
    /// <param name="parts">Parts. 
    ///     Value <  0 == Percent
    ///     Value >= 1 == Fixed Pixels
    /// </param>
    public static Rect[] SplitSuperFixed(this Rect r, RectBorder option, params float[] parts)
    {
        float freespace;
        float fixedSize = 0;

        List<float> sizes = new List<float>(parts);
        int lenght = parts.Length + 1;
        Rect[] rects = new Rect[lenght];
        float fixedTemp = 0;

        for (int x = 0; x < parts.Length; x++)
        {
            if (parts[x] < 1)
            {
                //Percent
                switch (option)
                {
                    case RectBorder.Down:
                    case RectBorder.Up:
                        fixedTemp = parts[x] * r.height;
                        break;
                    case RectBorder.Left:
                    case RectBorder.Right:
                        fixedTemp = parts[x] * r.width;
                        break;
                }
            }
            else
            {
                fixedTemp = parts[x];
            }
            sizes[x] = fixedTemp;
            fixedSize += fixedTemp;
        }

        switch (option)
        {
            case RectBorder.Down:
                freespace = r.height - fixedSize;
                sizes.Reverse();
                sizes.Insert(0, freespace);
                break;
            case RectBorder.Up:
                freespace = r.height - fixedSize;
                sizes.Add(freespace);
                break;
            case RectBorder.Left:
                freespace = r.width - fixedSize;
                sizes.Add(freespace);
                break;
            case RectBorder.Right:
                freespace = r.width - fixedSize;
                sizes.Reverse();
                sizes.Insert(0, freespace);
                break;
        }

        switch (option)
        {
            case RectBorder.Down:
            case RectBorder.Up:

                for (int x = 0; x < sizes.Count; x++)
                {
                    rects[x] = new Rect(r);
                    rects[x].height = sizes[x];
                    if (x > 0)
                    {
                        rects[x].y = rects[x - 1].yMax;
                    }
                }
                break;
            case RectBorder.Left:
            case RectBorder.Right:
                for (int x = 0; x < sizes.Count; x++)
                {
                    rects[x] = new Rect(r);
                    rects[x].width = sizes[x];
                    if (x > 0)
                    {
                        rects[x].x = rects[x - 1].xMax;
                    }
                }
                break;
        }
        return rects;
    }

    public static Rect[] SplitVertical(this Rect r, params float[] parts)
    {
        return SplitSuperFixedFlexible(r, true, Vector2.zero, parts);
    }
    public static Rect[] SplitHorizontal(this Rect r, params float[] parts)
    {
        return SplitSuperFixedFlexible(r, false, Vector2.zero, parts);
    }

    public static Rect[] SplitVertical(this Rect r, Vector2 padding, params float[] parts)
    {
        return SplitSuperFixedFlexible(r, true, padding, parts);
    }
    public static Rect[] SplitHorizontal(this Rect r, Vector2 padding, params float[] parts)
    {
        return SplitSuperFixedFlexible(r, false, padding, parts);
    }


    public static Rect[] SplitSuperFixedFlexible(this Rect r, bool vertical, params float[] parts)
    {
        return SplitSuperFixedFlexible(r, vertical, Vector2.zero, parts);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="r"></param>
    /// <param name="vertical"></param>
    /// <param name="padding"></param>
    /// <param name="parts">Parts. 
    ///     Value <  0 == Flexible Space
    ///     0 < Value < 1 == Percent
    ///     Value >= 1 == Fixed Pixels
    /// </param>
    /// <returns></returns>
    public static Rect[] SplitSuperFixedFlexible(this Rect r, bool vertical, Vector2 padding, params float[] parts)
    {
        float freespace;
        float sizeFilled = 0;
        float sizeFull = 0;
        float sizeFreespace = 0;

        if (parts.Length == 1)
        {
            int splitParts = Mathf.RoundToInt(parts[0]);
            float splitsizes = 1f / parts[0];

            parts = new float[splitParts];

            for (int x = 0; x < splitParts; x++)
            {
                parts[x] = splitsizes;
            }
        }

        List<float> sizes = new List<float>(parts);
        Rect[] rects = new Rect[parts.Length];

        float fixedTemp = 0;
        int numFlexibleSpaces = 0;

        if (vertical)
        {
            sizeFull = r.height;
        }
        else
        {
            sizeFull = r.width;
        }


        for (int x = 0; x < parts.Length; x++)
        {
            if (parts[x] < 1)
            {
                if (parts[x] < 0)
                {
                    //FlexibleSpace
                    fixedTemp = -1;
                    numFlexibleSpaces++;
                }
                else
                {
                    //Percent
                    if (vertical)
                    {
                        fixedTemp = parts[x] * r.height;
                    }
                    else
                    {
                        fixedTemp = parts[x] * r.width;
                    }
                }
            }
            else
            {
                //Num parts
                fixedTemp = parts[x];
            }

            sizes[x] = fixedTemp;

            if (fixedTemp > 0)
            {
                sizeFilled += fixedTemp;
            }
        }

        //Calculo de espacio libre. Calculan el tamaño maximo y el tamano ocupado, despues el espacio Flexible y se reparte en los huecos
        freespace = sizeFull - sizeFilled;
        sizeFreespace = freespace / numFlexibleSpaces;
        for (int x = 0; x < sizes.Count; x++)
        {
            if (sizes[x] < 0)
            {
                sizes[x] = sizeFreespace;
            }
        }

        if (vertical)
        {
            for (int x = 0; x < sizes.Count; x++)
            {
                rects[x] = new Rect(r);
                rects[x].height = sizes[x];
                if (x > 0)
                {
                    rects[x].y = rects[x - 1].yMax;
                }
                if (padding.x != 0 && padding.y != 0)
                    rects[x] = rects[x].Expand(-padding.x, -padding.y);
            }
        }
        else
        {
            for (int x = 0; x < sizes.Count; x++)
            {
                rects[x] = new Rect(r);
                rects[x].width = sizes[x];
                if (x > 0)
                {
                    rects[x].x = rects[x - 1].xMax;
                }
                if (padding.x != 0 && padding.y != 0)
                    rects[x] = rects[x].Expand(-padding.x, -padding.y);
            }
        }


        return rects;
    }


    public static Rect[][] SplitGrid(this Rect r, int columns, int rows)
    {
        Rect[][] rects = new Rect[columns][];
        float xsize = r.width / columns;
        float ysize = r.height / rows;
        for (float x = 0; x < r.width; x += xsize)
        {
            rects[(int)x] = new Rect[rows];
            for (float y = 0; y < r.height; y += ysize)
            {
                rects[(int)x][(int)y] = new Rect(r.x + x, r.y + y, xsize, ysize);
            }
        }
        return rects;
    }

    #region Setters
    public static Rect SetPosition(this Rect rect, Vector2 position)
    {
        rect.position = position;
        return rect;
    }

    public static Rect SetSize(this Rect rect, Vector2 size)
    {
        rect.size = size;
        return rect;
    }

    public static Rect SetPosition(this Rect rect, float x, float y)
    {
        rect.x = x;
        rect.y = y;
        return rect;
    }

    public static Rect SetSize(this Rect rect, float width, float height)
    {
        rect.width = width;
        rect.height = height;
        return rect;
    }


    public static Rect SetWidth(this Rect rect, float width)
    {
        rect.width = width;
        return rect;
    }
    public static Rect SetHeight(this Rect rect, float height)
    {
        rect.height = height;
        return rect;
    }
    public static Rect SetX(this Rect rect, float x)
    {
        rect.x = x;
        return rect;
    }
    public static Rect SetY(this Rect rect, float y)
    {
        rect.y = y;
        return rect;
    }


    public static Rect IncSize(this Rect rect, Vector2 offset)
    {
        return IncSize(rect, offset.x, offset.y);
    }

    public static Rect IncSize(this Rect rect, float width, float height)
    {
        rect.width += width;
        rect.height += height;
        return rect;
    }

    public static Rect IncWidth(this Rect rect, float width)
    {
        rect.width += width;
        return rect;
    }
    public static Rect IncHeight(this Rect rect, float height)
    {
        rect.height += height;
        return rect;
    }
    public static Rect IncX(this Rect rect, float x)
    {
        rect.x += x;
        return rect;
    }
    public static Rect IncY(this Rect rect, float y)
    {
        rect.y += y;
        return rect;
    }

    public static Rect IncPosition(this Rect rect, Vector2 offset)
    {
        return IncPosition(rect, offset.x, offset.y);
    }

    public static Rect IncPosition(this Rect rect, float x, float y)
    {
        rect.x += x;
        rect.y += y;
        return rect;
    }

    #endregion

    #region Obsolete
    /*
    [System.Obsolete("Use SplitSuperFixedFlexible instead")]
    public static Rect[] SpliHorizontal(this Rect r, params float[] percent)
    {
        return SplitSuperFixed(r, RectBorder.Left, percent);
    }


    [System.Obsolete("Use SplitSuperFixedFlexible instead")]
    public static Rect[] SpliVertical(this Rect r, params float[] percent)
    {
        return SplitSuperFixed(r, RectBorder.Up, percent);
    }

    [System.Obsolete("Use SplitSuperFixedFlexible instead")]
    public static Rect[] SplitHorizontal(this Rect r, int parts)
    {
        float percent = 1f / (float)parts;
        float[] percents = new float[parts];
        for (int x = 0; x < percents.Length; x++)
        {
            percents[x] = percent;
        }
        return SplitHorizontal(r, percents);
    }

    [System.Obsolete("Use SplitSuperFixedFlexible instead")]
    public static Rect[] SplitHorizontal(this Rect r, params float[] percent)
    {
        Rect[] rects = new Rect[percent.Length];
        float w = r.width / percent.Length;
        bool isRegular = percent == null || percent.Length == 0;
        for (int x = 0; x < percent.Length; x++)
        {
            rects[x] = new Rect(r);
            if (isRegular)
            {
                rects[x].width = w;
            }
            else
            {
                rects[x].width = r.width * percent[x];
            }
            if (x > 0)
                rects[x].x = rects[x - 1].xMax;
        }
        return rects;
    }

    [System.Obsolete("Use SplitSuperFixedFlexible instead")]
    public static Rect[] SplitVertical(this Rect r, int parts)
    {
        float percent = 1f / (float)parts;
        float[] percents = new float[parts];
        for (int x = 0; x < percents.Length; x++)
        {
            percents[x] = percent;
        }
        return SplitVertical(r, percents);
    }

    [System.Obsolete("Use SplitSuperFixedFlexible instead")]
    public static Rect[] SplitVertical(this Rect r, params float[] percent)
    {
        Rect[] rects = new Rect[percent.Length];
        float w = r.height / percent.Length;
        bool isRegular = percent == null || percent.Length == 0;
        for (int x = 0; x < percent.Length; x++)
        {
            rects[x] = new Rect(r);
            if (isRegular)
            {
                rects[x].height = w;
            }
            else
            {
                rects[x].height = r.height * percent[x];
            }
            if (x > 0)
                rects[x].y = rects[x - 1].yMax;
        }
        return rects;
    }


    [System.Obsolete("Use SplitSuperFixedFlexible instead")]
    public static Rect[] CenterLayout(this Rect r, float[] left, float[] right)
    {
        float freespace;
        float fixedSize = 0;

        List<float> sizes = new List<float>();
        int lenght = left.Length + right.Length + 1;
        Rect[] rects = new Rect[lenght];

        float fixedTemp = 0;
        int indexCenter = 0;
        for (int x = 0; x < left.Length; x++)
        {
            if (left[x] < 1)
            {
                fixedTemp = left[x] * r.width;

            }
            else
            {
                fixedTemp = left[x];
            }
            sizes.Add(fixedTemp);
            fixedSize += fixedTemp;
        }
        indexCenter = sizes.Count;
        for (int x = 0; x < right.Length; x++)
        {
            if (left[x] < 1)
            {
                fixedTemp = left[x] * r.width;

            }
            else
            {
                fixedTemp = left[x];
            }
            sizes.Add(fixedTemp);
            fixedSize += fixedTemp;
        }
        freespace = r.width - fixedSize;
        sizes.Insert(indexCenter, freespace);

        for (int x = 0; x < sizes.Count; x++)
        {
            rects[x] = new Rect(r);
            rects[x].width = sizes[x];
            if (x > 0)
            {
                rects[x].x = rects[x - 1].xMax;
            }
        }
        return rects;
    }
    */
    #endregion


}
