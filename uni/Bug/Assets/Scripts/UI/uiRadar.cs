using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiRadar : MonoBehaviour
{
    public uiRadarCamera Camera { get; private set; }
    private void Awake()
    {
        Camera = GetComponentInChildren<uiRadarCamera>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
