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
        uiWeaponStats,
        uiPlayerArmsStats,
        uiTodo,
        uiNotes,
        uiPlayerStats,
        uiVersion;
    public GameObject ui, uiDebugGroup;
    public bool showAllNotes;
    int deviceFps;
    bool fpsWait;
    [SerializeField] GameObject
        particleTestSpawn,
        particle1,
        particle1Instance,
        particle2,
        particle2Instance,
        particle3,
        particle3Instance;
    //string timePlayedSavedStr = "0s";
    //string timePlayedCurrentStr = "0s";
    void Awake()
    {
        instance = this;
        uiVersion.text = "v" + Application.version;
    }
    void Start()
    {
        //menuHandler.menuChanged.AddListener(menuChanged);
        InvokeRepeating(nameof(getRes), 0f, 1f);
        //InvokeRepeating(nameof(getTimePlayed), 0f, 1f);
        //InvokeRepeating(nameof(getDebugNotes), 0f, 1f);
        InvokeRepeating(nameof(getFPS), 0f, 0.2f);
        StartCoroutine(getWeaponStats());
        StartCoroutine(getPlayerArmsStats());
    }
    void Update()
    {
        //getStats();
        if (Input.GetKeyDown(KeyCode.Insert)) { showAllNotes = true; }

        if (Input.GetKey(KeyCode.F4) && particle1Instance == null) { particle1Instance = Instantiate(particle1, particleTestSpawn.transform); }
        else if (!Input.GetKey(KeyCode.F4) && particle1Instance != null) { Destroy(particle1Instance); }

        if (Input.GetKey(KeyCode.F5) && particle2Instance == null) { particle2Instance = Instantiate(particle2, particleTestSpawn.transform); }
        else if (!Input.GetKey(KeyCode.F5) && particle2Instance != null) { Destroy(particle2Instance); }

        if (Input.GetKey(KeyCode.F6) && particle3Instance == null) { particle3Instance = Instantiate(particle3, particleTestSpawn.transform); }
        else if (!Input.GetKey(KeyCode.F6) && particle3Instance != null) { Destroy(particle3Instance); }
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
    // void getTimePlayed() // seperated from getStats to reduce overhead
    // {
    //     timePlayedCurrentStr = convertTime(gameHandler.instance.timePlayedSec);
    //     timePlayedSavedStr = convertTime(fileHandler.timePlayedSec);
    // }
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
    IEnumerator getWeaponStats()
    {
        while (true)
        {
            if (Player.instance.weaponEquipped != null)
            {
                uiWeaponStats.text = Player.instance.weaponEquipped.weaponDebug().ToString();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator getPlayerArmsStats()
    {
        while (true)
        {
            uiPlayerArmsStats.text = PlayerArms.instance.playerArmsDebug().ToString();
            yield return new WaitForEndOfFrame();
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
