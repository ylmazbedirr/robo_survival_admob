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
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[index];

        if (selectedEnemy == null) return;

        Vector3 spawnPos = GetRandomSpawnPosition();

        // 🎯 Enemy'ye göre sabit yüksekliği ayarla
        if (selectedEnemy.name.Contains("Enemy11"))
            spawnPos.y = 0.334f;
        else if (selectedEnemy.name.Contains("Enemy12"))
            spawnPos.y = 0.334f;
        else if (selectedEnemy.name.Contains("Enemy13"))
            spawnPos.y = 0.334f;
        else if (selectedEnemy.name.Contains("Enemy14"))
            spawnPos.y = 0.334f;


        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);

    }

    Vector3 GetRandomSpawnPosition()
    {
        float distanceFromPlayer = 30f;
        int side = Random.Range(0, 4); // 0=left, 1=right, 2=front, 3=back
        Vector3 spawnPos = player.position;

        switch (side)
        {
            case 0: spawnPos += Vector3.left * distanceFromPlayer; break;
            case 1: spawnPos += Vector3.right * distanceFromPlayer; break;
            case 2: spawnPos += Vector3.forward * distanceFromPlayer; break;
            case 3: spawnPos += Vector3.back * distanceFromPlayer; break;
        }

        return spawnPos;
    }
}

*/

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


        // NOTE: Height should be in prefab; this is a quick safeguard.
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
