/*
// File: Assets/Scripts/Systems/RunStarterAndBindTimer.cs
using UnityEngine;

public class RunStarterAndBindTimer : MonoBehaviour
{
    private void Start()
    {
        if (GameTimer.Instance != null)
            GameTimer.Instance.StartRun();

        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null && GameTimer.Instance != null)
            ph.OnDied += () => GameTimer.Instance.Stop(); // ✅ Ölümde LastRun set edilir
    }
}
*/

using UnityEngine;


public class RunStarterAndBindTimer : MonoBehaviour
{
    private void Start()
    {
        if (GameTimer.Instance != null) GameTimer.Instance.StartRun();
        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null && GameTimer.Instance != null)
            ph.OnDied += () => GameTimer.Instance.Stop();
    }
}
