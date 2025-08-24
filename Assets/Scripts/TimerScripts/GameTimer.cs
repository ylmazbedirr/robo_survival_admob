/*
// File: Assets/Scripts/Systems/GameTimer.cs
using UnityEngine;
using System;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    private const string BestKey = "BEST_TIME_SECONDS";

    public float Elapsed { get; private set; }
    public float LastRun { get; private set; } // ✅ Ölünce/bitince set edilir
    public float Best => PlayerPrefs.GetFloat(BestKey, 0f);
    public bool Running { get; private set; }

    public event Action<float> OnTick;
    public event Action<float> OnStopped;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!Running) return;
        Elapsed += Time.deltaTime;
        OnTick?.Invoke(Elapsed);
    }

    public void StartRun()
    {
        Elapsed = 0f;
        Running = true;
        OnTick?.Invoke(Elapsed);
        // LastRun burada sıfırlanmaz; death menü eski değeri gösterebilir. İstersen:
        // LastRun = 0f;
    }

    public void Stop()
    {
        if (!Running) return;
        Running = false;

        LastRun = Elapsed;           // ✅ Son oyun süresi
        TrySaveBest(Elapsed);        // ✅ Best ile karşılaştır ve kaydet
        OnStopped?.Invoke(Elapsed);
    }

    public void CommitBestNow()      // retry/home vb. butonlardan güvenli çağrı
    {
        TrySaveBest(Elapsed);
    }

    private void TrySaveBest(float value)
    {
        if (value > Best)
        {
            PlayerPrefs.SetFloat(BestKey, value);
            PlayerPrefs.Save();
        }
    }

    public static string FormatMMSS(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);
        int mm = total / 60;
        int ss = total % 60;
        return $"{mm:00}:{ss:00}";
    }
}

*/


using UnityEngine;
using System;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }


    private const string BestKey = "BEST_TIME_SECONDS";


    public float Elapsed { get; private set; }
    public float LastRun { get; private set; }
    public float Best => PlayerPrefs.GetFloat(BestKey, 0f);
    public bool Running { get; private set; }


    public event Action<float> OnTick;
    public event Action<float> OnStopped;


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (!Running) return;
        Elapsed += Time.deltaTime;
        OnTick?.Invoke(Elapsed);
    }


    public void StartRun()
    {
        Elapsed = 0f; Running = true; OnTick?.Invoke(Elapsed);
    }


    public void Stop()
    {
        if (!Running) return;
        Running = false; LastRun = Elapsed; TrySaveBest(Elapsed); OnStopped?.Invoke(Elapsed);
    }


    public void CommitBestNow() { TrySaveBest(Elapsed); }


    private void TrySaveBest(float value)
    {
        if (value > Best)
        {
            PlayerPrefs.SetFloat(BestKey, value); PlayerPrefs.Save();
        }
    }


    public static string FormatMMSS(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);
        int mm = total / 60; int ss = total % 60; return $"{mm:00}:{ss:00}";
    }
}