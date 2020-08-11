using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public bool isRandom = true;

    public float xMin = -25;
    public float xMax = 25;
    public float yMin = 8;
    public float yMax = 25;
    public float zMin = -25;
    public float zMax = 25;


    public int firstSpawn = 3;
    public int repeatSpwan = 3;
    void Start()
    {
        InvokeRepeating("SpawnEnemies", firstSpawn, repeatSpwan);
    }


    void SpawnEnemies()
    {
        Vector3 enemyPosition;

        if(isRandom)
        {
            enemyPosition.x = Random.Range(xMin, xMax);
            enemyPosition.y = Random.Range(yMin, yMax);
            enemyPosition.z = Random.Range(zMin, zMax);
        } else
        {
            enemyPosition = transform.position;
        }

        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, transform.rotation) as GameObject;

        spawnedEnemy.transform.parent = gameObject.transform;
    }
}
