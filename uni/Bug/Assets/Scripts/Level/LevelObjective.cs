using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
    
[CreateAssetMenu(fileName = "NewLevelObjective", menuName = "Level/Objective")]
public class LevelObjective : ScriptableObject
{
    public enum objectiveTypes
    {
        grappleUses,
        grappleDistance,
        timeLimit
    }
    [SerializeField] objectiveTypes type;
    [SerializeField] bool requiredForCompletion;
    [SerializeField] double value;
    [SerializeField] double scoreAwarded;
    public UnityEvent testEvent = new();

    void Awake()
    {
        if (type == objectiveTypes.grappleUses)
        {
            value = (int)value;
        }
        Grapple.instance.grappleFired.AddListener(delegate { uiMessage.instance.New("grapple"); } );
    }
}