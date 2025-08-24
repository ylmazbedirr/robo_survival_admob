/*

// File: Assets/Scripts/Boot/PerfRescue.cs
// Purpose: When FPS is low, auto-apply conservative runtime quality overrides.
// One-file, zero-setup: it bootstraps itself before first scene.

using UnityEngine;
using System.Collections;

public sealed class PerfRescue : MonoBehaviour
{
    public static PerfRescue Instance { get; private set; }

    [Header("Trigger thresholds")]
    [Tooltip("Average FPS below this -> apply downshift")] public float targetFps = 50f;
    [Tooltip("If any frame exceeds this unscaled delta (sec), treat as spike")] public float spikeSeconds = 0.05f; // 50ms

    [Header("Sampler")]
    public float warmupSeconds = 2f;   // ignore initial load
    public float sampleSeconds = 3f;   // averaging window

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        var go = new GameObject("[PerfRescue]");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<PerfRescue>();
    }

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start() => StartCoroutine(CoMonitorAndApply());

    private IEnumerator CoMonitorAndApply()
    {
        // initial chill period
        float t = 0f; while (t < warmupSeconds) { t += Time.unscaledDeltaTime; yield return null; }

        // sample average fps
        float sumFps = 0f; int n = 0; float maxDt = 0f; float until = Time.realtimeSinceStartup + sampleSeconds;
        while (Time.realtimeSinceStartup < until)
        {
            yield return null;
            float dt = Time.unscaledDeltaTime;
            if (dt > 0f) { sumFps += 1f / dt; n++; }
            if (dt > maxDt) maxDt = dt;
        }

        float avg = (n > 0) ? (sumFps / n) : 0f;
        if (avg < targetFps || maxDt > spikeSeconds)
        {
            ApplyDownshift();
        }
    }

    private void ApplyDownshift()
    {
        // Frame pacing
        QualitySettings.vSyncCount = 0;             // don't let driver stall CPU
        if (Application.targetFrameRate < 60) Application.targetFrameRate = 60;

        // Lighting & shadows
        QualitySettings.pixelLightCount = Mathf.Min(QualitySettings.pixelLightCount, 0);
        QualitySettings.shadowCascades = 0;
        QualitySettings.shadowDistance = Mathf.Min(QualitySettings.shadowDistance, 15f);

        // Sampling / textures
        QualitySettings.antiAliasing = 0;           // MSAA off
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
#if UNITY_2021_1_OR_NEWER
        QualitySettings.globalTextureMipmapLimit = Mathf.Max(QualitySettings.globalTextureMipmapLimit, 1); // downscale to 1/2
#else
        QualitySettings.masterTextureLimit = Mathf.Max(QualitySettings.masterTextureLimit, 1); // downscale to 1/2
#endif
        QualitySettings.lodBias = Mathf.Min(QualitySettings.lodBias, 0.6f);

        // Particles (best-effort): reduce pressure
#if UNITY_2023_1_OR_NEWER
        var particles = Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
        var particles = Object.FindObjectsOfType<ParticleSystem>();
#endif
        foreach (var p in particles)
        {
            var main = p.main; main.maxParticles = Mathf.Min(main.maxParticles, 256);
            var emission = p.emission; emission.rateOverTimeMultiplier *= 0.6f;
            var trails = p.trails; if (trails.enabled) trails.ratio *= 0.7f;
        }

        // Cameras: turn off expensive toggles
#if UNITY_2023_1_OR_NEWER
        var cams = Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
        var cams = Object.FindObjectsOfType<Camera>();
#endif
        foreach (var c in cams)
        {
            c.allowHDR = false; c.allowMSAA = false;
        }

        // Soft particles & reflections
        QualitySettings.softParticles = false;
        QualitySettings.realtimeReflectionProbes = false;

        // Lower shader max LOD for cheaper variants
        Shader.globalMaximumLOD = Mathf.Min(Shader.globalMaximumLOD, 300);

        Debug.Log("[PerfRescue] Runtime downshift applied.");
    }
}

*/
