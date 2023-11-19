using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiDebug : MonoBehaviour
{
    public static uiDebug instance { get; private set; }
    [SerializeField] TextMeshProUGUI
        uiFPS,
        uiRes,
        uiNotes,
        uiStats,
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
        InvokeRepeating(nameof(GetDebugNotes), 0f, 1f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) { debugMode = !debugMode; }
        uiDebugGroup.SetActive(debugMode);
        if (debugMode)
        {
            uiStats.text = GetStats().ToString();
            debugControls();
            GetDebugNotes();
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
    StringBuilder GetStats() // contructs all stats for the debug overlay, uses stringbuilder & append to slightly improve performance
    {
        return new StringBuilder()
            // grapple stats
            .Append("<u>Grapple;</u>")
            .Append("\ngrapplePointValid = ").Append(Grapple.instance.grapplePointValid.ToString())
            .Append("\ngrapplePointCheck;\n  collisions = ").Append(((Grapple.instance.grapplePointCheck != null) ? Grapple.instance.grapplePointCheck.Length : 0).ToString())
            .Append(", names = ").Append((Grapple.instance.grapplePointCheck != null) ? Grapple.instance.grapplePointCheck.ToStringBuilder() : "n/a")
            .Append("\ntargetPosition = ").Append(Grapple.instance.debugTargetPosition.ToStringBuilder())
            .Append("\ntargetRotation = ").Append(Grapple.instance.debugTargetRotation.ToStringBuilder())
            .Append("\ndistanceToTargetPosition = ").Append(Grapple.instance.debugDistanceToTargetPosition.ToString())
            // level stats
            .Append("\n\n").Append((LevelLoader.instance.levelCurrent != null) ? LevelLoader.instance.levelCurrent.debugGetLevelStats().ToString() : "<u>Level</u>\nNo level loaded");
    }
    void GetDebugNotes() // scans all loaded scenes and their root game objects for uiDebugNote components and combines them all
    {
        if (!showAllNotes) { return; }
        List<uiDebugNote> noteComponents = new();
        List<Scene> scenes = new List<Scene>();
        StringBuilder notesText = new("<u>Notes:</u>"), todosText = new ("<u>\nTo do:</u>");

        for (int i = 0; i < SceneManager.sceneCount; i++) { scenes.Add(SceneManager.GetSceneAt(i)); }
        foreach (Scene scene in scenes) 
        { 
            foreach (GameObject rootObject in scene.GetRootGameObjects()) 
            {
                uiDebugNote noteComponent = rootObject.GetComponent<uiDebugNote>();
                if (noteComponent != null) { noteComponents.Add(noteComponent); }
            }
        }
        foreach (uiDebugNote noteComponent in noteComponents)
        {
            if (noteComponent.notes.Length > 0)
            {
                notesText.Append("\n").Append(noteComponent.gameObject.name);
                foreach (string note in noteComponent.notes) { notesText.Append("\n - ").Append(note); }
            }

            if (noteComponent.toDos.Length > 0)
            {
                todosText.Append("\n").Append(noteComponent.gameObject.name);
                foreach (string todo in noteComponent.toDos) { todosText.Append("\n - ").Append(todo); }
            }
        }
        uiNotes.text = notesText.Append(todosText).ToString();
    }
    void debugControls() // allows for WASD movement control and scroll to change the grapple distance
    {
        // wasd movement

        Vector3 playerMovementDirection = Vector3.zero;

        bool
            w = Input.GetKey(KeyCode.W),
            a = Input.GetKey(KeyCode.A),
            s = Input.GetKey(KeyCode.S),
            d = Input.GetKey(KeyCode.D);

        // return true 
        playerMovementDirection.z = (w || s && !(w && s)) ? (w ? 1 : -1) : 0;
        playerMovementDirection.x = (a || d && !(a && d)) ? (a ? -1 : 1) : 0;

        Player.instance.transform.position += movementSpeed * Time.deltaTime * Player.instance.transform.TransformDirection(playerMovementDirection);

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
        uiMessage.instance.New("noclip " + (noclipEnabled ? "enabled" : "disabled")); 
    }
    public void ToggleGod()
    {
        godEnabled = !godEnabled;
        uiMessage.instance.New("god mode " + (godEnabled ? "enabled" : "disabled"));
    }
}