using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMovement : MonoBehaviour
{
    [SerializeField] float 
        moveSpeed = 1f,
        amplitude = 1f;
    Vector3 
        startPos;

	void Start()
	{
		startPos = transform.position;
	}
	void Update()
	{
		//float verticalMovement = Mathf.Sin(Time.time * moveSpeed) * amplitude;
		//Vector3 newPosition = startPos + Vector3.up * verticalMovement;
		//transform.position = newPosition;

		transform.position = (startPos + Vector3.up) * (Mathf.Sin(Time.time * moveSpeed) * amplitude);
	}
}
