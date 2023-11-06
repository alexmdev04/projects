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
    [SerializeField] GameObject ui, uiDebugGroup;
    [SerializeField] bool showAllNotes;
    public bool debugMode { get; private set; }
    [SerializeField] float movementSpeed;
    int deviceFps;
    bool fpsWait;
    bool noclipEnabled, godEnabled;
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
        if (debugMode)
        {
            GetHookStats();
            debugControls();
        }
    }
    void GetRes() // gets the current resolution, refresh rate and aspect ratio
    {
        var gcd = Extensions.CalcGCD(Screen.width, Screen.height);
        uiRes.text = Screen.width.ToString() + "x" + Screen.height.ToString() + "\n" 
            + Screen.currentResolution.refreshRateRatio + "Hz" + "\n" +
            (string.Format("{0}:{1}", Screen.width / gcd, Screen.height / gcd));
    }
    void GetFPS() // fps counter
    {
        uiFPS.text = ((int)(1 / Time.unscaledDeltaTime)).ToString();
    }
    void GetHookStats() // contructs the grapple debug text, uses stringbuilder & append to slightly improve performance
    {
        uiHookStats.text = new StringBuilder()
            .Append("<u>grappleStats;</u>")
            .Append("\ngrapplePointValid = ").Append(Grapple.instance.grapplePointValid)
            .Append("\ngrapplePointCheck;\n  collisions = ").Append((Grapple.instance.grapplePointCheck != null) ? Grapple.instance.grapplePointCheck.Length : 0)
            .Append(GetGrapplePointCollisions())
            .Append("\ntargetPosition = ").Append(Grapple.instance.debugTargetPosition.x).Append(", ").Append(Grapple.instance.debugTargetPosition.y).Append(", ").Append(Grapple.instance.debugTargetPosition.z).Append(")")
            .Append("\ntargetRotation = ").Append(Grapple.instance.debugTargetRotation.x).Append(", ").Append(Grapple.instance.debugTargetRotation.y).Append(", ").Append(Grapple.instance.debugTargetRotation.z).Append(")")
            .Append("\ndistanceToTargetPosition = ").Append(Grapple.instance.debugDistanceToTargetPosition)
            .ToString();
    }
    StringBuilder GetGrapplePointCollisions() // returns the names of all objects within the player grapple point collision check
    {
        StringBuilder a = new StringBuilder("\n  names = ");
        if (Grapple.instance.grapplePointCheck == null) { return a.Append("n/a"); }
        if (Grapple.instance.grapplePointCheck.Length == 0) { return a.Append("n/a"); }
        foreach (Collider collider in Grapple.instance.grapplePointCheck) { a.Append(collider.gameObject.name).Append(", "); }
        return a;
    }
    void GetDebugNotes() // unused
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
    void debugControls() // allows for WASD movement control and scroll to change the grapple distance
    {
        // wasd movement
        transform.position += movementSpeed * Time.deltaTime * transform.TransformDirection(InputHandler.instance.input.Player.Move.ReadValue<Vector3>());

        // scroll to change hook distance
        Grapple.instance.maxDistance += Input.mouseScrollDelta.y;

        // toggle goal state
        if (Input.GetKeyDown(KeyCode.F4)) { LevelLoader.instance.levelCurrent.debugToggleGoal(); }

        // enables debug notes on screen
        if (Input.GetKeyDown(KeyCode.Insert)) { showAllNotes = true; }

        // toggles debug console
        if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Tilde)) 
        {
            uiDebugConsole.instance.gameObject.SetActive(!uiDebugConsole.instance.gameObject.activeSelf);
        }
    }
    public void ToggleNoclip()
    {
        noclipEnabled = !noclipEnabled;
        string i = noclipEnabled ? "enabled" : "disabled"; uiMessage.instance.New("noclip " + i); 
    }
    public void ToggleGod()
    {
        godEnabled = !godEnabled;
        string i = godEnabled ? "enabled" : "disabled"; uiMessage.instance.New("god mode " + i);
    }
}