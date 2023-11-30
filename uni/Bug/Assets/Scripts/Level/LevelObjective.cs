using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewLevelObjective", menuName = "Level/Objective")]
public class LevelObjective : ScriptableObject
{
    public enum objectiveTypes
    {
        grappleUses,
        grappleDistance,
        timeLimit,
        alertLevel
    }
    public enum objectiveCompletionTypes
    {
        lessThanCompletionValue,
        lessThanEqualToCompletionValue,
        greaterThanCompletetionValue,
        greaterThanEqualToCompletetionValue
    }
    [SerializeField] objectiveTypes type;
    [SerializeField] objectiveCompletionTypes completionType;
    public bool requiredForCompletion;
    [SerializeField] bool uiValueCountDown;
    [SerializeField][Tooltip("The player must be at this value to complete the objective")] double completionValue;
    public bool completed { get; private set; }
    public double currentValue
    {
        get
        {
            return _currentValue;
        }
        private set
        {
            _currentValue = value;
            completed = isCompleted();
            if (LevelLoader.instance.inLevel)
            {
                uiObjectiveRefresh();
            }
        }
    }
    double _currentValue = 0d;
    [SerializeField] [Tooltip("When the level ends this score will be awarded if the objective is complete")] double scoreAwarded;
    [HideInInspector] public uiObjective uiObjective;
    public bool successfulStart = false;

    public void Start()
    {
        currentValue = 0;
        switch (type)
        {
            case objectiveTypes.grappleUses:
                {
                    completionValue = (int)completionValue;
                    Grapple.instance.finished.AddListener(currentValueEditAuto);
                    break;
                }
            case objectiveTypes.grappleDistance:
                {
                    Grapple.instance.finished.AddListener(currentValueEditAuto);
                    break;
                }
            case objectiveTypes.timeLimit:
                {
                    break;
                }
            case objectiveTypes.alertLevel:
                {
                    break;
                }
        }
    }
    public void Stop()
    {
        Grapple.instance.fired.RemoveListener(currentValueEditAuto);
        Grapple.instance.finished.RemoveListener(currentValueEditAuto);
    }
    void currentValueSet(double value)
    {
        currentValue = value;
    }
    void currentValueEdit(double value)
    {

        if (currentValue >= completionValue)
        {
            Debug.Log(name + " completion value reached (" + completionValue + ")");
            currentValue = completionValue;
            return;
        }
        currentValue += value;
    }
    void currentValueEditAuto() // remove listener does not work when taking an argument
    {
        switch (type)
        {
            case objectiveTypes.grappleUses:
                {
                    currentValueEdit(1);
                    break;
                }
            case objectiveTypes.grappleDistance:
                {
                    currentValueEdit(Grapple.instance.distanceTravelled);
                    break;
                }
            case objectiveTypes.timeLimit:
                {
                    break;
                }
            case objectiveTypes.alertLevel:
                {
                    break;
                }

        }
    }
    public void currentValueUpdate()
    {
        if (type == objectiveTypes.timeLimit && currentValue < completionValue)
        {
            currentValue += Time.deltaTime;
        }
    }
    public bool isCompleted()
    {
        bool value;
        switch (completionType)
        {
            case objectiveCompletionTypes.lessThanCompletionValue:
                {
                    value = currentValue < completionValue;
                    break;
                }
            case objectiveCompletionTypes.lessThanEqualToCompletionValue:
                {
                    value = currentValue <= completionValue;
                    break;
                }
            case objectiveCompletionTypes.greaterThanCompletetionValue:
                {
                    value = currentValue > completionValue;
                    break;
                }
            case objectiveCompletionTypes.greaterThanEqualToCompletetionValue:
                {
                    value = currentValue >= completionValue;
                    break;
                }
            default:
                {
                    value = false;
                    break;
                }
        }
        return value;
    }
    public void uiObjectiveRefresh()
    {
        uiObjective.Refresh(type, currentValue, completionValue, uiValueCountDown);
    }
    public StringBuilder debugGetObjective()
    {
        return new StringBuilder(uiDebug.str_NewLine).Append(name).Append(uiDebug.str_dash).Append(type).Append(uiDebug.str_equals).Append(currentValue).Append(uiDebug.str_divide).Append(completionValue);
    }

} 