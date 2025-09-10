using UnityEngine;

public class PoolPreloader : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject[] enemyPrefabs; // birden fazla düşman için diziye çevirdik
    [SerializeField] private GameObject explosionPrefab;

    void Start()
    {
        // Mermileri ön yükle
        for (int i = 0; i < 50; i++)
        {
            PoolManager.Spawn(bulletPrefab, Vector3.zero, Quaternion.identity).SetActive(false);
        }

        // Tüm enemy prefablarını ön yükle
        foreach (var enemy in enemyPrefabs)
        {
            for (int i = 0; i < 25; i++)
            {
                PoolManager.Spawn(enemy, Vector3.zero, Quaternion.identity).SetActive(false);
            }
        }

        // Patlama efektini ön yükle
        for (int i = 0; i < 20; i++)
        {
            PoolManager.Spawn(explosionPrefab, Vector3.zero, Quaternion.identity).SetActive(false);
        }
    }
}
