using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private float hitPoints;
    private float hitPointsCurrent;

    public float HitPointsCurrent { get => hitPointsCurrent; set => hitPointsCurrent = value; }
    public float HitPoints { get => hitPoints; set => hitPoints = value; }

    private void Start()
    {
        HitPointsCurrent = HitPoints;
    }

    public void Hit(float damage)
    {
        HitPointsCurrent -= damage;

        if (HitPointsCurrent <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        BroadcastMessage("Destroyed");
        Destroy(gameObject);
    }
}
