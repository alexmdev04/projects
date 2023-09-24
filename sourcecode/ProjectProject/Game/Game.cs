using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    bool theEnd;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.End) && !theEnd) 
        {
            Debug.Log("the end.");
            theEnd = true;
            beginningOfTheEnd();
        }
    }
    void beginningOfTheEnd()
    {
        TextMeshProUGUI[] debugOverlayTextObjs = uiDebug.instance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI debugOverlayTextObj in debugOverlayTextObjs)
        {
            debugOverlayTextObj.color = Color.red;
        }
    }
}