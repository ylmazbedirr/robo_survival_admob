using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject explosionPrefab;

    //
    public System.Action OnDeath;


    private Transform player;
    private bool isDead;
    private Renderer rend;


    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        rend = GetComponentInChildren<Renderer>();
    }


    void Update()
    {
        if (isDead || player == null) return;
        Vector3 dir = player.position - transform.position; dir.y = 0f;
        float len2 = dir.sqrMagnitude; if (len2 < 0.000001f) return;
        dir *= 1f / Mathf.Sqrt(len2);
        transform.position += dir * speed * Time.deltaTime;
        var look = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 10f);
    }

    public void Die()
    {
        if (isDead) return; isDead = true;


        var gm = FindAnyObjectByType<GoldManager>();
        if (gm != null) gm.AddKill();


        if (hitSound != null)
        {
            AudioManager.Instance?.PlayAt(hitSound, transform.position, 1f, Random.Range(0.95f, 1.05f));
        }


        if (explosionPrefab != null)
        {
            PoolManager.Spawn(explosionPrefab, transform.position, Quaternion.identity);
        }


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
}