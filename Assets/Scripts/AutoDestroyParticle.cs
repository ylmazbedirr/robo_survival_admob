/*

using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    [SerializeField] private float maxLifetime = 2f;

    void Start()
    {
        Destroy(gameObject, maxLifetime);
    }
}

*/

using UnityEngine;


public class AutoDespawnParticle : MonoBehaviour
{
    [SerializeField] private float maxLifetime = 2f; // Fallback if no ParticleSystem
    private float _t;
    private ParticleSystem _ps;


    void Awake() { _ps = GetComponent<ParticleSystem>(); }
    void OnEnable() { _t = 0f; }


    void Update()
    {
        _t += Time.deltaTime;
        float limit = maxLifetime;
        if (_ps != null) { var m = _ps.main; limit = Mathf.Max(limit, m.duration + m.startLifetime.constantMax); }
        if (_t >= limit)
        {
            PoolManager.Despawn(gameObject);
        }
    }
}