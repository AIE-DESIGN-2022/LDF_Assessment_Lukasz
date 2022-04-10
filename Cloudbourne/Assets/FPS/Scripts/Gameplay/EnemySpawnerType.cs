using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerType : MonoBehaviour
{
    public enum EnemyRange // your custom enumeration
    {
        ShortRange,
        MediumRange,
        LongRange
    };
    public EnemyRange thisEnemy;

    public string ReturnEnemyType() 
    {
        return thisEnemy.ToString();
    }
}
