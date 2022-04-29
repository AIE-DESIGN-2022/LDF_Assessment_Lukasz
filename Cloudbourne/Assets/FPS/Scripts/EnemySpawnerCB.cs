using System.Collections;
using System.Collections.Generic;
using Unity.FPS.AI;
using UnityEngine;

public class EnemySpawnerCB : MonoBehaviour
{
    public GameObject enemyType;
    public int numberOfEnemiesToSpawn = 1;
    public PatrolPath patrolPath;

    bool haveSpawned = false;
    List<GameObject> spawnedEnemies;
    Transform locationToSpawn;

    private void Start()
    {
        spawnedEnemies = new List<GameObject>();
        locationToSpawn = transform;
        patrolPath = GetComponentInChildren<PatrolPath>();
    }

    private void Update()
    {
        if (haveSpawned)
        {
            foreach (GameObject enemy in spawnedEnemies.ToArray())
            {
                if (enemy == null)
                {
                    spawnedEnemies.Remove(enemy);
                }
            }
        }

        if (spawnedEnemies.Count == 0)
        {
            haveSpawned = false;
        }
    }

    public void SpawnEnemies()
    {
        if (haveSpawned) return;
        haveSpawned = true;
        spawnedEnemies.Clear();
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            GameObject instantiatedEnemy = Instantiate(enemyType, locationToSpawn.position, locationToSpawn.rotation);
            EnemyController enemyController = instantiatedEnemy.GetComponent<EnemyController>();
            if (enemyController != null && patrolPath != null) enemyController.PatrolPath = patrolPath;
            spawnedEnemies.Add(instantiatedEnemy);
        }
    }

}
