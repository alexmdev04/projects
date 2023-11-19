using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uiObjective : MonoBehaviour
{
    TextMeshProUGUI uiObjectiveText;
    [SerializeField] TextMeshProUGUI uiObjectiveMarker;
    public bool objectiveCompleted;
    [SerializeField] [Tooltip("For use with unicode characters")] string
        tickText,
        crossText;
    public LevelObjective objective;
    void Awake()
    {
        uiObjectiveText = GetComponent<TextMeshProUGUI>();
        if (uiObjectiveMarker == null) { uiObjectiveMarker = transform.Find("uiObjectiveMarker").GetComponent<TextMeshProUGUI>(); }
    }
    void Start()
    {
        
    }

    void Update()
    {

    }
    public void Refresh(string text)
    {
        name = "uiObjective - " + objective.name;
        uiObjectiveText.text = text;
        uiObjectiveMarker.text = objectiveCompleted ? tickText : crossText;
        uiObjectiveMarker.color = objectiveCompleted ? Color.green : Color.red;
    }
}