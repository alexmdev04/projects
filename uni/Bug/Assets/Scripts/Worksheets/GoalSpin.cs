using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 50f;
    void Update()
    {
        transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime);
    }
}
