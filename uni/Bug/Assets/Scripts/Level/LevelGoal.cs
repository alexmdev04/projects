using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    public bool goalUnlocked
    {
        get
        {
            return _goalUnlocked;
        }
        set 
        { 
            _goalUnlocked = value;
            movingObject.transform.localPosition = value ? unlockedPosition : lockedPosition;
            goalLight.color = value ? Color.green : Color.red;
        }
    }
    bool _goalUnlocked;
    [SerializeField] GameObject movingObject;
    [SerializeField] bool movingObjectIsThisObject;
    [Tooltip("Uses Local Position")]
    [SerializeField] Vector3 
        lockedPosition,
        unlockedPosition;
    [SerializeField] Light goalLight;
    void Awake()
    {
        if (movingObjectIsThisObject) { movingObject = gameObject; }
        //goalLight = GetComponentInChildren<Light>();
    }
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            uiMessage.instance.New("Goal Reached!");
            uiDebugConsole.instance.InternalCommandCall("menu");
        }
    }
}