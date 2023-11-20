using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Sets this transform and all its children to the specified layer
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    public static void SetChildLayers(this Transform t, int layer)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            SetChildLayers(child, layer);
        }
    }
    public static Quaternion ReflectRotation(this Quaternion source, Vector3 normal)
    {
        return Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
    }
    /// <summary>
    /// Converts a time given in seconds to days, hours, minutes and seconds (e.g. 1d 19h 35m 7s)
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string ConvertTime(this double seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds((int)seconds);
        return
            (ts.Days > 0 ? ts.Days.ToString() + "d " : "") +
            (ts.Hours > 0 ? ts.Hours.ToString() + "h " : "") +
            (ts.Minutes > 0 ? ts.Minutes.ToString() + "m " : "") +
            (ts.Seconds > 0 ? ts.Seconds.ToString() + "s " : "0s");
    }
    /// <summary>
    /// Calculates the greatest common denominator of a and b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float CalcGCD(float a, float b)
    {
        while (a != 0f && b != 0f)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }
        if (a == 0f)
            return b;
        else
            return a;
    }
    /// <summary>
    /// Converts a Vector3 to a StringBuilder containing the values like this (x, y, z)
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static StringBuilder ToStringBuilder(this Vector3 vector)
    {
        return new StringBuilder().Append("(")
            .Append(vector.x.ToString()).Append(", ")
            .Append(vector.y.ToString()).Append(", ")
            .Append(vector.z.ToString()).Append(")");
    }
    /// <summary>
    /// Converts an array of Colliders to a StringBuilder containing a comma-separated list of all the Collider's GameObject's names
    /// </summary>
    /// <param name="colliders"></param>
    /// <param name="ifNullText"></param>
    /// <param name="ifEmptyText"></param>
    /// <returns></returns>
    public static StringBuilder ToStringBuilder(this Collider[] colliders, string ifNullText = "n/a", string ifEmptyText = "n/a")
    {
        StringBuilder a = new();
        if (colliders == null) { return a.Append(ifNullText); }
        if (colliders.Length == 0) { return a.Append(ifEmptyText); }
        foreach (Collider collider in colliders) { if (collider != null) { a.Append(collider.gameObject.name).Append(", "); } }
        return a;
    }
    public static bool AllCharsAreDigits(this char[] chars)
    {
        List<bool> isDigitBools = new();
        foreach (char _char in chars) { isDigitBools.Add(char.IsDigit(_char)); }
        return isDigitBools.All(x => x);
    }
}