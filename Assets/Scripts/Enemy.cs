using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    private float defaultSpeed;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject explosionPrefab;

    public System.Action OnDeath;

    private Transform player;
    private bool isDead;
    private Renderer rend;

    // Avoidance
    private float avoidCheckInterval = 0.5f;
    private float avoidTimer = 0f;
    private Vector3 avoidanceDir = Vector3.zero;

    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        rend = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        // Kaçış yönü güncelleme zamanlaması
        avoidTimer -= Time.deltaTime;
        if (avoidTimer <= 0f)
        {
            avoidanceDir = CalculateAvoidanceDirection();
            avoidTimer = avoidCheckInterval;
        }

        // Hareket
        Vector3 dir = (player.position - transform.position).normalized + avoidanceDir;
        dir.y = 0f;

        transform.position += dir.normalized * speed * Time.deltaTime;

        // Yönünü oyuncuya çevir
        var look = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 10f);
    }

    Vector3 CalculateAvoidanceDirection()
    {
        Vector3 avoidance = Vector3.zero;
        int count = 0;

        Collider[] nearby = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var col in nearby)
        {
            if (col.gameObject == this.gameObject) continue;
            if (col.TryGetComponent(out Enemy other))
            {
                Vector3 push = transform.position - other.transform.position;
                push.y = 0f;

                float distSqr = push.sqrMagnitude;
                if (distSqr < 0.01f) continue;

                avoidance += push.normalized / Mathf.Sqrt(distSqr); // Daha az güçlü kaçış
                count++;
            }
        }

        return (count > 0) ? (avoidance / count) * 0.7f : Vector3.zero;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        var gm = FindAnyObjectByType<GoldManager>();
        if (gm != null) gm.AddKill();

        if (hitSound != null)
            AudioManager.Instance?.PlayAt(hitSound, transform.position, 1f, Random.Range(0.95f, 1.05f));

        if (explosionPrefab != null)
            PoolManager.Spawn(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
        OnDeath?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over");
        }
    }

    void OnEnable()
    {
        defaultSpeed = speed;
        isDead = false;
        avoidanceDir = Vector3.zero;
        avoidTimer = 0f;
    }

    public void SetSpeed(float newSpeed) => speed = newSpeed;

    public void ResetSpeed() => speed = defaultSpeed;
}