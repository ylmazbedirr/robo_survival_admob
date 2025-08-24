using System.Collections.Generic;
using UnityEngine;


public sealed class PoolManager : MonoBehaviour
{
    private static PoolManager _inst;
    public static PoolManager Instance
    {
        get
        {
            if (_inst != null) return _inst;
            var go = new GameObject("[PoolManager]");
            _inst = go.AddComponent<PoolManager>();
            DontDestroyOnLoad(go);
            return _inst;
        }
    }

    private readonly Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();


    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (prefab == null) return null;
        var inst = Instance;
        if (!inst._pools.TryGetValue(prefab, out var q)) q = inst._pools[prefab] = new Queue<GameObject>();


        GameObject go = null;
        while (q.Count > 0 && go == null) // skip destroyed entries
        {
            go = q.Dequeue();
        }


        if (go == null)
        {
            go = Instantiate(prefab, pos, rot);
            var tag = go.GetComponent<PooledObject>();
            if (tag == null) tag = go.AddComponent<PooledObject>();
            tag.Prefab = prefab;
        }
        else
        {
            go.transform.SetPositionAndRotation(pos, rot);
            go.SetActive(true);
        }
        return go;
    }

    public static void Despawn(GameObject instance)
    {
        if (instance == null) return;
        var tag = instance.GetComponent<PooledObject>();
        if (tag == null || tag.Prefab == null)
        {
            // Fallback: if not pool-managed, just Destroy to avoid leaks.
            Object.Destroy(instance);
            return;
        }
        instance.SetActive(false);
        Instance._pools[tag.Prefab].Enqueue(instance);
    }
}