/*


using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject explosionPrefab;

    private Transform player;
    private bool isDead;
    private AudioSource audioSource;
    private Renderer rend;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        audioSource = GetComponent<AudioSource>();
        rend = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        //
        var gm = FindAnyObjectByType<GoldManager>();
        if (gm != null)
        {
            gm.AddKill();
        }

        //

        if (hitSound != null)
        {
            GameObject tempAudio = new GameObject("TempEnemyDeathSound");
            tempAudio.transform.position = transform.position;

            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = hitSound;
            tempSource.spatialBlend = 1f;
            tempSource.rolloffMode = AudioRolloffMode.Linear;
            tempSource.minDistance = 2f;
            tempSource.maxDistance = 70f;

            // AudioManager null olabilir; güvenli al
            float sfx = (AudioManager.Instance != null) ? AudioManager.Instance.SFXVolume : 1f;
            tempSource.volume = sfx;

            tempSource.Play();
            Destroy(tempAudio, hitSound.length);
        }

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
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

*/


using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject explosionPrefab;


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



