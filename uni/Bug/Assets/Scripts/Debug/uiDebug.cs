using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class uiDebug : MonoBehaviour
{
    public static uiDebug instance { get; private set; }
    [SerializeField] TextMeshProUGUI
        uiFPS,
        uiRes,
        uiTodo,
        uiNotes,
        uiHookStats,
        uiVersion;
    public GameObject ui, uiDebugGroup;
    public bool showAllNotes;
    public bool debugMode { get; private set; }
    int deviceFps;
    bool fpsWait;
    void Awake()
    {
        instance = this;
        uiVersion.text = "v" + Application.version;

        #if UNITY_EDITOR
        debugMode = true;
        #endif
    }
    void Start()
    {
        InvokeRepeating(nameof(GetRes), 0f, 1f);
        InvokeRepeating(nameof(GetFPS), 0f, 0.2f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) { debugMode = !debugMode; }
        uiDebugGroup.SetActive(debugMode);
        if (Input.GetKeyDown(KeyCode.Insert)) { showAllNotes = true; }
        if (debugMode) { GetHookStats(); }
    }

    void GetRes()
    {
        var gcd = Extensions.CalcGCD(Screen.width, Screen.height);
        uiRes.text = Screen.width.ToString() + "x" + Screen.height.ToString() + "\n" 
            + Screen.currentResolution.refreshRateRatio + "Hz" + "\n" +
            (string.Format("{0}:{1}", Screen.width / gcd, Screen.height / gcd));
    }
    void GetFPS()
    {
        uiFPS.text = ((int)(1 / Time.unscaledDeltaTime)).ToString();
    }
    void GetHookStats()
    {
        uiHookStats.text = new StringBuilder()
            .Append("<u>hookStats;</u>")
            .Append("\nvalidLocation = ").Append(PlayerHook.instance.playerHookValidLocation)
            .Append("\noverlapSphere;\n  collisions = ").Append((PlayerHook.instance.playerHookPointCheck != null) ? PlayerHook.instance.playerHookPointCheck.Length : 0)
            .Append(GetHookPointCollisions())
            .Append("\ntargetPosition = ").Append(PlayerHook.instance.debugTargetPosition.x).Append(", ").Append(PlayerHook.instance.debugTargetPosition.y).Append(", ").Append(PlayerHook.instance.debugTargetPosition.z).Append(")")
            .Append("\ntargetRotation = ").Append(PlayerHook.instance.debugTargetRotation.x).Append(", ").Append(PlayerHook.instance.debugTargetRotation.y).Append(", ").Append(PlayerHook.instance.debugTargetRotation.z).Append(")")
            .Append("\ndistanceToTargetPosition = ").Append(PlayerHook.instance.debugDistanceToTargetPosition)
            .ToString();
    }
    StringBuilder GetHookPointCollisions()
    {
        StringBuilder a = new StringBuilder("\n  names = ");
        if (PlayerHook.instance.playerHookPointCheck == null) { return a.Append("n/a"); }
        if (PlayerHook.instance.playerHookPointCheck.Length == 0) { return a.Append("n/a"); }
        foreach (Collider collider in PlayerHook.instance.playerHookPointCheck) { a.Append(collider.gameObject.name).Append(", "); }
        return a;
    }
    void GetDebugNotes()
    {
        List<uiDebugNote> notes = ui.GetComponentsInChildren<uiDebugNote>().ToList();
        uiNotes.text = "notes:";
        uiTodo.text = "todo here:";
        notes.AddRange(Player.instance.gameObject.GetComponents<uiDebugNote>().ToList());            
        foreach (uiDebugNote note in notes)
        {
            if (note.gameObject.activeSelf || showAllNotes)
            {
                uiNotes.text += "\n" + note.note;
                uiTodo.text += "\n" + note.toDo; 
            }
        }
    }
}
