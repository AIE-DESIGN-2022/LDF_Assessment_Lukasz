using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemySpawner : MonoBehaviour
{
    EnemySpawnerCB[] enemys;
    public float timeBetweenSpawns = 10;
    float timeSinceLastSpawn = Mathf.Infinity;

    private void Start()
    {
        enemys = GetComponentsInChildren<EnemySpawnerCB>();
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }

    public void SpawnRoomOfEnemies()
    {
        if (enemys == null) return;
        if (timeSinceLastSpawn < timeBetweenSpawns) return;

        foreach (EnemySpawnerCB enemy in enemys)
        {
            enemy.SpawnEnemies();
        }

        timeSinceLastSpawn = 0;
    }

    public void SpawnRoomOfEnemiesOverride()
    {
        timeSinceLastSpawn = Mathf.Infinity;
        SpawnRoomOfEnemies();
    }

}
