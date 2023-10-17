using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
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
    public bool debugMode;
    int deviceFps;
    bool fpsWait;
    void Awake()
    {
        instance = this;
        uiVersion.text = "v" + Application.version;
    }
    void Start()
    {
        InvokeRepeating(nameof(getRes), 0f, 1f);
        InvokeRepeating(nameof(getFPS), 0f, 0.2f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) { debugMode = !debugMode; }
        if (Input.GetKeyDown(KeyCode.Insert)) { showAllNotes = true; }
        getHookStats();
    }

    void getRes()
    {
        var gcd = calcGCD(Screen.width, Screen.height);
        uiRes.text = Screen.width.ToString() + "x" + Screen.height.ToString() + "\n" 
            + Screen.currentResolution.refreshRateRatio + "Hz" + "\n" +
            (string.Format("{0}:{1}", Screen.width / gcd, Screen.height / gcd));
    }
    void getFPS()
    {
        uiFPS.text = ((int)(1 / Time.unscaledDeltaTime)).ToString();
    }
    void getHookStats()
    {
        uiHookStats.text = new StringBuilder()
            .Append("<u>hookStats;</u>")
            .Append("\nvalidLocation = ").Append(PlayerHook.instance.playerHookValidLocation)
            .Append("\noverlapSphere;\n  collisions = ").Append((PlayerHook.instance.playerHookPointCheck != null) ? PlayerHook.instance.playerHookPointCheck.Length : 0)
            .Append(getHookPointCollisions())
            .Append("\ntargetPosition = ").Append(PlayerHook.instance.debugTargetPosition.x).Append(", ").Append(PlayerHook.instance.debugTargetPosition.y).Append(", ").Append(PlayerHook.instance.debugTargetPosition.z).Append(")")
            .Append("\ntargetRotation = ").Append(PlayerHook.instance.debugTargetRotation.x).Append(", ").Append(PlayerHook.instance.debugTargetRotation.y).Append(", ").Append(PlayerHook.instance.debugTargetRotation.z).Append(")")
            .Append("\ndistanceToTargetPosition = ").Append(PlayerHook.instance.debugDistanceToTargetPosition)
            .ToString();
    }
    StringBuilder getHookPointCollisions()
    {
        StringBuilder a = new StringBuilder("\n  names = ");
        if (PlayerHook.instance.playerHookPointCheck == null) { return a.Append("n/a"); }
        if (PlayerHook.instance.playerHookPointCheck.Length == 0) { return a.Append("n/a"); }
        foreach (Collider collider in PlayerHook.instance.playerHookPointCheck) { a.Append(collider.gameObject.name).Append(", "); }
        return a;
    }
    void getDebugNotes()
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
    /// <summary>
    /// Converts a time given in seconds to days, hours, minutes and seconds (e.g. 1d 19h 35m 7s)
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string convertTime(float seconds)
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
    public float calcGCD(float a, float b)
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
