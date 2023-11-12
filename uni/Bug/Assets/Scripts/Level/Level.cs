using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    public enum levelDifficultiesEnum
    {
        easy,
        normal,
        hard
    }

    [Header("Level Attributes")]
    public string assetKey;
    public string inGameName;
    [SerializeField] int levelNumber;
    [SerializeField] levelDifficultiesEnum levelDifficulty = levelDifficultiesEnum.normal;
    [SerializeField] Vector3 playerStartPos;

    [Header("References")]
    [SerializeField] List<GameObject> levelSectionParents;
    [SerializeField] LevelGoal goal;
    //[SerializeField] List<LevelCheckpoint> checkpoints;
    [SerializeField] public List<LevelObjective> objectives;

    private void Awake()
    {
        goal = GetComponentInChildren<LevelGoal>();
    }
    void Start()
    {
        
    }
    void Update()
    {

    }
    void AddObjectives() 
    {
        foreach (LevelObjective objective in objectives) 
        {
            //objective.
        }
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
            .Append("\nobjectives = ").Append(debugGetObjectives())
            .Append("\nplayerStartPos = ").Append(playerStartPos.ToStringBuilder());
    }
    StringBuilder debugGetObjectives()
    {
        StringBuilder a = new();



        return a;
    }
}