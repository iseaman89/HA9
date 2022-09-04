using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    private NavMeshAgent _navMeshAgent;
    private Transform target;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float shootDelay;
    [SerializeField] private int scoreSize;
    private bool seeTarget;

    public Transform Target { get => target; set => target = value; }

    private void Awake()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        InvokeRepeating("Shoot", 0.0f, shootDelay);
    }

    private void Update()
    {
        _ = _navMeshAgent.SetDestination(Target.position);

        CheckTargetVisibility();
    }

    private void CheckTargetVisibility()
    {
        Vector3 targetDirection = target.position - Gun.transform.position;
        Ray ray = new Ray(Gun.transform.position, targetDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == target)
            {
                seeTarget = true;
                return;
            }
        }

        seeTarget = false;
    }

    private void Destroyed(ScoreLabel scoreLabel)
    {
        if(UnityEngine.Random.Range(0, 100) < 50) HealthBonus.Create(transform.position);
        scoreLabel.Score += scoreSize;
        if (explosionPrefab != null) Explosion.Create(transform.position, explosionPrefab);

    }

    private void Shoot()
    {
        if (seeTarget)
        {
            Vector3 targetDirection = target.position - Gun.transform.position;
            targetDirection.Normalize();
            ShootBullet(targetDirection);
        }

    }
}
