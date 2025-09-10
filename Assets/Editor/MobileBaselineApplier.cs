// File: Assets/Editor/MobileBaselineApplier.cs
// Menu: Tools → Mobile Baseline
// Goal: Apply mobile-friendly defaults to current project (Android focus) in one click.
// Why: Reproduces "3D Mobile" template-style settings without recreating the project.

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Linq;
using System.Reflection;

public static class MobileBaselineApplier
{
    private const string MenuRoot = "Tools/Mobile Baseline/";

    [MenuItem(MenuRoot + "Apply (GLES3)")]
    public static void ApplyGLES3() => ApplyInternal(useVulkan:false);

    [MenuItem(MenuRoot + "Apply (Vulkan)")]
    public static void ApplyVulkan() => ApplyInternal(useVulkan:true);

    [MenuItem(MenuRoot + "Report Current Settings")] 
    public static void Report() => ReportInternal();

    private static void ApplyInternal(bool useVulkan)
    {
        Undo.IncrementCurrentGroup();
        var grp = Undo.GetCurrentGroup();
        try
        {
            // PlayerSettings – Android target
            var nbt = NamedBuildTarget.Android;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, ManagedStrippingLevel.Medium);
            PlayerSettings.stripEngineCode = true;
            // removed ApiCompatibilityLevel set – not needed on Unity 6000

            // Graphics APIs (single API)
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
            var gapi = useVulkan ? new[] { GraphicsDeviceType.Vulkan } : new[] { GraphicsDeviceType.OpenGLES3 };
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, gapi);

            // Quality
            QualitySettings.vSyncCount = 0;                         // pacing engine'e kalsın
            QualitySettings.antiAliasing = 0;                       // MSAA off
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
#if UNITY_2021_1_OR_NEWER
            QualitySettings.globalTextureMipmapLimit = Mathf.Max(QualitySettings.globalTextureMipmapLimit, 1); // 1/2 textures
#else
            QualitySettings.masterTextureLimit = Mathf.Max(QualitySettings.masterTextureLimit, 1);
#endif
            QualitySettings.lodBias = Mathf.Min(QualitySettings.lodBias, 0.75f);
            QualitySettings.shadowDistance = Mathf.Min(QualitySettings.shadowDistance, 25f);
            QualitySettings.shadowCascades = Mathf.Min(QualitySettings.shadowCascades, 2);

            // URP pipeline tweaks (if present)
            TryTuneURP(renderScale:0.8f, hdr:false, msaa:1, additionalLightsPerObj:0);

            // Open scenes: camera flags
            foreach (var scene in Enumerable.Range(0, UnityEngine.SceneManagement.SceneManager.sceneCount)
                     .Select(i => UnityEngine.SceneManagement.SceneManager.GetSceneAt(i))
                     .Where(s => s.isLoaded))
            {
                foreach (var root in scene.GetRootGameObjects())
                {
                    foreach (var cam in root.GetComponentsInChildren<Camera>(true))
                    {
                        Undo.RecordObject(cam, "MobileBaseline Camera");
                        cam.allowHDR = false; cam.allowMSAA = false;
                        EditorUtility.SetDirty(cam);
                    }
                }
            }

            // Save scenes if asked
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog("Mobile Baseline",
                $"Applied mobile defaults (\nAPI={(useVulkan?"Vulkan":"GLES3")}\nURP renderScale=0.8, HDR off, MSAA x1\nQuality: AA off, Aniso off, Texture 1/2, LOD 0.75, Shadows ≤25m, 0–2 cascades).",
                "OK");
        }
        catch (Exception e)
        {
            Debug.LogError($"[MobileBaseline] Error: {e.Message}\n{e}");
        }
        finally { Undo.CollapseUndoOperations(grp); }
    }

    private static void TryTuneURP(float renderScale, bool hdr, int msaa, int additionalLightsPerObj)
    {
        var rp = GraphicsSettings.currentRenderPipeline;
        if (!rp) { Debug.Log("[MobileBaseline] No SRP asset set. Skipping URP tuning."); return; }
        var t = rp.GetType();
        if (t.FullName != "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset")
        {
            Debug.Log($"[MobileBaseline] Current RP is not URP: {t.FullName}. Skipping URP tuning.");
            return;
        }

        var so = new SerializedObject(rp);
        // Common URP properties (names are stable across URP 10+)
        SetFloatProp(so, "m_RenderScale", Mathf.Clamp(renderScale, 0.5f, 1.2f));
        SetBoolProp (so, "m_SupportsHDR", hdr);
        SetIntProp  (so, "m_MSAA", Mathf.Clamp(msaa, 1, 8));
        SetIntProp  (so, "m_AdditionalLightsPerObject", Mathf.Clamp(additionalLightsPerObj, 0, 8));
        SetBoolProp (so, "m_SoftShadowsSupported", false);
        SetIntProp  (so, "m_MainLightShadowmapResolution", 1024);
        so.ApplyModifiedPropertiesWithoutUndo();

        // Try to disable renderer features (post-processing etc.)
        TryDisableURPRendererFeatures(rp);

        EditorUtility.SetDirty(rp);
    }

    private static void TryDisableURPRendererFeatures(RenderPipelineAsset rp)
    {
        // UniversalRendererData lives in URP editor assembly; access via reflection best-effort.
        try
        {
            var tURPAsset = rp.GetType();
            var propRendererDataList = tURPAsset.GetProperty("rendererDataList", BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
            var list = propRendererDataList?.GetValue(rp) as Array;
            if (list == null) return;
            foreach (var rd in list)
            {
                if (rd == null) continue;
                var tRD = rd.GetType();
                var propFeatures = tRD.GetField("rendererFeatures", BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
                var features = propFeatures?.GetValue(rd) as System.Collections.IList;
                if (features == null) continue;
                foreach (var f in features)
                {
                    if (f == null) continue;
                    var tF = f.GetType();
                    var propActive = tF.GetField("m_Active", BindingFlags.Instance|BindingFlags.NonPublic) ?? tF.GetField("active", BindingFlags.Instance|BindingFlags.Public);
                    if (propActive != null) propActive.SetValue(f, false);
                }
                EditorUtility.SetDirty(rd as UnityEngine.Object);
            }
        }
        catch { /* if URP internals change, skip silently */ }
    }

    private static void SetFloatProp(SerializedObject so, string name, float value)
    {
        var p = so.FindProperty(name); if (p != null) p.floatValue = value;
    }
    private static void SetIntProp(SerializedObject so, string name, int value)
    {
        var p = so.FindProperty(name); if (p != null) p.intValue = value;
    }
    private static void SetBoolProp(SerializedObject so, string name, bool value)
    {
        var p = so.FindProperty(name); if (p != null) p.boolValue = value;
    }

    private static void ReportInternal()
    {
        var nbt = NamedBuildTarget.Android;
        var apis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
        var rp = GraphicsSettings.currentRenderPipeline;
        string rpInfo = rp ? rp.GetType().FullName : "<Builtin>";

        Debug.Log($"[MobileBaseline Report]\n" +
                  $"Scripting: {PlayerSettings.GetScriptingBackend(nbt)}\n" +
                  $"ManagedStripping: {PlayerSettings.GetManagedStrippingLevel(nbt)}  StripEngine: {PlayerSettings.stripEngineCode}\n" +
                  $"Graphics API: {string.Join(", ", apis.Select(a=>a.ToString()))}\n" +
                  $"Quality: vSync={QualitySettings.vSyncCount} AA={QualitySettings.antiAliasing} Aniso={QualitySettings.anisotropicFiltering}\n" +
#if UNITY_2021_1_OR_NEWER
                  $"TextureMipLimit={QualitySettings.globalTextureMipmapLimit} LODBias={QualitySettings.lodBias}\n" +
#else
                  $"TextureMipLimit={QualitySettings.masterTextureLimit} LODBias={QualitySettings.lodBias}\n" +
#endif
                  $"Shadows: dist={QualitySettings.shadowDistance} cascades={QualitySettings.shadowCascades}\n" +
                  $"RenderPipeline: {rpInfo}");

        if (rp)
        {
            try
            {
                var so = new SerializedObject(rp);
                var rs = so.FindProperty("m_RenderScale")?.floatValue;
                var hdr = so.FindProperty("m_SupportsHDR")?.boolValue;
                var msaa = so.FindProperty("m_MSAA")?.intValue;
                var add = so.FindProperty("m_AdditionalLightsPerObject")?.intValue;
                Debug.Log($"[URP] renderScale={rs} HDR={hdr} MSAA={msaa} AdditionalLightsPerObj={add}");
            }
            catch { }
        }
    }
}
#endif
