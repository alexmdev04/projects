using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiWMStartBox : MonoBehaviour
{
    [SerializeField] string OnTriggerEnterCommand = "level Level0";
    void OnTriggerEnter(Collider collision) { if (collision.gameObject.GetComponent<Player>() != null) { uiDebugConsole.instance.InternalCommandCall(OnTriggerEnterCommand); } }
}
