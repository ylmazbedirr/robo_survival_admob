using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Enemy Settings")]
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private float spawnInterval = 0.5f;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private int maxEnemyCount = 80;

    private float timer;
    private List<Enemy> activeEnemies = new();

    [Header("Difficulty Scaling")]
    private float difficultyTimer = 0f;
    private bool isSpeedBoostActive = false;
    private float speedBoostDuration = 5f;
    private float speedBoostCooldown = 20f;

    [Header("Health Orb Settings")]
    [SerializeField]
    private GameObject healthOrbPrefab;

    [SerializeField]
    private float healthOrbInterval = 43f;

    [SerializeField]
    private Vector2 xRange = new Vector2(-19.4f, 22.6f);

    [SerializeField]
    private Vector2 zRange = new Vector2(-25f, 16.5f);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnHealthOrbRoutine());
    }

    void Update()
    {
        timer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (GetActiveEnemyCount() < maxEnemyCount)
            {
                SpawnEnemy();
            }
        }

        if (!isSpeedBoostActive && difficultyTimer >= speedBoostCooldown)
        {
            difficultyTimer = 0f;
            StartCoroutine(TempSpeedBoost());
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || player == null)
            return;

        int index = Random.Range(0, enemyPrefabs.Length);
        var prefab = enemyPrefabs[index];
        Vector3 pos = GetRandomSpawnPosition();

        var go = PoolManager.Spawn(prefab, pos, Quaternion.identity);

        if (go.TryGetComponent(out Enemy enemy))
        {
            activeEnemies.Add(enemy);
            enemy.OnDeath = () =>
            {
                activeEnemies.Remove(enemy);
            };
        }
    }

    private int GetActiveEnemyCount()
    {
        int count = 0;
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
                count++;
        }
        return count;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float d = 30f;
        Vector3 spawnPos = player.position;
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0:
                spawnPos += Vector3.left * d;
                break;
            case 1:
                spawnPos += Vector3.right * d;
                break;
            case 2:
                spawnPos += Vector3.forward * d;
                break;
            default:
                spawnPos += Vector3.back * d;
                break;
        }
        return spawnPos;
    }

    private IEnumerator TempSpeedBoost()
    {
        isSpeedBoostActive = true;

        CameraShake.Instance?.Shake(1f);

        foreach (var enemy in activeEnemies)
            enemy?.SetSpeed(9f);

        yield return new WaitForSeconds(speedBoostDuration);

        foreach (var enemy in activeEnemies)
            enemy?.ResetSpeed();

        isSpeedBoostActive = false;
    }

    private IEnumerator SpawnHealthOrbRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(healthOrbInterval);

            Vector3 spawnPos = new Vector3(
                Random.Range(xRange.x, xRange.y),
                0.5f,
                Random.Range(zRange.x, zRange.y)
            );

            Instantiate(healthOrbPrefab, spawnPos, Quaternion.identity);
        }
    }

    public List<Enemy> GetActiveEnemies()
    {
        return activeEnemies;
    }
}
