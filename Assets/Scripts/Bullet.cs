/*

using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    [SerializeField] private float lifeTime = 2f;

    void Start()
    {


        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.clip != null)
        {
            float baseVolume = 0.18f;
            float sfxVolume = AudioManager.Instance != null ? AudioManager.Instance.SFXVolume : 1f;

            audio.volume = baseVolume * sfxVolume * Random.Range(0.9f, 1.1f);
            audio.pitch = Random.Range(0.95f, 1.1f);
            audio.Play();
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out Enemy enemy))
        {
            // enemy null ise çağırma
            if (enemy != null) enemy.Die();
        }

        Destroy(gameObject, 0.1f);
    }
}

*/


using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private AudioSource shotAudio; // assign on prefab for 0 cost GetComponent
    [SerializeField] private float baseVolume = 0.18f;


    private float _alive;


    void Awake()
    {
        if (shotAudio == null) shotAudio = GetComponent<AudioSource>();
    }


    void OnEnable()
    {
        _alive = 0f;
        if (shotAudio != null && shotAudio.clip != null)
        {
            float sfx = (AudioManager.Instance != null) ? AudioManager.Instance.SFXVolume : 1f;
            shotAudio.volume = Mathf.Clamp01(baseVolume * sfx * Random.Range(0.9f, 1.1f));
            shotAudio.pitch = Random.Range(0.95f, 1.1f);
            shotAudio.Play();
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        _alive += Time.deltaTime;
        if (_alive >= lifeTime)
            PoolManager.Despawn(gameObject);
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (other.TryGetComponent(out Enemy enemy) && enemy != null)
            enemy.Die();
        PoolManager.Despawn(gameObject);
    }
}