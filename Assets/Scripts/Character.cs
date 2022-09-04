using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float shootPower;
    [SerializeField] private float bulletDamage = 10;
    [SerializeField] private float rocketDamage = 20;
    [SerializeField] private float rocketDelay;
    private float rocketDelayCurrent;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform gun;

    public Transform Gun { get => gun; set => gun = value; }

    public void ShootRocket(Vector3 direction)
    {
        if (rocketDelayCurrent <= 0)
        {
            rocketDelayCurrent = rocketDelay;
            GameObject newRocket = Instantiate(rocketPrefab, Gun.position, Gun.rotation) as GameObject;
            newRocket.GetComponent<Rigidbody>().AddForce(direction * shootPower);
            Damager bulletBehaviour = newRocket.GetComponent<Damager>();
            bulletBehaviour.Damage = rocketDamage;
            bulletBehaviour.Owner = gameObject;
            Destroy(newRocket, 5);
        }
    }
        

    public void ShootBullet(Vector3 direction)
    {
        GameObject newBullet = Instantiate(bulletPrefab, Gun.position, Gun.rotation) as GameObject;
        newBullet.GetComponent<Rigidbody>().AddForce(direction * shootPower);
        Damager bulletBehaviour = newBullet.GetComponent<Damager>();
        bulletBehaviour.Damage = bulletDamage;
        bulletBehaviour.Owner = gameObject;
        Destroy(newBullet, 5);
    }

    protected void UpdateTimer()
    {
        if (rocketDelayCurrent > 0)
        {
            rocketDelayCurrent -= Time.deltaTime;        }
    }
}
