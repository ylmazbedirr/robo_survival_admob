using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private AudioClip shotClip; // sadece sesi taşıyacak
    [SerializeField] private AudioSource shotAudioSource; // sahnede ya da prefab içinde sabit AudioSource
    [SerializeField] private float baseVolume = 0.18f;

    private float _alive;

    void Awake()
    {
        // Eğer sahneye ait bir AudioManager varsa oradan da alabilirsin
        if (shotAudioSource == null)
            shotAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        _alive = 0f;

        if (shotAudioSource != null && shotClip != null)
        {
            float sfx = (AudioManager.Instance != null) ? AudioManager.Instance.SFXVolume : 1f;
            float finalVolume = Mathf.Clamp01(baseVolume * sfx * Random.Range(0.9f, 1.1f));
            float pitch = Random.Range(0.95f, 1.1f);

            shotAudioSource.pitch = pitch;
            shotAudioSource.PlayOneShot(shotClip, finalVolume);
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
