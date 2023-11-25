using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform minSpawnpoint;
    [SerializeField] private Transform maxSpawnpoint;
    private float spawnerCooldown;

    private void Awake()
    {
        spawnerCooldown = PlayerPrefs.GetFloat("SpawnRate"); 
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnerCooldown);

        Vector3 spawnAtLocation = new Vector3(Random.Range(minSpawnpoint.position.x, maxSpawnpoint.position.x), Random.Range(minSpawnpoint.position.y, maxSpawnpoint.position.y), 0);

        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnAtLocation, transform.rotation);

        StartCoroutine(SpawnEnemies());
    }
}
