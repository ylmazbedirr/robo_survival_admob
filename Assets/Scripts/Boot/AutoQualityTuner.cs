using UnityEngine;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections;

public sealed class AutoQualityTuner : MonoBehaviour
{
    private const string PP_KEY = "AQT_PROFILE_";
    public enum Tier { Low = 0, Mid = 1, High = 2 }

    [Header("Sampling")] public float warmupSeconds = 2f; public float sampleSeconds = 4f; public float reevaluateAfter = 30f;
    [Header("Thresholds (avg FPS)")] public float midThreshold = 55f; public float lowThreshold = 35f;
    [Header("Profiles")]
    public Profile low = new Profile(0, 0.55f, false, 0.5f, 2);  // Aşırı düşük cihaz
    public Profile mid = new Profile(0, 0.75f, false, 0.8f, 1);  // Orta
    public Profile high = new Profile(0, 0.9f, true, 1.0f, 0);   // Güçlü cihazlar

    [Header("Debug")] public bool showOverlay;
    [Header("Visual Safety")]
    [SerializeField] private bool touchHDR = false;
    [SerializeField] private bool touchMSAA = false;

    [System.Serializable]
    public struct Profile
    {
        public int targetFps;
        public float renderScale;
        public bool shadows;
        public float lodBias;
        public int texMipLimit;

        public Profile(int fps, float scale, bool shad, float lod, int mip)
        {
            targetFps = fps;
            renderScale = scale;
            shadows = shad;
            lodBias = lod;
            texMipLimit = mip;
        }
    }

    private Tier _applied; private bool _appliedOnce; private string _ppKey;

    private void Awake()
    {
        _ppKey = PP_KEY + (SystemInfo.deviceModel ?? "device");
        if (PlayerPrefs.HasKey(_ppKey))
        {
            var cached = (Tier)PlayerPrefs.GetInt(_ppKey, (int)Tier.Mid);
            //
            Application.targetFrameRate = 999; // ya da 120
            //
            ApplyTier(cached);
            _appliedOnce = true;
        }
        StartCoroutine(CoTune());
    }

    private IEnumerator CoTune()
    {
        float t = 0f; while (t < warmupSeconds) { t += Time.unscaledDeltaTime; yield return null; }
        float sum = 0f; int n = 0; float until = Time.realtimeSinceStartup + sampleSeconds;
        while (Time.realtimeSinceStartup < until)
        {
            yield return null;
            var dt = Time.unscaledDeltaTime;
            if (dt > 0f) { sum += 1f / dt; n++; }
        }
        float avg = (n > 0 ? sum / n : 0f);
        var tier = Decide(avg);
        if (!_appliedOnce || tier != _applied) ApplyTier(tier);
        if (reevaluateAfter > 0f) { yield return new WaitForSecondsRealtime(reevaluateAfter); StartCoroutine(CoTune()); }
    }

    private Tier Decide(float avgFps)
    {
        if (avgFps < lowThreshold) return Tier.Low;
        if (avgFps < midThreshold) return Tier.Mid;
        return Tier.High;
    }

    private void ApplyTier(Tier tier)
    {
        _applied = tier; _appliedOnce = true;
        PlayerPrefs.SetInt(_ppKey, (int)tier); PlayerPrefs.Save();
        var p = tier == Tier.Low ? low : (tier == Tier.Mid ? mid : high);

        // vSync kapalı, FPS limiti yok
        QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = p.targetFps; // KAPALI

        TrySetRenderScale(p.renderScale);

        // Gölge ayarları
        QualitySettings.shadowCascades = p.shadows ? 2 : 0;
        QualitySettings.shadowDistance = p.shadows ? Mathf.Min(QualitySettings.shadowDistance, 25f) : 0f;

        // Texture, LOD
#if UNITY_2021_1_OR_NEWER
        QualitySettings.globalTextureMipmapLimit = Mathf.Max(QualitySettings.globalTextureMipmapLimit, p.texMipLimit);
#else
        QualitySettings.masterTextureLimit = Mathf.Max(QualitySettings.masterTextureLimit, p.texMipLimit);
#endif
        QualitySettings.lodBias = Mathf.Min(QualitySettings.lodBias, p.lodBias);

        // HDR ve MSAA kapatma (isteğe bağlı)
        if (touchHDR || touchMSAA)
        {
            var cams = GetAllCameras();
            foreach (var c in cams)
            {
                if (touchHDR) c.allowHDR = false;
                if (touchMSAA) c.allowMSAA = false;
            }
        }

        if (showOverlay)
            Debug.Log($"[AQT] Applied Tier={tier} → {p.renderScale * 100}% scale, Shadows={p.shadows}");
    }

    private void TrySetRenderScale(float scale)
    {
        try
        {
            var rp = GraphicsSettings.currentRenderPipeline;
            if (rp != null)
            {
                var t = rp.GetType();
                if (t.FullName == "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset")
                {
                    var prop = t.GetProperty("renderScale", BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(rp, Mathf.Clamp(scale, 0.5f, 1.4f));
                        return;
                    }
                }
            }
        }
        catch { }

        try { ScalableBufferManager.ResizeBuffers(scale, scale); } catch { }
    }

    private Camera[] GetAllCameras()
    {
#if UNITY_2023_1_OR_NEWER
        return UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
        return Camera.allCamerasCount > 0 ? Camera.allCameras : GameObject.FindObjectsOfType<Camera>();
#endif
    }
}
