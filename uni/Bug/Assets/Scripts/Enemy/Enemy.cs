using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public static Enemy instance { get; private set; }
    NavMeshAgent agent;
    Animator animator;
    bool
        wait,
        idle = true;
    public float
        turnSpeed = 100f,
        moveSpeed = 3f,
        distanceToStop = 3f,
        decelerationSpeed = 1f,
        moveSpeedLowClamp = 1f;
    public bool useAsync = true;
    public enum movementStateEnum
    {
        idle,
        walking,
        turning,
    }
    public movementStateEnum movementState;
    public bool loopPatrol = true;
    public int patrolInstructionCurrent;
    public List<patrolInstruction> patrolInstructions;
    public bool patrolRunning;

    public enum patrolAction
    {
        turnLeft90,
        turnLeft180,
        turnRight90,
        turnRight180,
        walkTo
    }
    [Serializable]
    public struct patrolInstruction
    {
        public patrolAction action;
        public Vector3 walkTo;
        public float waitAtEnd;
    }
    void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {  }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {  }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {  }
        if (Input.GetKeyDown(KeyCode.Space) && !patrolRunning) { StartCoroutine(Patrol()); };
        MovementStateUpdate();
    }

    void MovementStateUpdate()
    {
        switch (movementState)
        {
            case movementStateEnum.idle:
                break;
            case movementStateEnum.walking:
                break;
            case movementStateEnum.turning:
                break;
            default:
                break;
        }
    }
     
    IEnumerator Patrol()
    {
        patrolRunning = true;
        foreach (patrolInstruction instruction in patrolInstructions)
        {
            PatrolAction(instruction);
            while (wait) { yield return new WaitForEndOfFrame(); }
        }
        if (loopPatrol) { RestartPatrol(); yield break; }
        patrolRunning = false;
        Debug.Log("Patrol Loop Stopped");
        yield break;
    }
    void PatrolAction(patrolInstruction instruction)
    {
        Debug.Log((useAsync ? "async " : "") + instruction.action.ToString());
        switch (instruction.action)
        {
            case patrolAction.turnLeft90:
                TurnLeft90(); break;
            case patrolAction.turnLeft180:
                TurnLeft180(); break;
            case patrolAction.turnRight90:
                TurnRight90(); break;
            case patrolAction.turnRight180:
                TurnRight180(); break;
            case patrolAction.walkTo:
                GoToPosition(instruction.walkTo); break;
        }
    }
    void RestartPatrol()
    {
        StartCoroutine(Patrol());
    }
    public void TurnLeft90()
    {
        //Turn(true, false);
    }
    public void TurnLeft180()
    {
        //Turn(true, false);
    }
    public void TurnRight90()
    {
        //Turn(false, false);
    }
    public void TurnRight180()
    {
        //Turn(false, true);
    }

    public void GoToPosition(Vector3 targetPosition)
    {
        agent.destination = targetPosition;
    }
}