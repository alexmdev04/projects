using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiWMTutorialStart : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name + " found");
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            uiDebugConsole.instance.InternalCommandCall("level Level0");
        }
    }
}
