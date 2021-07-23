using System;
using System.Linq;


public static class StringExtensions
{
    public static string ToSentence(this string str) => string.Concat(str.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

}
