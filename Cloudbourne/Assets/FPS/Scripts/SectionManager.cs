using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public void DespawnEnemies()
    {
        RoomEnemySpawner[] rooms = GetComponentsInChildren<RoomEnemySpawner>();
        foreach (RoomEnemySpawner room in rooms)
        {
            room.Despawn();
        }
    }
}
