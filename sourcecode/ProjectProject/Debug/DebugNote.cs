using UnityEngine;

public static class Extension
{
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
}

public class DebugNote : MonoBehaviour
{
    public string[] notes, toDos;
    [System.NonSerialized] 
    public string note, toDo;
    void Awake()
    {
        note = note.getDebugNote(notes, gameObject);
        toDo = toDo.getDebugNote(toDos, gameObject);
        //if (notes.Length != 0) { foreach (string _note in notes) { note += _note + "\n"; } }
        //if (toDos.Length != 0) { foreach (string _todo in toDos) { toDo += _todo + "\n"; } }
    }
}