using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
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
        }
    }
    bool _goalUnlocked;
    [SerializeField] GameObject movingObject;
    [SerializeField] bool movingObjectIsThisObject;
    [Tooltip("Uses Local Position")]
    [SerializeField] Vector3 
        lockedPosition,
        unlockedPosition;

    void Awake()
    {
        if (movingObjectIsThisObject) { movingObject = gameObject; }
    }
    void Update()
    {

    }
}