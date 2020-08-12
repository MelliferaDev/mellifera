using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    public bool isRandom = true;

    [Header("Location Settings")]
    public float xMin = -25;
    public float xMax = 25;
    public float yMin = 8;
    public float yMax = 25;
    public float zMin = -25;
    public float zMax = 25;
    [Header("Spawn Settings")]
    public int firstSpawn = 3;
    public int repeatSpwan = 3;
    public int capacity = 10; // limit the amount of wasps at a time
    void Start()
    {
        InvokeRepeating("SpawnEnemies", firstSpawn, repeatSpwan);
    }


    void SpawnEnemies()
    {
        if (gameObject.transform.childCount >= capacity) return;
        
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

    private void OnDrawGizmosSelected()
    {
        if (!isRandom) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(new Vector3(xMax , yMax, zMax), new Vector3(xMax, yMin, zMax));
        Gizmos.DrawLine(new Vector3(xMin , yMax, zMax), new Vector3(xMin, yMin, zMax));
        Gizmos.DrawLine(new Vector3(xMin , yMax, zMin), new Vector3(xMin, yMin, zMin));
        Gizmos.DrawLine(new Vector3(xMax , yMax, zMin), new Vector3(xMax, yMin, zMin));

        Gizmos.DrawLine(new Vector3(xMax , yMax, zMax), new Vector3(xMin , yMax, zMax));
        Gizmos.DrawLine(new Vector3(xMin , yMax, zMax), new Vector3(xMin , yMax, zMin));
        Gizmos.DrawLine(new Vector3(xMin , yMax, zMin), new Vector3(xMax , yMax, zMin));
        Gizmos.DrawLine(new Vector3(xMax , yMax, zMin), new Vector3(xMax , yMax, zMax));
        
        Gizmos.DrawLine(new Vector3(xMax , yMin, zMax), new Vector3(xMin , yMin, zMax));
        Gizmos.DrawLine(new Vector3(xMin , yMin, zMax), new Vector3(xMin , yMin, zMin));
        Gizmos.DrawLine(new Vector3(xMin , yMin, zMin), new Vector3(xMax , yMin, zMin));
        Gizmos.DrawLine(new Vector3(xMax , yMin, zMin), new Vector3(xMax , yMin, zMax));
    }
}
