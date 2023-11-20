using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public enum levelDifficultiesEnum
    {
        easy,
        normal,
        hard
    }
    [System.Serializable] public struct DifficultySpecificValues
    {
        public float grappleMaxDistance;
        public List<LevelObjective> objectives;
    }
    [System.Serializable] public struct Section
    {
        public GameObject parent;
        [Tooltip("The player will be teleported here when the section is started")] public GameObject playerStartPos;
        public LevelGoal goal;
    }

    [Header("Level Attributes")]
    public string assetKey;
    public string inGameName;
    public levelDifficultiesEnum levelDifficulty = levelDifficultiesEnum.normal;

    [Header("Difficulties")]
    public DifficultySpecificValues easyValues;
    public DifficultySpecificValues normalValues;
    public DifficultySpecificValues hardValues;
    public DifficultySpecificValues currentValues { get; private set; }

    [Header("References")]
    [SerializeField] List<Section> sections = new();

    public Section currentSection { get; private set; }
    [HideInInspector] public string levelNumber;

    void Awake()
    {
        // levelNumber is only used for the radar, if the assetKey format is Level1 then it will be 1, otherwise it will be the full asset key
        levelNumber = assetKey[..4].ToLower() == "level" ? (assetKey.ToCharArray()[5..].AllCharsAreDigits() ? assetKey.ToCharArray()[5..].ToString() : assetKey) : assetKey;
    }
    void Start() // sets the difficulty, currentSection to the first section, activates all objectives and enables grapple movement
    {
        Debug.Log(assetKey + " Start()");
        switch (levelDifficulty)
        {
            case levelDifficultiesEnum.easy:
                {
                    currentValues = easyValues;
                    break;
                }
            case levelDifficultiesEnum.normal:
                {
                    currentValues = normalValues;
                    break;
                }
            case levelDifficultiesEnum.hard:
                {
                    currentValues = hardValues;
                    break;
                }
        }
        currentSection = sections[0];
        SectionStart(0);
        foreach (LevelObjective obj in currentValues.objectives) { obj.Start(); }
        Grapple.instance.SetMovementActive(true);
    }
    void Update() // checks if the goal should be unlocked
    {
        currentSection.goal.goalUnlocked = ObjectivesValidate();
    }
    bool ObjectivesValidate()
    {
        List<bool> bools = new();
        foreach (LevelObjective objective in currentValues.objectives) 
        {
            objective.currentValueUpdate();
            if (objective.requiredForCompletion)
            {
                bools.Add(objective.isCompleted());
            }
        }
        return bools.All(x => x);
    }
    public void debugSkipToSection()
    {

    }
    public StringBuilder debugGetLevelStats()
    {
        return new StringBuilder()
            .Append("<u>Level;</u>\ninLevel = ").Append(LevelLoader.instance.inLevel.ToString())
            .Append("\nassetKey = ").Append(assetKey)
            .Append("\ninGameName = ").Append(inGameName)
            .Append("\nobjectives; ").Append(debugGetObjectives())
            .Append("\nplayerStartPos = ").Append(currentSection.playerStartPos.transform.position.ToStringBuilder());
    }
    StringBuilder debugGetObjectives()
    {
        StringBuilder a = new();
        foreach (LevelObjective objective in currentValues.objectives) { a.Append(objective.debugGetObjective()); }
        return a;
    }
    public void Unload()
    {
        Debug.Log("Unloading" + assetKey);
        foreach (LevelObjective objective in currentValues.objectives) { objective.Stop(); }
        ui.instance.uiObjectivesDestroy();
    }
    void End()
    {
        uiMessage.instance.New("end of " + assetKey);
        uiDebugConsole.instance.InternalCommandCall("menu");
    }
    public void GoalReached()
    {
        if (sections.Count == 0)
        {
            End();
        }
        else
        {
            SectionEnd();
        }
    }
    void SectionEnd()
    {
        if (sections.IndexOf(currentSection) == sections.Count - 1)
        {
            End();
        }
        else
        {
            SectionStart(sections.IndexOf(currentSection) + 1);            
        }
    }
    void SectionStart(int index)
    {
        currentSection = sections[index];
        foreach (Section section in sections) { section.parent.SetActive(false); }
        sections[index].parent.SetActive(true);
        Grapple.instance.maxDistance = currentValues.grappleMaxDistance;
        Player.instance.TeleportInstant(currentSection.playerStartPos != null ? currentSection.playerStartPos.transform.position : transform.TransformPoint(Vector3.zero));
    }
}