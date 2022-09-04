using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destructable otherHealth = other.gameObject.GetComponent<Destructable>();

        if (otherHealth != null)
        {
            otherHealth.HitPointsCurrent = otherHealth.HitPoints;
            Destroy(gameObject);
        }
    }

    public static void Create(Vector3 position)
    {
        Instantiate(Resources.Load("Health"), position, Quaternion.identity);
    }
}
