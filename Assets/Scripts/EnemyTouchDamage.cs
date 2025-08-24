/*

// Assets/Scripts/Combat/EnemyTouchDamage.cs
// Neden: Küsurat her framede 1'e yuvarlanmasın; DPS doğru işlesin ve fizik döngüsüyle uyumlu olsun.
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class EnemyTouchDamage : MonoBehaviour
{
    private float damagePerSecond = 7f;
    [SerializeField] private string playerTag = "Player";

    // Neden: Her hedef için küsuratı biriktirmek.
    private readonly Dictionary<GameObject, float> _fractions = new();

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;                         // neden: temas halinde sürekli hasar
        var rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; rb.useGravity = false; // neden: trigger güvenilirliği
    }

    void OnTriggerStay(Collider other)
    {
        var root = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (!root.CompareTag(playerTag)) return;

        var hp = root.GetComponent<PlayerHealth>();
        if (hp == null) return;

        // Bu frame'de eklenecek küsurat
        float add = damagePerSecond * Time.fixedDeltaTime;

        if (!_fractions.TryGetValue(root, out float sum)) sum = 0f;
        sum += add;

        // Tam sayıya ulaşınca uygula, küsuratı sakla
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
        _fractions.Remove(root); // neden: Temas bitince birikimi sıfırla
    }
}

*/

using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public sealed class EnemyTouchDamage : MonoBehaviour
{
    private float damagePerSecond = 7f;
    [SerializeField] private string playerTag = "Player";


    private readonly Dictionary<GameObject, float> _fractions = new();


    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
        var rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; rb.useGravity = false;
    }


    void OnTriggerStay(Collider other)
    {
        var root = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (!root.CompareTag(playerTag)) return;
        var hp = root.GetComponent<PlayerHealth>();
        if (hp == null) return;


        float add = damagePerSecond * Time.fixedDeltaTime;
        if (!_fractions.TryGetValue(root, out float sum)) sum = 0f;
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