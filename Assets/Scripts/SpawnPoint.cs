using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float spawnDelay;
    [SerializeField] private List<Enemy> enemyPrefab = new List<Enemy>();

    public void Spawn()
    {
        Enemy newEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Count)], transform.position, Quaternion.identity) as Enemy;
        newEnemy.Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        InvokeRepeating("Spawn", 0.0f, spawnDelay);
    }
}
