using UnityEngine;

public class uiDebugNote : MonoBehaviour
{
    public string[] notes, toDos;
    [System.NonSerialized] 
    public string note, toDo;
    void Awake()
    {
        note = note.GetDebugNote(notes, gameObject);
        toDo = toDo.GetDebugNote(toDos, gameObject);
        //if (notes.Length != 0) { foreach (string _note in notes) { note += _note + "\n"; } }
        //if (toDos.Length != 0) { foreach (string _todo in toDos) { toDo += _todo + "\n"; } }
    }
}