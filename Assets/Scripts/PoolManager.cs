using UnityEngine;
using System.Collections.Generic;


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
        return Instantiate(prefab, pos, rot);
    }


    public static void Despawn(GameObject instance)
    {
        Destroy(instance);
    }
}