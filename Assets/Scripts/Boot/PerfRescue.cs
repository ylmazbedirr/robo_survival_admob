// ─────────────────────────────────────────────────────────────────────────────
// File: Assets/Scripts/Boot/PerfRescue.cs
// Purpose: If early avg-FPS is low or there are spikes, apply conservative runtime quality cuts.
// NOTE: Does NOT set Application.targetFrameRate (AQT yönetiyor).
// ─────────────────────────────────────────────────────────────────────────────
using UnityEngine;
using System.Collections;


public sealed class PerfRescue : MonoBehaviour
{
        public static PerfRescue Instance { get; private set; }


        [Header("Trigger thresholds")] public float targetFps = 50f; public float spikeSeconds = 0.05f;
        [Header("Sampler")] public float warmupSeconds = 2f; public float sampleSeconds = 3f;


        private void Awake() { if (Instance && Instance != this) { Destroy(gameObject); return; } Instance = this; DontDestroyOnLoad(gameObject); }


        private void Start() => StartCoroutine(CoMonitorAndApply());


        private IEnumerator CoMonitorAndApply()
        {
                float t = 0f; while (t < warmupSeconds) { t += Time.unscaledDeltaTime; yield return null; }
                float sum = 0f; int n = 0; float maxDt = 0f; float until = Time.realtimeSinceStartup + sampleSeconds;
                while (Time.realtimeSinceStartup < until) { yield return null; float dt = Time.unscaledDeltaTime; if (dt > 0f) { sum += 1f / dt; n++; } if (dt > maxDt) maxDt = dt; }
                float avg = (n > 0 ? sum / n : 0f);
                if (avg < targetFps || maxDt > spikeSeconds) ApplyDownshift();
        }

        private void ApplyDownshift()
        {
                QualitySettings.vSyncCount = 0; // AQT targetFrameRate'i yönetsin
                                                // (visual-safe) do not change pixelLightCount
                QualitySettings.shadowCascades = 0; QualitySettings.shadowDistance = Mathf.Min(QualitySettings.shadowDistance, 20f);
                /*
                QualitySettings.antiAliasing = 0; QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable; QualitySettings.lodBias = Mathf.Min(QualitySettings.lodBias, 0.6f);
#if UNITY_2021_1_OR_NEWER
                QualitySettings.globalTextureMipmapLimit = Mathf.Max(QualitySettings.globalTextureMipmapLimit, 1);
#else
QualitySettings.masterTextureLimit = Mathf.Max(QualitySettings.masterTextureLimit,1);
#endif
                var particles =
#if UNITY_2023_1_OR_NEWER
                UnityEngine.Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
UnityEngine.Object.FindObjectsOfType<ParticleSystem>();
#endif
                foreach (var p in particles) { var main = p.main; main.maxParticles = Mathf.Min(main.maxParticles, 256); var em = p.emission; em.rateOverTimeMultiplier *= 0.6f; var tr = p.trails; if (tr.enabled) tr.ratio *= 0.7f; }
                // (visual-safe) do not touch camera HDR/MSAA here
                QualitySettings.softParticles = false; QualitySettings.realtimeReflectionProbes = false; Shader.globalMaximumLOD = Mathf.Min(Shader.globalMaximumLOD, 300);
                Debug.Log("[PerfRescue] Runtime downshift applied.");
                
                */
        }
}