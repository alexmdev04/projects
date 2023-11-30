using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ui : MonoBehaviour
{
    public static ui instance { get; private set; }
    public uiGrapple grapple { get; private set; }
    public uiRadar radar { get; private set; }
    public uiCrosshair crosshair { get; private set; }
    public uiMenuLevel menuLevel { get; private set; }
    public static UnityEvent menuChanged { get; private set; }
    public List<uiObjective> 
        uiObjectives;
    public bool 
        uiFadeToBlack;
    [SerializeField] TextMeshProUGUI 
        uiLevelNum,
        uiSectionNum;
    [SerializeField] GameObject 
        uiObjectivePrefab,
        uiObjectivesParent;
    [SerializeField] GameObject[] 
        uiObjs;
    [SerializeField] Image 
        uiFade;
    [SerializeField] float 
        uiFadeInSpeed,
        uiFadeOutSpeed;


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
    const string
        uiLevelNumText = "Level ",
        uiSectionNumText = "Section ",
        uiLevelNumDashText = "-";
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
        uiFadeUpdate();
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
    public void uiObjectivesDestroy()
    {
        foreach (uiObjective uiObjective in uiObjectives) { Destroy(uiObjective.gameObject); }
        uiObjectives.Clear();
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
                LevelLoader.instance.levelCurrent.currentValues.objectives[i].uiObjectiveRefresh();
            }
        }
    }
    /// <summary>
    /// Updates the level number text to the current levels number
    /// </summary>
    void uiLevelNumUpdate()
    {
        uiLevelNum.text = LevelLoader.instance.inLevel ? uiLevelNumText + LevelLoader.instance.levelCurrent.levelNumber + uiLevelNumDashText + LevelLoader.instance.levelCurrent.levelDifficulty.ToString().ToUpper().ToCharArray()[0] : string.Empty;
    }
    /// <summary>
    /// Updates the section number text to the section the current level is in
    /// </summary>
    public void uiSectionNumUpdate()
    {
        if (LevelLoader.instance.inLevel)
        {
            uiSectionNum.text = (LevelLoader.instance.levelCurrent.SectionCount() > 1) ? uiSectionNumText + LevelLoader.instance.levelCurrent.SectionNum() : string.Empty;
        }
    }
    void uiFadeUpdate()
    {
        float 
            newValue = 0,
            fadeSpeed = 1;
        if (uiFadeToBlack && uiFade.color.a != 1)
        {
            newValue = 1;
            fadeSpeed = uiFadeInSpeed;
        }
        else if (!uiFadeToBlack && uiFade.color.a != 0)
        {
            newValue = -1;
            fadeSpeed = uiFadeOutSpeed;
        }
        uiFade.color = new Color(0, 0, 0, Mathf.MoveTowards(uiFade.color.a, newValue, Time.deltaTime * fadeSpeed));
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