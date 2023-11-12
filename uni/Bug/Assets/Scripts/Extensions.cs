using System;
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
    public static string ConvertTime(float seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds((int)seconds);

        if (ts.Days > 0)
        //{ return ts.Days + "d " + ts.Hours + "h " + ts.Minutes + "m " + ts.Seconds + "s"; }
        { return string.Format("{0}d {1}h {2}m {3}s ", ts.Days, ts.Hours, ts.Minutes, ts.Seconds); }

        else if (ts.Hours > 0)
        //{ return ts.Hours + "h " + ts.Minutes + "m " + ts.Seconds + "s"; }
        { return string.Format("{0}h {1}m {2}s ", ts.Hours, ts.Minutes, ts.Seconds); }

        else if (ts.Minutes > 0)
        //{ return ts.Minutes + "m " + ts.Seconds + "s"; }
        { return string.Format("{0}m {1}s ", ts.Minutes, ts.Seconds); }

        else //{ return ts.Seconds + "s"; }
        { return string.Format("{0}s ", ts.Seconds); }
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
        if (Grapple.instance.grapplePointCheck == null) { return a.Append(ifNullText); }
        if (Grapple.instance.grapplePointCheck.Length == 0) { return a.Append(ifEmptyText); }
        foreach (Collider collider in Grapple.instance.grapplePointCheck) { a.Append(collider.gameObject.name).Append(", "); }
        return a;
    }
}