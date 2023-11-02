using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ui : MonoBehaviour
{
    public static ui instance { get; private set; }
    public uiGrapple grapple { get; private set; }
    public uiRadar radar { get; private set; }
    public uiCrosshair crosshair { get; private set; }
    public static UnityEvent menuChanged { get; private set; }


    [SerializeField] TextMeshProUGUI 
        uiLevelNum,
        uiCheckpointNum;
    [SerializeField] GameObject[] 
        uiObjs;
    public enum menus
    {
        inGame,
        main,
        pause,
        settings
    };
    public static menus currentMenu
    {
        get
        {
            return _currentMenu;
        }
        set
        {
            _currentMenu = value;
            // can invoke code when currentMenu is changed
            //menuChanged.Invoke();
        }
    }
    public static menus _currentMenu { get; private set; } = menus.main;
    void Awake()
    {
        instance = this;
        grapple = GetComponentInChildren<uiGrapple>();
        radar = GetComponentInChildren<uiRadar>();
        crosshair = GetComponentInChildren<uiCrosshair>();
    }
    void Update()
    {

    }
    /// <summary>
    /// <para>
    /// Activates the given menu and deactivates all other menus
    /// </para>
    /// <para>Use in StartCoroutine() or this won't do anything</para>
    /// </summary>
    /// <param name="setMenu"></param>
    /// <returns></returns>
    public void ForceSetMenu(menus setMenu)
    {
        foreach (GameObject gameObject in uiObjs)
        {
            gameObject.SetActive(false);
        }
        uiObjs[(int)setMenu].SetActive(true);
        currentMenu = setMenu;
        Debug.Log("set menu: " + setMenu);
    }
}