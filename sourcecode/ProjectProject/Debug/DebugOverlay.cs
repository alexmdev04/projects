using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    public static DebugOverlay instance { get; private set; }
    [SerializeField] TextMeshProUGUI
        uiFPS,
        uiRes,
        uiWeaponStats,
        uiPlayerArmsStats,
        uiTodo,
        uiNotes,
        uiPlayerStats,
        uiVersion;
    public GameObject mainParent;
    public bool showAllNotes;
    int deviceFps;
    bool fpsWait;
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
        List<DebugNote> notes = mainParent.GetComponentsInChildren<DebugNote>().ToList();
        uiNotes.text = "notes:";
        uiTodo.text = "todo here:";
        notes.AddRange(Player.instance.gameObject.GetComponents<DebugNote>().ToList());            
        foreach (DebugNote note in notes)
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
            StringBuilder weaponDebugStatsGet;
            Player.weaponSlots weaponSlot = Player.instance.weaponSlotEquipped;
            weaponDebugStatsGet = Player.instance.weaponEquipped.getWeaponDebugStats();
            //switch (weaponSlot)
            //{
            //    case Player.weaponSlots.weaponPrimary:
            //        {
            //            weaponDebugStatsGet = Player.instance.weaponPrimary.getWeaponDebugStats();
            //            break;
            //        }
            //    case Player.weaponSlots.weaponSecondary:
            //        {
            //            weaponDebugStatsGet = Player.instance.weaponSecondary.getWeaponDebugStats();
            //            break;
            //        }
            //    case Player.weaponSlots.weaponMelee:
            //        {
            //            weaponDebugStatsGet = Player.instance.weaponMelee.getWeaponDebugStats();
            //            break;
            //        }
            //    case Player.weaponSlots.weaponEquipment1:
            //        {
            //            weaponDebugStatsGet = Player.instance.weaponEquipment1.getWeaponDebugStats();
            //            break;
            //        }
            //    case Player.weaponSlots.weaponEquipment2:
            //        {
            //            weaponDebugStatsGet = Player.instance.weaponEquipment2.getWeaponDebugStats();
            //            break;
            //        }
            //    case Player.weaponSlots.weaponEquipment3:
            //        {
            //            weaponDebugStatsGet = Player.instance.weaponEquipment3.getWeaponDebugStats();
            //            break;
            //        }
            //    default:
            //        {
            //            weaponDebugStatsGet = new StringBuilder();
            //            break;
            //        }
            //}
            StringBuilder weaponDebugStats = 
                new StringBuilder().Append("weapon;\nequippedSlot: ").Append(Enum.GetName(weaponSlot.GetType(), weaponSlot))
                .Append("\n").Append(weaponDebugStatsGet);
            uiWeaponStats.text = weaponDebugStats.ToString();
            yield return new WaitForEndOfFrame();
            
        }
    }

    IEnumerator getPlayerArmsStats()
    {
        while (true)
        {
            StringBuilder playerArmsDebugStats = 
                new StringBuilder()
                .Append("<u>playerArms;</u>")
                .Append("\ndriftPosition;")
                .Append("\n    Input: ").Append(PlayerArms.instance.playerArmsDrift.debugDriftPositionInput)
                .Append("\n    Target: ").Append(PlayerArms.instance.playerArmsDrift.debugDriftPositionTarget)
                .Append("\n    Output: ").Append(PlayerArms.instance.playerArmsDrift.debugDriftPositionOutput)
                .Append("\n    Speed: ").Append(PlayerArms.instance.playerArmsDrift.debugDriftPositionSpeed)
                .Append("\ndriftRotation;")
                .Append("\n    Target: ").Append(PlayerArms.instance.playerArmsDrift.debugDriftRotationTarget)
                .Append("\n    Output: ").Append(PlayerArms.instance.playerArmsDrift.debugDriftRotationOutput)
                .Append("\n    Speed: ").Append(PlayerArms.instance.playerArmsDrift.debugDriftRotationSpeed);

            uiPlayerArmsStats.text = playerArmsDebugStats.ToString();
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
