using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

public class ui : MonoBehaviour
{
    public static ui instance;
    [SerializeField] GameObject[] uiObjs;
    public static UnityEvent menuChanged { get; private set; }
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
    /// <summary>
    /// <para>
    /// Activates the given menu and deactivates all other menus
    /// </para>
    /// <para>Use in StartCoroutine() or this won't do anything</para>
    /// </summary>
    /// <param name="menuObjects"></param>
    /// <param name="setMenu"></param>
    /// <returns></returns>
    void Awake()
    {
        instance = this;
    }
    void Update()
    {

    }
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