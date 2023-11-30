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
    [SerializeField] GameObject uiDebugGroup;
    [SerializeField] bool showAllNotes;
    public bool debugMode { get; private set; }
    [SerializeField] float movementSpeed;
    int deviceFps;
    bool fpsWait;
    bool noclipEnabled, godEnabled;

    [SerializeField] Enemy enemy;

    public const string
        // player
        str_playerTitle = "<u>Player/Game;</u>",
        str_targetFramerate = "\ntargetFramerate = ",
        str_mouseRotation = "\nmouseRotation = ",
        str_lookSensitivity = "\nlookSensitivity = ",
        str_playerDimensions = "\nplayerRadius = ",
        str_multiply = " * ",
        // grapple
        str_grappleTitle = "\n\n<u>Grapple;</u>",
        str_maxDistance = "\nmaxDistance = ",
        str_currentDistance = "\ncurrentDistance = ",
        str_playerMoving = "\nplayerMoving = ",
        str_grapplePointValid = "\ngrapplePointValid = ",
        str_grapplePointCheck = "\ngrapplePointCheck;\n  collisions = ",
        str_grapplePointCheckNames = ", names = ",
        str_targetPosition = "\ntargetPosition = ",
        str_targetRotation = "\ntargetRotation = ",
        str_distanceToTargetPosition = "\ndistanceToTargetPosition = ",
        str_notApplicable = "n/a",
        // level
        str_levelTitle = "\n\n<u>Level;</u>\ninLevel = ",
        str_assetKey = "\nassetKey = ",
        str_inGameName = "\ninGameName = ",
        str_objectives = "\nobjectives; ",
        str_playerStartPos = "\nplayerStartPos = ",
        //other
        str_x = "x",
        str_v = "v",
        str_dash = " - ",
        str_equals = " = ",
        str_divide = " / ",
        str_NewLine = "\n",
        str_NewLineDash = "\n - ",
        str_Hz = "Hz",
        str_ResFormat = "{0}:{1}",
        str_noclip = "noclip ",
        str_god = "god mode ",
        str_enabled = "enabled",
        str_disabled = "disabled",
        str_notesTitle = "<u>Notes:</u>",
        str_todoTitle = "<u>\nTo do:</u>";

    void Awake()
    {
        instance = this;
        uiVersion.text = str_v + Application.version;

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
            Controls();
        }
    }
    void GetRes() // gets the current resolution, refresh rate and aspect ratio
    {
        var gcd = Extensions.CalcGCD(Screen.width, Screen.height);
        uiRes.text = Screen.width.ToString() + str_x + Screen.height.ToString() + str_NewLine
            + Screen.currentResolution.refreshRateRatio + str_Hz + str_NewLine +
            (string.Format(str_ResFormat, Screen.width / gcd, Screen.height / gcd));
    }
    void GetFPS() // fps counter
    {
        uiFPS.text = ((int)(1 / Time.unscaledDeltaTime)).ToString();
    }
    StringBuilder GetStats() // contructs all stats for the debug overlay, uses stringbuilder & append to slightly improve performance
    {
        return
            // player stats
            Player.instance.debugGetStats()
            // grapple stats
            .Append(Grapple.instance.debugGetStats()
            // level stats
            // if a level is loaded then its stats will be grabbed, otherwise return the value of inLevel
            .Append(str_levelTitle).Append(LevelLoader.instance.inLevel).Append(LevelLoader.instance.levelCurrent != null ? LevelLoader.instance.levelCurrent.debugGetStats().ToString() : string.Empty));
    }
    void GetDebugNotes() // scans all loaded scenes and their root game objects for uiDebugNote components and combines them all
    {
        if (!showAllNotes) { return; }
        if (!debugMode) { return; }
        List<uiDebugNote> noteComponents = new();
        List<Scene> scenes = new List<Scene>();
        StringBuilder notesText = new(str_notesTitle), todosText = new (str_todoTitle);

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
                notesText.Append(str_NewLine).Append(noteComponent.gameObject.name);
                foreach (string note in noteComponent.notes) { notesText.Append(str_NewLineDash).Append(note); }
            }

            if (noteComponent.toDos.Length > 0)
            {
                todosText.Append("\n").Append(noteComponent.gameObject.name);
                foreach (string todo in noteComponent.toDos) { todosText.Append(str_NewLineDash).Append(todo); }
            }
        }
        uiNotes.text = notesText.Append(todosText).ToString();
    }
    void Controls() // allows for WASD movement control and scroll to change the grapple distance
    {
        // wasd movement
        Player.instance.transform.position += movementSpeed * Time.deltaTime * Player.instance.transform.TransformDirection(new(FloatFromAxis(Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A)), 0, FloatFromAxis(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S))));

        // scroll to change hook distance
        float scrollY = Input.mouseScrollDelta.y;
        if (scrollY != 0) { Grapple.instance.debugMaxDistanceEdit(scrollY); }

        // enables debug notes on screen
        if (Input.GetKeyDown(KeyCode.Insert)) { showAllNotes = true; }

        // toggles debug console
        if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Tilde)) 
        {
            uiDebugConsole.instance.gameObject.SetActive(!uiDebugConsole.instance.gameObject.activeSelf);
        }
        
        // toggle experimental torch
        if (Input.GetKeyDown(KeyCode.F))
        {
            Player.instance.debugToggleTorch();
        }
    }
    public void ToggleNoclip()
    {
        noclipEnabled = !noclipEnabled;
        uiMessage.instance.New(str_noclip + (noclipEnabled ? str_enabled : str_disabled)); 
    }
    public void ToggleGod()
    {
        godEnabled = !godEnabled;
        uiMessage.instance.New(str_god + (godEnabled ? str_enabled : str_disabled));
    }
    private float FloatFromAxis(bool positive, bool negative) =>
        (positive, negative) switch
        {
            (true, false) => 1f,
            (false, true) => -1f,
            _ => 0f
        };
}