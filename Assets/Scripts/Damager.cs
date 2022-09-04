using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private float damage;
    private float radius;
    private GameObject owner;
    [SerializeField] private int multiple = 1000;
    [SerializeField] private GameObject explosionPrefab;

    public float Damage { get => damage; set => damage = value; }
    public GameObject Owner { get => owner; set => owner = value; }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameObject.Equals(collision.gameObject, Owner))
        {
            if (radius > 0)
            {
                CauseExplosionDamage();
            }
            else
            {
                Destructable target = collision.gameObject.GetComponent<Destructable>();
                if (target != null)
                {
                    target.Hit(Damage);
                }

            }

            if(explosionPrefab != null) Explosion.Create(transform.position, explosionPrefab);
            ParticleSystem trail = gameObject.GetComponentInChildren<ParticleSystem>();
            if (trail != null)
            {
                Destroy(trail.gameObject, trail.startLifetime);

                trail.Stop();

                trail.transform.SetParent(null);
            }

            Destroy(gameObject);
        }
    }

    private void CauseExplosionDamage()
    {
        Collider[] explosionVictims = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < explosionVictims.Length; i++)
        {
            Vector3 vectorToVictim = explosionVictims[i].transform.position - transform.position;
            float decay = 1 - (vectorToVictim.magnitude / radius);
            Destructable currentVictim = explosionVictims[i].gameObject.GetComponent<Destructable>();
            if (currentVictim != null)
            {
                currentVictim.Hit(damage * decay);
            }
            Rigidbody victimRigidbody = explosionVictims[i].gameObject.GetComponent<Rigidbody>();
            if (victimRigidbody != null)
            {
                victimRigidbody.AddForce(vectorToVictim.normalized * decay * multiple);
            }
        }
    }
}
