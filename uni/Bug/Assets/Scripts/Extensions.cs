using System;
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
    public static string GetDebugNote(this string _this, string[] input, GameObject gameObject)
    {
        if (input.Length != 0)
        {
            _this += "---- " + gameObject.name + " ----\n";
            foreach (string _string in input) { _this += _string + "\n"; }
            return _this;
        }
        return "";
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
}