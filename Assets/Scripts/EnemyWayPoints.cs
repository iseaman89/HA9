using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWayPoints : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    private Transform target;
    public Transform[] wayPoints;
    private int index = 0;

    private void Start()
    {
        MoveToWayPoint();
    }

    private void MoveToWayPoint()
    {
        target = wayPoints[index];
    }

    private void FixedUpdate()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if ((transform.position - playerTransform.position).magnitude < 20)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }
        else
        {
            navMeshAgent.SetDestination(target.position);

            if ((transform.position - target.position).magnitude < 3f)
            {
                index++;
                if (index >= wayPoints.Length)
                {
                    index = 0;
                    MoveToWayPoint();
                }
                else
                {
                    MoveToWayPoint();
                }
            }
        }
    }
}
