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
    [System.Serializable] public struct difficultySpecificValues
    {
        public float grappleMaxDistance;
        public List<LevelObjective> objectives;
    }
    public difficultySpecificValues easyValues;
    public difficultySpecificValues normalValues;
    public difficultySpecificValues hardValues;
    public difficultySpecificValues currentValues { get; private set; }
    [Header("Level Attributes")]
    public string assetKey;
    public string inGameName;
    public int levelNumber;
    public levelDifficultiesEnum levelDifficulty = levelDifficultiesEnum.normal;
    public float grappleMaxDistance = 30f;
    [SerializeField] [Tooltip("The player will be teleported here when the level is started, in local space by default")] Vector3 playerStartPos;
    [SerializeField] bool playerStartPosWorldSpace;
    [Header("References")]
    [SerializeField] List<GameObject> levelSectionParents;
    public LevelGoal goal;

    void Awake()
    {
        goal = GetComponentInChildren<LevelGoal>();
    }
    void Start()
    {
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
        foreach (LevelObjective obj in currentValues.objectives)
        {
            obj.Start();
        }
        Grapple.instance.SetMovementActive(true);
        Grapple.instance.maxDistance = grappleMaxDistance;
        Player.instance.TeleportInstant(playerStartPosWorldSpace ? playerStartPos : transform.TransformPoint(playerStartPos));
    }
    void Update()
    {
        goal.goalUnlocked = validateObjectives();
    }
    bool validateObjectives()
    {
        List<bool> bools = new List<bool>();
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
    public void debugToggleGoal()
    {
        goal.goalUnlocked = !goal.goalUnlocked;
    }
    public void debugSkipToSection()
    {

    }
    public StringBuilder debugGetLevelStats()
    {
        return new StringBuilder()
            .Append("<u>Level;</u>")
            .Append("\nassetKey = ").Append(assetKey)
            .Append("\ninGameName = ").Append(inGameName)
            .Append("\nobjectives; ").Append(debugGetObjectives())
            .Append("\nplayerStartPos = ").Append(playerStartPos.ToStringBuilder());
    }
    StringBuilder debugGetObjectives()
    {
        StringBuilder a = new();
        foreach (LevelObjective objective in currentValues.objectives) { a.Append(objective.debugGetObjective()); }
        return a;
    }
    public void Unload()
    {
        Debug.Log("stopping objectives in " + assetKey);
        foreach (LevelObjective objective in currentValues.objectives) { objective.Stop(); }
        foreach (uiObjective uiObjective in ui.instance.uiObjectives)
        {
            Destroy(uiObjective);
        }
    }
}