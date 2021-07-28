using System;
using System.Linq;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

    /// <summary>
    /// Parses the sentence.
    /// RecordadQueArturoEsUnPipa -> Recordad que arturo es un pipa
    /// </summary>
    /// <returns>The sentence.</returns>
    /// <param name="text">Text.</param>
    public static string ToSpacesSentence(this string str) => string.Concat(str.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

    /// <summary>
    /// Tos the sentence.
    /// Recordad que arturo es un pipa -> RecordadQueArturoEsUnPipa
    /// </summary>
    /// <returns>The sentence.</returns>
    /// <param name="text">Text.</param>
    public static string ToNoSpacesSentence(this string text, bool forzeLower = false)
    {
        string finalString = string.Empty;

        foreach (var s in text.Trim().Split(' '))
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (i == 0)
                {
                    finalString += char.ToUpper(s[i]);
                }
                else
                {
                    if (forzeLower)
                    {
                        finalString += char.ToLower(s[i]);
                    }
                    else
                    {
                        finalString += s[i];
                    }
                }
            }
        }
        return finalString;
    }

    /// <summary>
    /// Normaliza una cadena de texto para guardar el fichero
    /// Quita todos los acentos y Ñ de un string
    /// </summary>
    /// <returns>The file string.</returns>
    /// <param name="text">Text.</param>
    public static string ToFileString(this string text) => QuitAccentsAndN(text);

    public static string QuitAccentsAndN(this string texto)
    {
        string con = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜÑçÇ";
        string sin = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUNcC";
        for (int i = 0; i < con.Length; i++)
        {
            texto = texto.Replace(con[i], sin[i]);
        }
        return texto;
    }

}
