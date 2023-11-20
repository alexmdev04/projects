using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiRadar : MonoBehaviour
{
    public uiRadarCamera Camera { get; private set; }
    [SerializeField] TMPro.TextMeshProUGUI distanceToGoal;
    private void Awake()
    {
        Camera = GetComponentInChildren<uiRadarCamera>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (LevelLoader.instance.inLevel) { distanceToGoal.text = MathF.Round(Vector3.Distance(Player.instance.transform.position, LevelLoader.instance.levelCurrent.currentSection.goal.transform.position), 1).ToString() + "m"; }
    }
}