using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class ui : MonoBehaviour
{
    public static ui instance { get; private set; }
    public uiGrapple grapple { get; private set; }
    public uiRadar radar { get; private set; }
    public uiCrosshair crosshair { get; private set; }
    public static UnityEvent menuChanged { get; private set; }


    [SerializeField] TextMeshProUGUI 
        uiLevelNum,
        uiSectionNum;
    [SerializeField] GameObject 
        uiObjectivePrefab,
        uiObjectivesParent;
    [SerializeField] GameObject[] 
        uiObjs;
    public List<uiObjective> uiObjectives;
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
        LevelLoader.instance.levelLoaded.AddListener(Refresh);
    }
    void Start()
    {
        InvokeRepeating(nameof(uiObjectivesRefresh), 1f, 1f);
    }
    void Update()
    {
        radar.gameObject.SetActive(LevelLoader.instance.inLevel);
    }
    void Refresh()
    {
        uiObjectivesBuild();
        uiObjectivesRefresh();
        uiLevelNumUpdate();
        uiSectionNumUpdate();
    }
    /// <summary>
    /// Creates ui representations for all active objectives and deletes any currently present.
    /// </summary>
    public void uiObjectivesBuild()
    {
        foreach (uiObjective uiObjective in uiObjectives) { Destroy(uiObjective.gameObject); }
        uiObjectives.Clear();
        foreach (LevelObjective objective in LevelLoader.instance.levelCurrent.currentValues.objectives)
        {
            uiObjective uiObjectiveNew = Instantiate(uiObjectivePrefab, uiObjectivesParent.transform).GetComponent<uiObjective>();
            objective.uiObjective = uiObjectiveNew;
            uiObjectiveNew.objective = objective;
            uiObjectives.Add(uiObjectiveNew);
            uiObjectiveNew.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Keeps the text displayed on all uiObjectives up to date with current values
    /// </summary>
    void uiObjectivesRefresh()
    {
        if (LevelLoader.instance.inLevel)
        {
            for (int i = 0; i < LevelLoader.instance.levelCurrent.currentValues.objectives.Count; i++)
            {
                uiObjectives[i].Refresh(LevelLoader.instance.levelCurrent.currentValues.objectives[i].uiGetText());
            }
        }
    }
    /// <summary>
    /// Updates the level number text to the current levels number
    /// </summary>
    void uiLevelNumUpdate()
    {
        uiLevelNum.text = LevelLoader.instance.inLevel ? "Level " + LevelLoader.instance.levelCurrent.levelNumber + "-" + LevelLoader.instance.levelCurrent.levelDifficulty.ToString().ToUpper().ToCharArray()[0] : "";
    }
    /// <summary>
    /// Updates the section number text to the section the current level is in
    /// </summary>
    void uiSectionNumUpdate()
    {
        uiSectionNum.text = LevelLoader.instance.inLevel ? "Section 1": "";// + LevelLoader.instance.levelCurrent.sectionCurrent;
    }
    /// <summary>
    /// Activates the given menu and deactivates all other menus
    /// </summary>
    /// <param name="setMenu"></param>
    public void ForceSetMenu(menus setMenu)
    {
        foreach (GameObject gameObject in uiObjs)
        {
            gameObject.SetActive(false);
        }
        uiObjs[(int)setMenu].SetActive(true);
        currentMenu = setMenu;
        menuChanged.Invoke();
        Debug.Log("set menu: " + setMenu);
    }
}