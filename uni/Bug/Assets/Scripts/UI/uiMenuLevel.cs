using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiMenuLevel : MonoBehaviour
{
    public static uiMenuLevel instance { get; private set; }
    RaycastHit hit;
    [SerializeField] LayerMask layerMask;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //StartCoroutine(Tutorial());
    }
    IEnumerator Tutorial()
    {
        float mouseRotationThreshhold = 1000f;
        do
        {
            float meanMouseRotation = Player.instance.mouseRotation.x + Player.instance.mouseRotation.y / 2;
            mouseRotationThreshhold -= meanMouseRotation;
        }
        while (mouseRotationThreshhold > 0);
        yield return new WaitForSeconds(1f);
        uiMessage.instance.New("Use your mouse to look around");
    }
    public void ForceRestartTutorial()
    {
        StartCoroutine(Tutorial());
    }
    void Update()
    {
        
    }
}