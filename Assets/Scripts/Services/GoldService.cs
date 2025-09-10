/*
// Assets/Scripts/Services/GoldService.cs
using System;
using UnityEngine;

/// Kalıcı toplam altın servisi (DontDestroyOnLoad).
public sealed class GoldService : MonoBehaviour
{
    private static GoldService _inst;

    public static GoldService Instance
    {
        get
        {
            if (_inst) return _inst;

            // 1) Sahne içinde var mı?
#if UNITY_2023_1_OR_NEWER
            _inst = FindFirstObjectByType<GoldService>(FindObjectsInactive.Exclude);
#else
            _inst = FindObjectOfType<GoldService>();
#endif
            if (_inst) return _inst;

            // 2) Yoksa oluştur
            var go = new GameObject("[GoldService]");
            _inst = go.AddComponent<GoldService>();
            DontDestroyOnLoad(go);
            _inst.Load();
            return _inst;
        }
    }

    public event Action<int> OnGoldChanged;

    private const string KEY_TOTAL = "GOLD_TOTAL";
    public int TotalGold { get; private set; }

    // Neden: İlk sahne yüklenmeden önce servis hazır olsun (null yarışını önler).
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Prewarm() => _ = Instance;

    private void Awake()
    {
        if (_inst && _inst != this) { Destroy(gameObject); return; }
        _inst = this;
        DontDestroyOnLoad(gameObject);
        if (TotalGold == 0) Load();
    }

    public void AddGold(int amount)
    {
        if (amount == 0) return;
        TotalGold = Mathf.Max(0, TotalGold + amount);
        OnGoldChanged?.Invoke(TotalGold);
        Save();
    }

    public bool SpendGold(int amount)
    {
        if (amount <= 0 || TotalGold < amount) return false;
        TotalGold -= amount;
        OnGoldChanged?.Invoke(TotalGold);
        Save();
        return true;
    }

    public void Load()
    {
        TotalGold = PlayerPrefs.GetInt(KEY_TOTAL, 0);
        OnGoldChanged?.Invoke(TotalGold);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(KEY_TOTAL, TotalGold);
        PlayerPrefs.Save();
    }
}

*/

using System;
using UnityEngine;


public sealed class GoldService : MonoBehaviour
{
    private static GoldService _inst;


    public static GoldService Instance
    {
        get
        {
            if (_inst) return _inst;
#if UNITY_2023_1_OR_NEWER
            _inst = FindFirstObjectByType<GoldService>(FindObjectsInactive.Exclude);
#else
_inst = FindObjectByType<GoldService>();
if (_inst == null) _inst = FindObjectOfType<GoldService>();
#endif
            if (_inst) return _inst;
            var go = new GameObject("[GoldService]");
            _inst = go.AddComponent<GoldService>();
            DontDestroyOnLoad(go); _inst.Load();
            return _inst;
        }
    }

    public event Action<int> OnGoldChanged;
    private const string KEY_TOTAL = "GOLD_TOTAL";
    public int TotalGold { get; private set; }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Prewarm() => _ = Instance;


    private void Awake()
    {
        if (_inst && _inst != this) { Destroy(gameObject); return; }
        _inst = this; DontDestroyOnLoad(gameObject);
        if (TotalGold == 0) Load();
    }


    public void AddGold(int amount)
    {
        if (amount == 0) return;
        TotalGold = Mathf.Max(0, TotalGold + amount);
        OnGoldChanged?.Invoke(TotalGold); Save();
    }


    public bool SpendGold(int amount)
    {
        if (amount <= 0 || TotalGold < amount) return false;
        TotalGold -= amount; OnGoldChanged?.Invoke(TotalGold); Save(); return true;
    }


    public void Load()
    {
        TotalGold = PlayerPrefs.GetInt(KEY_TOTAL, 0);
        OnGoldChanged?.Invoke(TotalGold);
    }


    public void Save()
    {
        PlayerPrefs.SetInt(KEY_TOTAL, TotalGold); PlayerPrefs.Save();
    }
}