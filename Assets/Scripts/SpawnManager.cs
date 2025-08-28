/*

using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 0.7f;
    [SerializeField] private Transform player;
    private float timer;


    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || player == null) return;
        int index = Random.Range(0, enemyPrefabs.Length);
        var selectedEnemy = enemyPrefabs[index]; if (selectedEnemy == null) return;
        Vector3 spawnPos = GetRandomSpawnPosition();


        if (selectedEnemy.name.Contains("Enemy1")) spawnPos.y = 0.334f;


        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
    }


    Vector3 GetRandomSpawnPosition()
    {
        float distanceFromPlayer = 30f;
        int side = Random.Range(0, 4);
        Vector3 spawnPos = player.position;
        switch (side)
        {
            case 0: spawnPos += Vector3.left * distanceFromPlayer; break;
            case 1: spawnPos += Vector3.right * distanceFromPlayer; break;
            case 2: spawnPos += Vector3.forward * distanceFromPlayer; break;
            default: spawnPos += Vector3.back * distanceFromPlayer; break;
        }
        return spawnPos;
    }
}

*/


using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 0.4f;
    [SerializeField] private Transform player;
    [SerializeField] private int maxEnemyCount = 90;

    private float timer;
    private List<Enemy> activeEnemies = new();

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            // SAHNEDEKİ AKTİF DÜŞMAN SAYISI < MAX OLDUĞUNDA SPAWN ET
            if (GetActiveEnemyCount() < maxEnemyCount)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        var prefab = enemyPrefabs[index];
        Vector3 pos = GetRandomSpawnPosition();

        var go = PoolManager.Spawn(prefab, pos, Quaternion.identity);

        if (go.TryGetComponent(out Enemy enemy))
        {
            // Eğer zaten listede yoksa ekle
            if (!activeEnemies.Contains(enemy))
            {
                activeEnemies.Add(enemy);
                enemy.OnDeath = () => { activeEnemies.Remove(enemy); };
            }
        }
    }

    int GetActiveEnemyCount()
    {
        int count = 0;
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
                count++;
        }
        return count;
    }

    Vector3 GetRandomSpawnPosition()
    {
        float distance = 30f;
        Vector3 spawnPos = player.position;
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: spawnPos += Vector3.left * distance; break;
            case 1: spawnPos += Vector3.right * distance; break;
            case 2: spawnPos += Vector3.forward * distance; break;
            default: spawnPos += Vector3.back * distance; break;
        }

        return spawnPos;
    }
}
