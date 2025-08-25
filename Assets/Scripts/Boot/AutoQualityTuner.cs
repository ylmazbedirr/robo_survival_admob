// ─────────────────────────────────────────────────────────────────────────────
// File: Assets/Scripts/Boot/AutoQualityTuner.cs
// Purpose: Measure early-FPS → choose Low/Mid/High profile (renderScale, shadows, LOD, textures, target FPS).
// NOTE: No self-bootstrap attribute here; PerfBootstrapper creates it once.
// ─────────────────────────────────────────────────────────────────────────────
using UnityEngine;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections;


public sealed class AutoQualityTuner : MonoBehaviour
{
    private const string PP_KEY = "AQT_PROFILE_"; // + deviceModel
    public enum Tier { Low = 0, Mid = 1, High = 2 }


    [Header("Sampling")] public float warmupSeconds = 2f; public float sampleSeconds = 4f; public float reevaluateAfter = 20f;
    [Header("Thresholds (avg FPS)")] public float midThreshold = 50f; public float lowThreshold = 30f;
    [Header("Profiles")] public Profile low = new Profile(30, 0.70f, false, 0.55f, 2);
    public Profile mid = new Profile(60, 0.85f, true, 0.75f, 1);
    public Profile high = new Profile(60, 0.95f, true, 1.00f, 0);
    [Header("Debug")] public bool showOverlay;


    [Header("Visual Safety")]
    [Tooltip("If ON, AQT will force-disable HDR on Cameras. Leave OFF to preserve your look.")]
    [SerializeField] private bool touchHDR = false;
    [Tooltip("If ON, AQT will force-disable MSAA on Cameras. Leave OFF to preserve your look.")]
    [SerializeField] private bool touchMSAA = false;


    [System.Serializable]
    public struct Profile { public int targetFps; public float renderScale; public bool shadows; public float lodBias; public int texMipLimit; public Profile(int fps, float scale, bool shad, float lod, int mip) { targetFps = fps; renderScale = scale; shadows = shad; lodBias = lod; texMipLimit = mip; } }


    private Tier _applied; private bool _appliedOnce; private string _ppKey;


    private void Awake()
    {
        _ppKey = PP_KEY + (SystemInfo.deviceModel ?? "device");
        if (PlayerPrefs.HasKey(_ppKey)) { var cached = (Tier)PlayerPrefs.GetInt(_ppKey, (int)Tier.Mid); ApplyTier(cached); _appliedOnce = true; }
        StartCoroutine(CoTune());
    }

    private IEnumerator CoTune()
    {
        float t = 0f; while (t < warmupSeconds) { t += Time.unscaledDeltaTime; yield return null; }
        float sum = 0f; int n = 0; float until = Time.realtimeSinceStartup + sampleSeconds;
        while (Time.realtimeSinceStartup < until) { yield return null; var dt = Time.unscaledDeltaTime; if (dt > 0f) { sum += 1f / dt; n++; } }
        float avg = (n > 0 ? sum / n : 0f);
        var tier = Decide(avg);
        if (!_appliedOnce || tier != _applied) ApplyTier(tier);
        if (reevaluateAfter > 0f) { yield return new WaitForSecondsRealtime(reevaluateAfter); StartCoroutine(CoTune()); }
    }


    private Tier Decide(float avgFps) { if (avgFps < lowThreshold) return Tier.Low; if (avgFps < midThreshold) return Tier.Mid; return Tier.High; }


    /*
        private void ApplyTier(Tier tier)
        {
            _applied = tier; _appliedOnce = true; PlayerPrefs.SetInt(_ppKey, (int)tier); PlayerPrefs.Save();
            var p = tier == Tier.Low ? low : (tier == Tier.Mid ? mid : high);


            // pacing (AQT tek otorite)
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = p.targetFps;


            TrySetRenderScale(p.renderScale);


            QualitySettings.shadowCascades = p.shadows ? 2 : 0;
            QualitySettings.shadowDistance = p.shadows ? Mathf.Min(QualitySettings.shadowDistance, 25f) : 0f;
            QualitySettings.antiAliasing = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
    #if UNITY_2021_1_OR_NEWER
            QualitySettings.globalTextureMipmapLimit = Mathf.Max(QualitySettings.globalTextureMipmapLimit, p.texMipLimit);
    #else
            QualitySettings.masterTextureLimit = Mathf.Max(QualitySettings.masterTextureLimit, p.texMipLimit);
    #endif
            QualitySettings.lodBias = Mathf.Min(QualitySettings.lodBias, p.lodBias);

            var cams = GetAllCameras();
            foreach (var c in cams)
            {
                if (touchHDR) c.allowHDR = false;
                if (touchMSAA) c.allowMSAA = false;
            }
            if (showOverlay) Debug.Log($"[AQT] tier={tier} → {p.targetFps} FPS @ scale {p.renderScale}");
        }
    */

    private void ApplyTier(Tier tier)
    {
        _applied = tier; _appliedOnce = true;
        PlayerPrefs.SetInt(_ppKey, (int)tier); PlayerPrefs.Save();
        var p = tier == Tier.Low ? low : (tier == Tier.Mid ? mid : high);

        // 1) FPS pacing
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = p.targetFps;

        // 2) Sadece çözünürlük ölçeği (URP varsa renderScale, yoksa S.B.Manager)
        TrySetRenderScale(p.renderScale);

        // 3) Görsel-güvenli: AŞAĞIDAKİLERİ KULLANMA
        // QualitySettings.shadowCascades = p.shadows ? 2 : 0;
        // QualitySettings.shadowDistance = p.shadows ? Mathf.Min(QualitySettings.shadowDistance, 25f) : 0f;
        // QualitySettings.antiAliasing = 0;
        // QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        // #if UNITY_2021_1_OR_NEWER
        // QualitySettings.globalTextureMipmapLimit = Mathf.Max(QualitySettings.globalTextureMipmapLimit, p.texMipLimit);
        // #else
        // QualitySettings.masterTextureLimit = ...
        // #endif
        // QualitySettings.lodBias = Mathf.Min(QualitySettings.lodBias, p.lodBias);

        // Kameralara da DOKUNMA (HDR/MSAA)
        // var cams = GetAllCameras(); foreach (var c in cams) { c.allowHDR = false; c.allowMSAA = false; }

        if (showOverlay) Debug.Log($"[AQT] tier={tier} → {p.targetFps} FPS @ scale {p.renderScale}");
    }

    private void TrySetRenderScale(float scale)
    {
        try { var rp = GraphicsSettings.currentRenderPipeline; if (rp != null) { var t = rp.GetType(); if (t.FullName == "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset") { var prop = t.GetProperty("renderScale", BindingFlags.Public | BindingFlags.Instance); if (prop != null && prop.CanWrite) { prop.SetValue(rp, Mathf.Clamp(scale, 0.5f, 1.4f)); return; } } } } catch { }
        try { ScalableBufferManager.ResizeBuffers(scale, scale); } catch { }
    }


    private Camera[] GetAllCameras()
    {
#if UNITY_2023_1_OR_NEWER
        return UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
return Camera.allCamerasCount>0?Camera.allCameras:GameObject.FindObjectsOfType<Camera>();
#endif
    }
}