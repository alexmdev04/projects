using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float detectionDistance = 5f;
    void Update()
    {
        //float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Vector3.Distance(transform.position, player.position) <= detectionDistance)
        {
            Debug.Log("Player is close enough");
        }
    }
}
