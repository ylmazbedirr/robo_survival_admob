<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class EnemyTouchDamage : MonoBehaviour
{
    private float damagePerSecond = 20f;
<<<<<<< HEAD
=======
=======


using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public sealed class EnemyTouchDamage : MonoBehaviour
{
    private float damagePerSecond = 10f;
    [SerializeField] private string playerTag = "Player";
>>>>>>> d039317d713cb09fd8ef2ef750340e470f42d285
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35

    [SerializeField]
    private string playerTag = "Player";

    private readonly Dictionary<GameObject, float> _fractions = new();

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
        var rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void OnTriggerStay(Collider other)
    {
        var root = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (!root.CompareTag(playerTag))
            return;
        var hp = root.GetComponent<PlayerHealth>();
        if (hp == null)
            return;

        float add = damagePerSecond * Time.fixedDeltaTime;
        if (!_fractions.TryGetValue(root, out float sum))
            sum = 0f;
        sum += add;
        int whole = Mathf.FloorToInt(sum);
        if (whole > 0)
        {
            hp.TakeDamage(whole);
            sum -= whole;
        }
        _fractions[root] = sum;
    }

    void OnTriggerExit(Collider other)
    {
        var root = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        _fractions.Remove(root);
    }
}
