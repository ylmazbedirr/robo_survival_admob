/*


using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    // Ateşleme Sistemi
    [Header("Fire Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootInterval = 0.1f;
    private float shootTimer;

    void FixedUpdate()
    {
        // Ateşleme Sistemi
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    // Ateşleme Sistemi
    private void Shoot()
    {

        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

    }
}

*/

using UnityEngine;


public class AutoShooter : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField, Min(0.01f)] private float shootInterval = 0.1f;
    private float _t;


    void Update()
    {
        _t += Time.deltaTime;
        if (_t >= shootInterval)
        {
            _t -= shootInterval;
            if (bulletPrefab != null && shootPoint != null)
            {
                PoolManager.Spawn(bulletPrefab, shootPoint.position, shootPoint.rotation);
            }
        }
    }
}