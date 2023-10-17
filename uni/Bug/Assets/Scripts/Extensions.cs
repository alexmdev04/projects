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
    public static string getDebugNote(this string _this, string[] input, GameObject gameObject)
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
}