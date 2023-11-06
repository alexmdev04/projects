using System.Collections;
using System.Collections.Generic;
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
    public string levelSceneName;
    public string levelInGameName;
    [SerializeField] int levelNumber;
    [SerializeField] levelDifficultiesEnum levelDifficulty = levelDifficultiesEnum.normal;

    // o = objective
    [Header("Objective Toggles")]
    [SerializeField] bool oGrappleDistanceEnabled;
    [SerializeField] bool oGrappleUsesEnabled;
    [SerializeField] bool oTimeLimitEnabled;

    [Header("Objective Values")]
    [SerializeField] [Tooltip("In Meters")] float oGrappleDistanceValue;
    [SerializeField] [Tooltip("Whole Numbers")] int oGrappleUsesValue;
    [SerializeField] [Tooltip("In Seconds")] float oTimeLimitValue;

    [Header("Objective Completion Status")]
    [SerializeField] bool oGrappleDistanceComplete;
    [SerializeField] bool oGrappleUsesComplete;
    [SerializeField] bool oTimeLimitComplete;

    [Header("References")]
    [SerializeField] List<GameObject> levelSectionParents;
    [SerializeField] Goal goal;
    //[SerializeField] List<Checkpoint> checkpoints;

    void Start()
    {
    }
    void Update()
    {

    }
    public void debugToggleGoal()
    {
        goal.goalUnlocked = !goal.goalUnlocked;
    }
    public void debugSkipToSection()
    {

    }
    public void Announce()
    {
        Debug.Log(levelSceneName + " loaded!");
    }
}