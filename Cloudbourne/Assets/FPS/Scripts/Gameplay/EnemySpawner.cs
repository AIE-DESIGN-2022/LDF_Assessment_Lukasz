using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> locationsToSpawn;
    public GameObject enemyLongRange;
    public GameObject enemyMediumRange;
    public GameObject enemyShortRange;
    public int numberOfEnemiesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            int randomNumber = Random.Range(0, locationsToSpawn.Count);
            string enemyName = locationsToSpawn[randomNumber].GetComponent<EnemySpawnerType>().ReturnEnemyType();
            if (enemyName == "ShortRange")
            {
                GameObject instantiatedEnemy = Instantiate(enemyShortRange, locationsToSpawn[randomNumber].transform.position, locationsToSpawn[randomNumber].transform.rotation, locationsToSpawn[randomNumber].transform);
            }
            else if (enemyName == "MediumRange")
            {
                GameObject instantiatedEnemy = Instantiate(enemyMediumRange, locationsToSpawn[randomNumber].transform.position, locationsToSpawn[randomNumber].transform.rotation, locationsToSpawn[randomNumber].transform);
            }
            else if (enemyName == "LongRange")
            {
                GameObject instantiatedEnemy = Instantiate(enemyLongRange, locationsToSpawn[randomNumber].transform.position, locationsToSpawn[randomNumber].transform.rotation, locationsToSpawn[randomNumber].transform);
            }
            locationsToSpawn.Remove(locationsToSpawn[randomNumber]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
