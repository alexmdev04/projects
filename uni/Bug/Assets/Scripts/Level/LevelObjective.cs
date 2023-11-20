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
    [SerializeField] [Tooltip("The player must be at this value to complete the objective")] double completionValue;
    public double currentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            _currentValue = value;
            if (LevelLoader.instance.inLevel)
            {
                uiObjective.Refresh(uiGetText());
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
                    Grapple.instance.grappleFired.AddListener(delegate { currentValueEdit(1); });
                    break;
                }
            case objectiveTypes.grappleDistance:
                {
                    Grapple.instance.grappleFinished.AddListener(delegate { currentValueEdit(Grapple.instance.distanceTravelled); });
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
        Grapple.instance.grappleFired.RemoveListener(delegate { currentValueEdit(1); });
        Grapple.instance.grappleFinished.RemoveListener(delegate { currentValueEdit(Grapple.instance.distanceTravelled); });
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


        uiObjective.objectiveCompleted = value;
        return value;
    }
    public string uiGetText()
    {
        string text = name;
        string customReturn = string.Empty;
        string unit = string.Empty;
        switch (type)
        {
            case objectiveTypes.grappleUses:
                {
                    text = "Grapple Uses = ";
                    break;
                }
            case objectiveTypes.grappleDistance:
                {
                    text = "Grapple Distance = ";
                    unit = "m";
                    break;
                }
            case objectiveTypes.timeLimit:
                {
                    text = "Time = ";
                    customReturn = (completionValue - currentValue).ConvertTime() + " left";
                    break;
                }
        }
        return text += (customReturn != string.Empty) ? customReturn : 
            (uiValueCountDown ? completionValue - currentValue + unit + " left" : currentValue + unit + " / " + completionValue + unit);
    }
    public StringBuilder debugGetObjective()
    {
        return new StringBuilder("\n").Append(name).Append(" - ").Append(type).Append(" = ").Append(currentValue).Append(" / ").Append(completionValue);
    }
    //private float FloatFromAxis(bool positive, bool negative) =>
    //    (positive, negative) switch
    //    {
    //        (true, false) => 1f,
    //        (false, true) => -1f,
    //        _ => 0f
    //    };

    //private Vector2 GetInput()
    //{
    //    float horizontal = FloatFromAxis(Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.D));
    //    float vertical = FloatFromAxis(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S));
    //    return new Vector2(horizontal, vertical);
    //}
} 